using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Cpp.Ast;
using System.Reflection;
using Mono.Cecil;

namespace ICSharpCode.NRefactory.Cpp.Visitors
{
    public interface IEnvironmentProvider
    {
        string RootNamespace { get; }
        string GetTypeNameForAttribute(CSharp.Attribute attribute);
        TypeKind GetTypeKindForAstType(CSharp.AstType type);
        TypeCode ResolveExpression(CSharp.Expression expression);
        bool? IsReferenceType(CSharp.Expression expression);

        IType ResolveType(AstType type, TypeDeclaration entity = null);
    }

    public class CSharpToCppConverterVisitor : CSharp.IAstVisitor<object, Cpp.AstNode>
    {
        //Auxiliar list to change the array specifiers from one branch to another
        private List<CSharp.ArraySpecifier> arraySpecifiers = new List<CSharp.ArraySpecifier>();

        IEnvironmentProvider provider;
        Stack<BlockStatement> blocks;
        Stack<TypeDeclaration> types;
        Stack<MemberInfo> members;


        public CSharpToCppConverterVisitor(IEnvironmentProvider provider)
        {
            this.provider = provider;
            this.blocks = new Stack<BlockStatement>();
            this.types = new Stack<TypeDeclaration>();
            this.members = new Stack<MemberInfo>();

            Resolver.Restart();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitAnonymousMethodExpression(CSharp.AnonymousMethodExpression anonymousMethodExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitUndocumentedExpression(CSharp.UndocumentedExpression undocumentedExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitArrayCreateExpression(CSharp.ArrayCreateExpression arrayCreateExpression, object data)
        {
            var expr = new ArrayCreateExpression()
            {
                Type = (AstType)arrayCreateExpression.Type.AcceptVisitor(this, data),
                Initializer = (ArrayInitializerExpression)arrayCreateExpression.Initializer.AcceptVisitor(this, data)
            };
            ConvertNodes(arrayCreateExpression.Arguments, expr.Arguments);
            ConvertNodes(arrayCreateExpression.AdditionalArraySpecifiers, expr.AdditionalArraySpecifiers);

            return EndNode(arrayCreateExpression, expr);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitArrayInitializerExpression(CSharp.ArrayInitializerExpression arrayInitializerExpression, object data)
        {
            ArrayInitializerExpression aiexp = new ArrayInitializerExpression();
            ConvertNodes(arrayInitializerExpression.Elements, aiexp.Elements);
            return aiexp;
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitAsExpression(CSharp.AsExpression asExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitAssignmentExpression(CSharp.AssignmentExpression assignmentExpression, object data)
        {
            var left = (Expression)assignmentExpression.Left.AcceptVisitor(this, data);
            var op = AssignmentOperatorType.Any;
            var right = (Expression)assignmentExpression.Right.AcceptVisitor(this, data);

            switch (assignmentExpression.Operator)
            {
                case ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.Assign:
                    op = AssignmentOperatorType.Assign;
                    break;
                case ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.Add:
                    op = AssignmentOperatorType.Add;
                    break;
                case ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.Subtract:
                    op = AssignmentOperatorType.Subtract;
                    break;
                case ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.Multiply:
                    op = AssignmentOperatorType.Multiply;
                    break;
                case ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.Divide:
                    op = AssignmentOperatorType.Divide;
                    break;
                case ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.Modulus:
                    op = AssignmentOperatorType.Assign;
                    right = new BinaryOperatorExpression((Expression)left.Clone(), BinaryOperatorType.Modulus, right);
                    break;
                case ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.ShiftLeft:
                    op = AssignmentOperatorType.ShiftLeft;
                    break;
                case ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.ShiftRight:
                    op = AssignmentOperatorType.ShiftRight;
                    break;
                case ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.BitwiseAnd:
                    op = AssignmentOperatorType.Assign;
                    right = new BinaryOperatorExpression((Expression)left.Clone(), BinaryOperatorType.BitwiseAnd, right);
                    break;
                case ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.BitwiseOr:
                    op = AssignmentOperatorType.Assign;
                    right = new BinaryOperatorExpression((Expression)left.Clone(), BinaryOperatorType.BitwiseOr, right);
                    break;
                case ICSharpCode.NRefactory.CSharp.AssignmentOperatorType.ExclusiveOr:
                    op = AssignmentOperatorType.Assign;
                    right = new BinaryOperatorExpression((Expression)left.Clone(), BinaryOperatorType.ExclusiveOr, right);
                    break;
                default:
                    throw new Exception("Invalid value for AssignmentOperatorType: " + assignmentExpression.Operator);
            }

            var expr = new AssignmentExpression(left, op, right);
            return EndNode(assignmentExpression, expr);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitBaseReferenceExpression(CSharp.BaseReferenceExpression baseReferenceExpression, object data)
        {
            return EndNode(baseReferenceExpression, new BaseReferenceExpression());
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitBinaryOperatorExpression(CSharp.BinaryOperatorExpression binaryOperatorExpression, object data)
        {
            var left = (Expression)binaryOperatorExpression.Left.AcceptVisitor(this, data);
            var op = BinaryOperatorType.Any;
            var right = (Expression)binaryOperatorExpression.Right.AcceptVisitor(this, data);

            switch (binaryOperatorExpression.Operator)
            {
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.BitwiseAnd:
                    op = BinaryOperatorType.BitwiseAnd;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.BitwiseOr:
                    op = BinaryOperatorType.BitwiseOr;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.ConditionalAnd:
                    op = BinaryOperatorType.ConditionalAnd;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.ConditionalOr:
                    op = BinaryOperatorType.ConditionalOr;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.ExclusiveOr:
                    op = BinaryOperatorType.ExclusiveOr;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.GreaterThan:
                    op = BinaryOperatorType.GreaterThan;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.GreaterThanOrEqual:
                    op = BinaryOperatorType.GreaterThanOrEqual;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.Equality:
                    op = BinaryOperatorType.Equality;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.InEquality:
                    op = BinaryOperatorType.InEquality;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.LessThan:
                    op = BinaryOperatorType.LessThan;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.LessThanOrEqual:
                    op = BinaryOperatorType.LessThanOrEqual;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.Add:
                    // TODO might be string concatenation
                    op = BinaryOperatorType.Add;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.Subtract:
                    op = BinaryOperatorType.Subtract;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.Multiply:
                    op = BinaryOperatorType.Multiply;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.Divide:
                    op = BinaryOperatorType.Divide;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.Modulus:
                    op = BinaryOperatorType.Modulus;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.ShiftLeft:
                    op = BinaryOperatorType.ShiftLeft;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.ShiftRight:
                    op = BinaryOperatorType.ShiftRight;
                    break;
                case ICSharpCode.NRefactory.CSharp.BinaryOperatorType.NullCoalescing:
                    var nullCoalescing = new ConditionalExpression
                    {
                        TrueExpression = left,
                        FalseExpression = right
                    };
                    return EndNode(binaryOperatorExpression, nullCoalescing);
                default:
                    throw new Exception("Invalid value for BinaryOperatorType: " + binaryOperatorExpression.Operator);
            }

            return EndNode(binaryOperatorExpression, new BinaryOperatorExpression(left, op, right));
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitCastExpression(CSharp.CastExpression castExpression, object data)
        {
            CastExpression cexp = new CastExpression((AstType)castExpression.Type.AcceptVisitor(this, data), (Expression)castExpression.Expression.AcceptVisitor(this, data));
            return EndNode(castExpression, cexp);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitCheckedExpression(CSharp.CheckedExpression checkedExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitConditionalExpression(CSharp.ConditionalExpression conditionalExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitDefaultValueExpression(CSharp.DefaultValueExpression defaultValueExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitDirectionExpression(CSharp.DirectionExpression directionExpression, object data)
        {
            return EndNode(directionExpression, (Expression)directionExpression.Expression.AcceptVisitor(this, data));
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitIdentifierExpression(CSharp.IdentifierExpression identifierExpression, object data)
        {
            var expr = new IdentifierExpression();
            expr.Identifier = identifierExpression.Identifier;
            ConvertNodes(identifierExpression.TypeArguments, expr.TypeArguments);

            return EndNode(identifierExpression, expr);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitIndexerExpression(CSharp.IndexerExpression indexerExpression, object data)
        {
            var expr = new IndexerExpression((Expression)indexerExpression.Target.AcceptVisitor(this, data));
            ConvertNodes(indexerExpression.Arguments, expr.Arguments);
            return EndNode(indexerExpression, expr);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitInvocationExpression(CSharp.InvocationExpression invocationExpression, object data)
        {
            var expr = new InvocationExpression(
                (Expression)invocationExpression.Target.AcceptVisitor(this, data));
            ConvertNodes(invocationExpression.Arguments, expr.Arguments);

            return EndNode(invocationExpression, expr);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitIsExpression(CSharp.IsExpression isExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitLambdaExpression(CSharp.LambdaExpression lambdaExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitMemberReferenceExpression(CSharp.MemberReferenceExpression memberReferenceExpression, object data)
        {
            var mref = new MemberReferenceExpression();

            mref.Target = (Expression)memberReferenceExpression.Target.AcceptVisitor(this, data);
            mref.MemberName = memberReferenceExpression.MemberName;
            ConvertNodes(memberReferenceExpression.TypeArguments, mref.TypeArguments);

            return EndNode(memberReferenceExpression, mref);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitNamedArgumentExpression(CSharp.NamedArgumentExpression namedArgumentExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitNamedExpression(CSharp.NamedExpression namedExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitNullReferenceExpression(CSharp.NullReferenceExpression nullReferenceExpression, object data)
        {
            return EndNode(nullReferenceExpression, new NullReferenceExpression());
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitObjectCreateExpression(CSharp.ObjectCreateExpression objectCreateExpression, object data)
        {
            //When a variable is initialized gc_ptr<List> list (new(gc)(List)); first List is pointer type, second List is SimpleType
            bool isGcPtr = false;
            var type = (AstType)objectCreateExpression.Type.AcceptVisitor(this, data);
            if (type is PtrType)//Here we make the change
            {
                PtrType ptr = type as PtrType;
                SimpleType stype = new SimpleType();
                stype.Identifier = (ptr.Target as SimpleType).Identifier;

                type = stype;

                isGcPtr = true;
            }


            var expr = new ObjectCreateExpression(type);
            ConvertNodes(objectCreateExpression.Arguments, expr.Arguments);
            expr.isGCPtr = isGcPtr;
            if (!objectCreateExpression.Initializer.IsNull)
                expr.Initializer = (ArrayInitializerExpression)objectCreateExpression.Initializer.AcceptVisitor(this, data);

            return EndNode(objectCreateExpression, expr);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitAnonymousTypeCreateExpression(CSharp.AnonymousTypeCreateExpression anonymousTypeCreateExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitParenthesizedExpression(CSharp.ParenthesizedExpression parenthesizedExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitPointerReferenceExpression(CSharp.PointerReferenceExpression pointerReferenceExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitPrimitiveExpression(CSharp.PrimitiveExpression primitiveExpression, object data)
        {
            Expression expr;

            if (!string.IsNullOrEmpty(primitiveExpression.Value as string) || primitiveExpression.Value is char)
                expr = new PrimitiveExpression(primitiveExpression.Value);//TODO
            //expr = ConvertToConcat(primitiveExpression.Value.ToString());
            else
                expr = new PrimitiveExpression(primitiveExpression.Value);

            return EndNode(primitiveExpression, expr);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitSizeOfExpression(CSharp.SizeOfExpression sizeOfExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitStackAllocExpression(CSharp.StackAllocExpression stackAllocExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitThisReferenceExpression(CSharp.ThisReferenceExpression thisReferenceExpression, object data)
        {
            return EndNode(thisReferenceExpression, new ThisReferenceExpression());
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitTypeOfExpression(CSharp.TypeOfExpression typeOfExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitTypeReferenceExpression(CSharp.TypeReferenceExpression typeReferenceExpression, object data)
        {
            var expr = new TypeReferenceExpression((AstType)typeReferenceExpression.Type.AcceptVisitor(this, data));
            return EndNode(typeReferenceExpression, expr);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitUnaryOperatorExpression(CSharp.UnaryOperatorExpression unaryOperatorExpression, object data)
        {

            Expression expr;

            switch (unaryOperatorExpression.Operator)
            {
                case ICSharpCode.NRefactory.CSharp.UnaryOperatorType.Not:
                case ICSharpCode.NRefactory.CSharp.UnaryOperatorType.BitNot:
                    expr = new UnaryOperatorExpression()
                    {
                        Expression = (Expression)unaryOperatorExpression.Expression.AcceptVisitor(this, data),
                        Operator = UnaryOperatorType.Not
                    };
                    break;
                case ICSharpCode.NRefactory.CSharp.UnaryOperatorType.Minus:
                    expr = new UnaryOperatorExpression()
                    {
                        Expression = (Expression)unaryOperatorExpression.Expression.AcceptVisitor(this, data),
                        Operator = UnaryOperatorType.Minus
                    };
                    break;
                case ICSharpCode.NRefactory.CSharp.UnaryOperatorType.Plus:
                    expr = new UnaryOperatorExpression()
                    {
                        Expression = (Expression)unaryOperatorExpression.Expression.AcceptVisitor(this, data),
                        Operator = UnaryOperatorType.Plus
                    };
                    break;
                case ICSharpCode.NRefactory.CSharp.UnaryOperatorType.Increment:
                    expr = new InvocationExpression();
                    ((InvocationExpression)expr).Target = new IdentifierExpression() { Identifier = "__Increment" };
                    ((InvocationExpression)expr).Arguments.Add((Expression)unaryOperatorExpression.Expression.AcceptVisitor(this, data));
                    break;
                case ICSharpCode.NRefactory.CSharp.UnaryOperatorType.PostIncrement:
                    expr = new InvocationExpression();
                    ((InvocationExpression)expr).Target = new IdentifierExpression() { Identifier = "__PostIncrement" };
                    ((InvocationExpression)expr).Arguments.Add((Expression)unaryOperatorExpression.Expression.AcceptVisitor(this, data));
                    break;
                case ICSharpCode.NRefactory.CSharp.UnaryOperatorType.Decrement:
                    expr = new InvocationExpression();
                    ((InvocationExpression)expr).Target = new IdentifierExpression() { Identifier = "__Decrement" };
                    ((InvocationExpression)expr).Arguments.Add((Expression)unaryOperatorExpression.Expression.AcceptVisitor(this, data));
                    break;
                case ICSharpCode.NRefactory.CSharp.UnaryOperatorType.PostDecrement:
                    expr = new InvocationExpression();
                    ((InvocationExpression)expr).Target = new IdentifierExpression() { Identifier = "__PostDecrement" };
                    ((InvocationExpression)expr).Arguments.Add((Expression)unaryOperatorExpression.Expression.AcceptVisitor(this, data));
                    break;
                //case ICSharpCode.NRefactory.CSharp.UnaryOperatorType.AddressOf:
                //    expr = new UnaryOperatorExpression()
                //    {
                //        Expression = (Expression)unaryOperatorExpression.Expression.AcceptVisitor(this, data),
                //        Operator = UnaryOperatorType.AddressOf
                //    };
                //    break;
                //case ICSharpCode.NRefactory.CSharp.UnaryOperatorType.Dereference:
                //    expr = new InvocationExpression();
                //    ((InvocationExpression)expr).Target = new IdentifierExpression() { Identifier = "__Dereference" };
                //    ((InvocationExpression)expr).Arguments.Add((Expression)unaryOperatorExpression.Expression.AcceptVisitor(this, data));
                //    break;
                default:
                    throw new Exception("Invalid value for UnaryOperatorType");
            }

            return EndNode(unaryOperatorExpression, expr);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitUncheckedExpression(CSharp.UncheckedExpression uncheckedExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitEmptyExpression(CSharp.EmptyExpression emptyExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitQueryExpression(CSharp.QueryExpression queryExpression, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitQueryContinuationClause(CSharp.QueryContinuationClause queryContinuationClause, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitQueryFromClause(CSharp.QueryFromClause queryFromClause, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitQueryLetClause(CSharp.QueryLetClause queryLetClause, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitQueryWhereClause(CSharp.QueryWhereClause queryWhereClause, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitQueryJoinClause(CSharp.QueryJoinClause queryJoinClause, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitQueryOrderClause(CSharp.QueryOrderClause queryOrderClause, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitQueryOrdering(CSharp.QueryOrdering queryOrdering, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitQuerySelectClause(CSharp.QuerySelectClause querySelectClause, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitQueryGroupClause(CSharp.QueryGroupClause queryGroupClause, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitAttribute(CSharp.Attribute attribute, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitAttributeSection(CSharp.AttributeSection attributeSection, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitDelegateDeclaration(CSharp.DelegateDeclaration delegateDeclaration, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitNamespaceDeclaration(CSharp.NamespaceDeclaration namespaceDeclaration, object data)
        {
            var newNamespace = new NamespaceDeclaration();

            ConvertNodes(namespaceDeclaration.Identifiers, newNamespace.Identifiers);
            ConvertNodes(namespaceDeclaration.Members, newNamespace.Members);

            return EndNode(namespaceDeclaration, newNamespace);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitTypeDeclaration(CSharp.TypeDeclaration typeDeclaration, object data)
        {
            var type = new TypeDeclaration();
            CopyAnnotations(typeDeclaration, type);

            CSharp.Attribute stdModAttr;

            if (typeDeclaration.ClassType == CSharp.ClassType.Class && HasAttribute(typeDeclaration.Attributes, "Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute", out stdModAttr))
            {
                type.ClassType = ClassType.Class;
                // remove AttributeSection if only one attribute is present
                var attrSec = (CSharp.AttributeSection)stdModAttr.Parent;
                if (attrSec.Attributes.Count == 1)
                    attrSec.Remove();
                else
                    stdModAttr.Remove();
            }
            else
            {
                switch (typeDeclaration.ClassType)
                {
                    case CSharp.ClassType.Class:
                        type.ClassType = ClassType.Class;
                        break;
                    case CSharp.ClassType.Struct:
                        type.ClassType = ClassType.Struct;
                        break;
                    case CSharp.ClassType.Interface:
                        type.ClassType = ClassType.Interface;
                        break;
                    default:
                        throw new InvalidOperationException("Invalid value for ClassType");
                }
            }

            if ((typeDeclaration.Modifiers & CSharp.Modifiers.Static) == CSharp.Modifiers.Static)
            {
                type.ClassType = ClassType.Class;
                typeDeclaration.Modifiers &= ~CSharp.Modifiers.Static;
            }

            ConvertNodes(typeDeclaration.Attributes, type.Attributes);
            ConvertNodes(typeDeclaration.ModifierTokens, type.ModifierTokens);

            if (typeDeclaration.BaseTypes.Any())
                ConvertNodes(typeDeclaration.BaseTypes, type.BaseTypes);

            type.Name = typeDeclaration.Name;

            types.Push(type);
            ConvertNodes(typeDeclaration.Members, type.Members);
            types.Pop();

            Resolver.ProcessIncludes(type.Name);
            return EndNode(typeDeclaration, type);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitUsingAliasDeclaration(CSharp.UsingAliasDeclaration usingAliasDeclaration, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitUsingDeclaration(CSharp.UsingDeclaration usingDeclaration, object data)
        {
            AstType import = (AstType)usingDeclaration.Import.AcceptVisitor(this, data);
            var include = new IncludeDeclaration(import);

            return EndNode(usingDeclaration, include);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitExternAliasDeclaration(CSharp.ExternAliasDeclaration externAliasDeclaration, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitBlockStatement(CSharp.BlockStatement blockStatement, object data)
        {
            var block = new BlockStatement();
            blocks.Push(block);
            ConvertNodes(blockStatement, block.Statements);
            blocks.Pop();
            return EndNode(blockStatement, block);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitBreakStatement(CSharp.BreakStatement breakStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitCheckedStatement(CSharp.CheckedStatement checkedStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitContinueStatement(CSharp.ContinueStatement continueStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitDoWhileStatement(CSharp.DoWhileStatement doWhileStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitEmptyStatement(CSharp.EmptyStatement emptyStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitExpressionStatement(CSharp.ExpressionStatement expressionStatement, object data)
        {
            var expr = new ExpressionStatement((Expression)expressionStatement.Expression.AcceptVisitor(this, data));
            return EndNode(expressionStatement, expr);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitFixedStatement(CSharp.FixedStatement fixedStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitForeachStatement(CSharp.ForeachStatement foreachStatement, object data)
        {
            ForeachStatement feach = new ForeachStatement();
            feach.EmbeddedStatement = (Statement)foreachStatement.EmbeddedStatement.AcceptVisitor(this,data);
            feach.InExpression = (Expression)foreachStatement.InExpression.AcceptVisitor(this, data); ;
            feach.VariableName = foreachStatement.VariableName;
            feach.VariableType = (AstType)foreachStatement.VariableType.AcceptVisitor(this,data);
            return EndNode(foreachStatement, feach);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitForStatement(CSharp.ForStatement forStatement, object data)
        {
            ForStatement for_stmt = new ForStatement();
            for_stmt.Condition = (Expression)forStatement.Condition.AcceptVisitor(this, data);
            for_stmt.EmbeddedStatement = (Statement)forStatement.EmbeddedStatement.AcceptVisitor(this, data);
            ConvertNodes(forStatement.Initializers, for_stmt.Initializers);
            ConvertNodes(forStatement.Iterators, for_stmt.Iterators);
            return EndNode(forStatement, for_stmt);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitGotoCaseStatement(CSharp.GotoCaseStatement gotoCaseStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitGotoDefaultStatement(CSharp.GotoDefaultStatement gotoDefaultStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitGotoStatement(CSharp.GotoStatement gotoStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitIfElseStatement(CSharp.IfElseStatement ifElseStatement, object data)
        {
            var stmt = new IfElseStatement();

            stmt.Condition = (Expression)ifElseStatement.Condition.AcceptVisitor(this, data);
            stmt.TrueStatement = (Statement)ifElseStatement.TrueStatement.AcceptVisitor(this, data);
            stmt.FalseStatement = (Statement)ifElseStatement.FalseStatement.AcceptVisitor(this, data);

            return EndNode(ifElseStatement, stmt);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitLabelStatement(CSharp.LabelStatement labelStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitLockStatement(CSharp.LockStatement lockStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitReturnStatement(CSharp.ReturnStatement returnStatement, object data)
        {
            var stmt = new ReturnStatement((Expression)returnStatement.Expression.AcceptVisitor(this, data));
            return EndNode(returnStatement, stmt);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitSwitchStatement(CSharp.SwitchStatement switchStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitSwitchSection(CSharp.SwitchSection switchSection, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitCaseLabel(CSharp.CaseLabel caseLabel, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitThrowStatement(CSharp.ThrowStatement throwStatement, object data)
        {
            return EndNode(throwStatement, new ThrowStatement((Expression)throwStatement.Expression.AcceptVisitor(this, data)));
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitTryCatchStatement(CSharp.TryCatchStatement tryCatchStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitCatchClause(CSharp.CatchClause catchClause, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitUncheckedStatement(CSharp.UncheckedStatement uncheckedStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitUnsafeStatement(CSharp.UnsafeStatement unsafeStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitUsingStatement(CSharp.UsingStatement usingStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitVariableDeclarationStatement(CSharp.VariableDeclarationStatement variableDeclarationStatement, object data)
        {
            var vds = new VariableDeclarationStatement();
            vds.Type = (AstType)variableDeclarationStatement.Type.AcceptVisitor(this, data);
            ConvertNodes(variableDeclarationStatement.Variables, vds.Variables);
            return EndNode(variableDeclarationStatement, vds);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitWhileStatement(CSharp.WhileStatement whileStatement, object data)
        {
            WhileStatement whiles = new WhileStatement();
            whiles.Condition = (Expression)whileStatement.Condition.AcceptVisitor(this, data);
            whiles.EmbeddedStatement = (Statement)whileStatement.EmbeddedStatement.AcceptVisitor(this, data);
            return EndNode(whileStatement, whiles); ;
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitYieldBreakStatement(CSharp.YieldBreakStatement yieldBreakStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitYieldReturnStatement(CSharp.YieldReturnStatement yieldReturnStatement, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitAccessor(CSharp.Accessor accessor, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitConstructorDeclaration(CSharp.ConstructorDeclaration constructorDeclaration, object data)
        {
            var result = new ConstructorDeclaration();

            ConvertNodes(constructorDeclaration.Attributes, result.Attributes);
            ConvertNodes(constructorDeclaration.ModifierTokens, result.ModifierTokens);
            ConvertNodes(constructorDeclaration.Parameters, result.Parameters);
            result.Body = (BlockStatement)constructorDeclaration.Body.AcceptVisitor(this, data);
            if (!constructorDeclaration.Initializer.IsNull)
                result.Body.Statements.InsertBefore(result.Body.FirstOrDefault(), (Statement)constructorDeclaration.Initializer.AcceptVisitor(this, data));

            return EndNode(constructorDeclaration, result);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitConstructorInitializer(CSharp.ConstructorInitializer constructorInitializer, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitDestructorDeclaration(CSharp.DestructorDeclaration destructorDeclaration, object data)
        {
            var result = new DestructorDeclaration();

            ConvertNodes(destructorDeclaration.Attributes, result.Attributes);
            ConvertNodes(destructorDeclaration.ModifierTokens, result.ModifierTokens);
            result.Body = (BlockStatement)destructorDeclaration.Body.AcceptVisitor(this, data);

            return EndNode(destructorDeclaration, result);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitEnumMemberDeclaration(CSharp.EnumMemberDeclaration enumMemberDeclaration, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitEventDeclaration(CSharp.EventDeclaration eventDeclaration, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitCustomEventDeclaration(CSharp.CustomEventDeclaration customEventDeclaration, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitFieldDeclaration(CSharp.FieldDeclaration fieldDeclaration, object data)
        {
            var decl = new FieldDeclaration();

            decl.ReturnType = (AstType)fieldDeclaration.ReturnType.AcceptVisitor(this, data);
            decl.Modifiers = ConvertModifiers(fieldDeclaration.Modifiers, fieldDeclaration);
            ConvertNodes(fieldDeclaration.Attributes, decl.Attributes);
            ConvertNodes(fieldDeclaration.Variables, decl.Variables);

            return EndNode(fieldDeclaration, decl);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitIndexerDeclaration(CSharp.IndexerDeclaration indexerDeclaration, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitMethodDeclaration(CSharp.MethodDeclaration methodDeclaration, object data)
        {
            var result = new MethodDeclaration();

            ConvertNodes(methodDeclaration.Attributes.Where(section => section.AttributeTarget != "return"), result.Attributes);
            ConvertNodes(methodDeclaration.ModifierTokens, result.ModifierTokens);
            result.Name = methodDeclaration.Name;

            ConvertNodes(methodDeclaration.Parameters, result.Parameters);
            ConvertNodes(methodDeclaration.TypeParameters, result.TypeParameters);

            result.ReturnType = (AstType)methodDeclaration.ReturnType.AcceptVisitor(this, data);
            result.Body = (BlockStatement)methodDeclaration.Body.AcceptVisitor(this, data);

            return EndNode(methodDeclaration, result);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitOperatorDeclaration(CSharp.OperatorDeclaration operatorDeclaration, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitParameterDeclaration(CSharp.ParameterDeclaration parameterDeclaration, object data)
        {
            var param = new ParameterDeclaration();

            ConvertNodes(parameterDeclaration.Attributes, param.Attributes);
            param.ParameterModifier = (ParameterModifier)parameterDeclaration.ParameterModifier;
            param.Type = (AstType)parameterDeclaration.Type.AcceptVisitor(this, data);
            param.NameToken = (Identifier)parameterDeclaration.NameToken.AcceptVisitor(this, data);
            param.DefaultExpression = (Expression)parameterDeclaration.DefaultExpression.AcceptVisitor(this, data);

            return EndNode(parameterDeclaration, param);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitPropertyDeclaration(CSharp.PropertyDeclaration propertyDeclaration, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitVariableInitializer(CSharp.VariableInitializer variableInitializer, object data)
        {
            var vi = new VariableInitializer();

            vi.Initializer = (Expression)variableInitializer.Initializer.AcceptVisitor(this, data);
            vi.Name = variableInitializer.Name;
            vi.NameToken = (Identifier)variableInitializer.NameToken.AcceptVisitor(this, data);

            return EndNode(variableInitializer, vi);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitFixedFieldDeclaration(CSharp.FixedFieldDeclaration fixedFieldDeclaration, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitFixedVariableInitializer(CSharp.FixedVariableInitializer fixedVariableInitializer, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitCompilationUnit(CSharp.CompilationUnit compilationUnit, object data)
        {
            var unit = new CompilationUnit();

            foreach (var node in compilationUnit.Children)
                unit.AddChild(node.AcceptVisitor(this, null), CompilationUnit.MemberRole);

            return EndNode(compilationUnit, unit);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitSimpleType(CSharp.SimpleType simpleType, object data)
        {
            string id = simpleType.Identifier;
            bool isPtr = true;
            //If the simpleType is of an include, we need to transform the name, and it is not marked as pointer
            if (simpleType.Parent is CSharp.UsingDeclaration ||
                ((simpleType.Parent is CSharp.ComposedType) && (simpleType.Parent.Parent is CSharp.UsingDeclaration)))
            {
                id = Resolver.GetCppName(simpleType.Identifier);
                Resolver.AddNewLibrary(simpleType.Identifier);
                isPtr = false;
            }
            var type = new SimpleType(id);
            ConvertNodes(simpleType.TypeArguments, type.TypeArguments);

            //Add the visited type to the resolver in order to include it after
            //Also this call adds the type to the include list for detecting forward declarations
            //If its parent is null, is better to ignore :)
            if (simpleType.Parent != null)
                Resolver.AddVistedType(type, simpleType.Identifier);

            if (simpleType.Annotations.Count() > 0)
                Resolver.AddSymbol(id, simpleType.Annotations.ElementAt(0) as TypeReference);

            //If the type is in the Visual Tree, the parent is null. 
            //If its parent is a TypeReferenceExpression it is like Console::ReadLine                
            if (simpleType.Parent == null || !isPtr || simpleType.Parent is CSharp.TypeReferenceExpression)
                return EndNode(simpleType, type);

            var ptrType = new PtrType(type);
            ptrType.Target = type;

            return EndNode(simpleType, ptrType);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitMemberType(CSharp.MemberType memberType, object data)
        {
            AstType target = null;

            //if (memberType.Target is CSharp.SimpleType && ((CSharp.SimpleType)(memberType.Target)).Identifier.Equals("global", StringComparison.Ordinal))
            //    target = new PrimitiveType("Global");
            //else
            //    target = (AstType)memberType.Target.AcceptVisitor(this, data);

            target = (AstType)memberType.Target.AcceptVisitor(this, data);

            var type = new QualifiedType(target, new Identifier(memberType.MemberName, TextLocation.Empty));
            ConvertNodes(memberType.TypeArguments, type.TypeArguments);

            Resolver.AddVistedType(type, memberType.MemberName);
            return EndNode(memberType, type);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitComposedType(CSharp.ComposedType composedType, object data)
        {
            //If there is ArraySpecifier, get it and return the simpleType or primitiveType
            if (composedType.ArraySpecifiers.Count > 0)
                arraySpecifiers.AddRange(composedType.ArraySpecifiers);

            if (composedType.HasNullableSpecifier)
            {
                ComposedType ctype = new ComposedType();
                ctype.BaseType = (AstType)composedType.BaseType.AcceptVisitor(this, data);
                ctype.HasNullableSpecifier = composedType.HasNullableSpecifier;
                return EndNode(composedType, ctype);
            }
            return composedType.BaseType.AcceptVisitor(this, data);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitArraySpecifier(CSharp.ArraySpecifier arraySpecifier, object data)
        {
            return EndNode(arraySpecifier, new ArraySpecifier(arraySpecifier.Dimensions));
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitPrimitiveType(CSharp.PrimitiveType primitiveType, object data)
        {
            string typeName;

            switch (primitiveType.Keyword)
            {
                case "object":
                    typeName = "Object";
                    break;
                case "sbyte":
                    typeName = "short";
                    break;
                case "byte":
                    typeName = "short";
                    break;
                case "decimal":
                    typeName = "float";
                    break;
                case "double":
                    typeName = "float";
                    break;
                case "string":
                    typeName = "String";
                    break;
                default:
                    typeName = primitiveType.Keyword;
                    break;
            }
            return EndNode(primitiveType, new PrimitiveType(typeName));
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitComment(CSharp.Comment comment, object data)
        {
            CommentType cmntType;
            switch (comment.CommentType)
            {
                case CSharp.CommentType.Documentation:
                    cmntType = CommentType.Documentation;
                    break;
                case CSharp.CommentType.InactiveCode:
                    cmntType = CommentType.InactiveCode;
                    break;
                case CSharp.CommentType.MultiLine:
                    cmntType = CommentType.MultiLine;
                    break;
                case CSharp.CommentType.SingleLine:
                    cmntType = CommentType.SingleLine;
                    break;
                default:
                    throw new Exception("Invalid comment type");
            }
            Comment cmnt = new Comment(comment.Content, cmntType);
            return cmnt;
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitPreProcessorDirective(CSharp.PreProcessorDirective preProcessorDirective, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitTypeParameterDeclaration(CSharp.TypeParameterDeclaration typeParameterDeclaration, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitConstraint(CSharp.Constraint constraint, object data)
        {
            throw new NotImplementedException();
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitCSharpTokenNode(CSharp.CSharpTokenNode cSharpTokenNode, object data)
        {
            var mod = cSharpTokenNode as CSharp.CSharpModifierToken;
            if (mod != null)
            {
                var convertedModifiers = ConvertModifiers(mod.Modifier, mod.Parent);
                CppModifierToken token = null;
                if (convertedModifiers != Modifiers.None)
                {
                    token = new CppModifierToken(TextLocation.Empty, convertedModifiers);
                    return EndNode(cSharpTokenNode, token);
                }
                return EndNode(cSharpTokenNode, token);
            }
            else
            {
                throw new NotSupportedException("Should never visit individual tokens");
            }
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitIdentifier(CSharp.Identifier identifier, object data)
        {
            if (arraySpecifiers.Count > 0)
            {
                var compIdent = new ComposedIdentifier(identifier.Name, TextLocation.Empty);
                ConvertNodes(arraySpecifiers, compIdent.ArraySpecifiers);
                arraySpecifiers.Clear();

                compIdent.BaseIdentifier = (Identifier)identifier.AcceptVisitor(this, data);

                return compIdent;
            }
            var ident = new Identifier(identifier.Name, identifier.StartLocation);

            return EndNode(identifier, ident);
        }

        AstNode CSharp.IAstVisitor<object, AstNode>.VisitPatternPlaceholder(CSharp.AstNode placeholder, PatternMatching.Pattern pattern, object data)
        {
            throw new NotImplementedException();
        }

        void ConvertNodes<T>(IEnumerable<CSharp.AstNode> nodes, Cpp.AstNodeCollection<T> result) where T : Cpp.AstNode
        {
            foreach (var node in nodes)
            {
                T n = (T)node.AcceptVisitor(this, null);
                if (n != null)
                    result.Add(n);
            }
        }

        T EndNode<T>(CSharp.AstNode node, T result) where T : Cpp.AstNode
        {
            if (result != null)
            {
                CopyAnnotations(node, result);
            }

            return result;
        }

        void CopyAnnotations<T>(CSharp.AstNode node, T result) where T : Cpp.AstNode
        {
            foreach (var ann in node.Annotations)
                result.AddAnnotation(ann);
        }

        Modifiers ConvertModifiers(CSharp.Modifiers modifier, CSharp.AstNode container)
        {
            if ((modifier & CSharp.Modifiers.Any) == CSharp.Modifiers.Any)
                return Modifiers.Any;

            var mod = Modifiers.None;

            if ((modifier & CSharp.Modifiers.Static) == CSharp.Modifiers.Static)
                mod |= Modifiers.Static;

            if ((modifier & CSharp.Modifiers.Public) == CSharp.Modifiers.Public)
                mod |= Modifiers.Public;
            if ((modifier & CSharp.Modifiers.Protected) == CSharp.Modifiers.Protected)
                mod |= Modifiers.Protected;
            if ((modifier & CSharp.Modifiers.Internal) == CSharp.Modifiers.Internal)
                mod |= Modifiers.Public;
            if ((modifier & CSharp.Modifiers.Private) == CSharp.Modifiers.Private)
                mod |= Modifiers.Private;

            if ((modifier & CSharp.Modifiers.Abstract) == CSharp.Modifiers.Abstract)
            {
                if (container is CSharp.TypeDeclaration)
                    mod |= Modifiers.Abstract;//MUST INHERIT
                else
                    mod |= Modifiers.Virtual;//MUST OVERRIDE
            }

            if ((modifier & CSharp.Modifiers.Override) == CSharp.Modifiers.Override)
                mod |= Modifiers.Virtual;//CPP OVERRIDE KEYWORD IS NOT ALLOWED BY ALL THE COMPILERS
            if ((modifier & CSharp.Modifiers.Virtual) == CSharp.Modifiers.Virtual)
                mod |= Modifiers.Virtual;

            return mod;
        }

        bool HasAttribute(CSharp.AstNodeCollection<CSharp.AttributeSection> attributes, string name, out CSharp.Attribute foundAttribute)
        {
            foreach (var attr in attributes.SelectMany(a => a.Attributes))
            {
                if (provider.GetTypeNameForAttribute(attr) == name)
                {
                    foundAttribute = attr;
                    return true;
                }
            }
            foundAttribute = null;
            return false;
        }
    }
}
