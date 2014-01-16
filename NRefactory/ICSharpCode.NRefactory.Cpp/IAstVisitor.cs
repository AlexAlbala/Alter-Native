// Copyright (c) AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using ICSharpCode.NRefactory.Cpp.Ast;
using Attribute = ICSharpCode.NRefactory.Cpp.Ast.Attribute;

namespace ICSharpCode.NRefactory.Cpp
{
    /// <summary>
    /// AST visitor.
    /// </summary>
    public interface IAstVisitor<in T, out S>
    {
        S VisitAnonymousMethodExpression(AnonymousMethodExpression anonymousMethodExpression, T data);
        S VisitUndocumentedExpression(UndocumentedExpression undocumentedExpression, T data);
        S VisitArrayCreateExpression(ArrayCreateExpression arrayCreateExpression, T data);
        S VisitArrayInitializerExpression(ArrayInitializerExpression arrayInitializerExpression, T data);
        //S VisitAsExpression(AsExpression asExpression, T data);
        S VisitAssignmentExpression(AssignmentExpression assignmentExpression, T data);
        S VisitBaseReferenceExpression(BaseReferenceExpression baseReferenceExpression, T data);
        S VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression, T data);
        S VisitCastExpression(CastExpression castExpression, T data);
        S VisitDynamicCastExpression(DynamicCastExpression castExpression, T data);
        S VisitCheckedExpression(CheckedExpression checkedExpression, T data);
        S VisitConditionalExpression(ConditionalExpression conditionalExpression, T data);
        S VisitDefaultValueExpression(DefaultValueExpression defaultValueExpression, T data);
        S VisitDirectionExpression(DirectionExpression directionExpression, T data);
        S VisitIdentifierExpression(IdentifierExpression identifierExpression, T data);
        S VisitIndexerExpression(IndexerExpression indexerExpression, T data);
        S VisitInvocationExpression(InvocationExpression invocationExpression, T data);
        S VisitDelegateInvokeExpression(DelegateInvokeExpression delegateInvokeExpression, T data);//*****************
        S VisitEventFireExpression(EventFireExpression eventFireExpression, T data);//*****************
        S VisitInterfaceMemberSpecifier(InterfaceMemberSpecifier interfaceMemberSpecifier, T data);
        //S VisitIsExpression(IsExpression isExpression, T data);
        S VisitLambdaExpression(LambdaExpression lambdaExpression, T data);
        S VisitMemberAccessExpression(MemberAccessExpression memberAccessExpression, T data);
        S VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression, T data);
        S VisitNamedArgumentExpression(NamedArgumentExpression namedArgumentExpression, T data);
        S VisitNamedExpression(NamedExpression namedExpression, T data);
        S VisitNullReferenceExpression(NullReferenceExpression nullReferenceExpression, T data);
        S VisitObjectCreateExpression(ObjectCreateExpression objectCreateExpression, T data);
        S VisitDelegateCreateExpression(DelegateCreateExpression delegateCreateExpression, T data);//******************
        S VisitAnonymousTypeCreateExpression(AnonymousTypeCreateExpression anonymousTypeCreateExpression, T data);
        S VisitParenthesizedExpression(ParenthesizedExpression parenthesizedExpression, T data);
        S VisitPointerExpression(PointerExpression pointerExpression, T data);//*********************
        S VisitAddressOfExpression(AddressOfExpression addressOfExpression, T data);//*****************
        S VisitPointerReferenceExpression(PointerReferenceExpression pointerReferenceExpression, T data);
        S VisitPrimitiveExpression(PrimitiveExpression primitiveExpression, T data);
        S VisitSizeOfExpression(SizeOfExpression sizeOfExpression, T data);
        S VisitStackAllocExpression(StackAllocExpression stackAllocExpression, T data);
        S VisitThisReferenceExpression(ThisReferenceExpression thisReferenceExpression, T data);
        S VisitTypeOfExpression(TypeOfExpression typeOfExpression, T data);
        S VisitTypeReferenceExpression(TypeReferenceExpression typeReferenceExpression, T data);
        S VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression, T data);
        S VisitUncheckedExpression(UncheckedExpression uncheckedExpression, T data);
        S VisitEmptyExpression(EmptyExpression emptyExpression, T data);
        S VisitBoxExpression(BoxExpression boxExpression, T data);//******************
        S VisitUnBoxExpression(UnBoxExpression unBoxExpression, T data);//******************
        S VisitGlobalNamespaceReferenceReferenceExpression(GlobalNamespaceReferenceExpression globalNamespaceReferenceExpression, T data);//************

        S VisitQueryExpression(QueryExpression queryExpression, T data);
        S VisitQueryContinuationClause(QueryContinuationClause queryContinuationClause, T data);
        S VisitQueryFromClause(QueryFromClause queryFromClause, T data);
        S VisitQueryLetClause(QueryLetClause queryLetClause, T data);
        S VisitQueryWhereClause(QueryWhereClause queryWhereClause, T data);
        S VisitQueryJoinClause(QueryJoinClause queryJoinClause, T data);
        S VisitQueryOrderClause(QueryOrderClause queryOrderClause, T data);
        S VisitQueryOrdering(QueryOrdering queryOrdering, T data);
        S VisitQuerySelectClause(QuerySelectClause querySelectClause, T data);
        S VisitQueryGroupClause(QueryGroupClause queryGroupClause, T data);

        S VisitAttribute(Attribute attribute, T data);
        S VisitAttributeSection(AttributeSection attributeSection, T data);
        S VisitDelegateDeclaration(DelegateDeclaration delegateDeclaration, T data);
        S VisitHeaderDelegateDeclaration(HeaderDelegateDeclaration headerDelegateDeclaration, T data);//************
        S VisitHeaderEventDeclaration(HeaderEventDeclaration headerEventDeclaration, T data);//************
        S VisitNamespaceDeclaration(NamespaceDeclaration namespaceDeclaration, T data);
        S VisitTypeDeclaration(TypeDeclaration typeDeclaration, T data);
        S VisitNestedTypeDeclaration(NestedTypeDeclaration nestedTypeDeclaration, T data);//****************
        S VisitExplicitInterfaceTypeDeclaration(ExplicitInterfaceTypeDeclaration explicitInterfaceTypeDeclaration, T data); //******************
        S VisitInterfaceTypeDeclaration(InterfaceTypeDeclaration interfaceTypeDeclaration, T data); //******************
        S VisitGenericTemplateTypeDeclaration(GenericTemplateTypeDeclaration genericTemplateTypeDeclaration, T data); //******************
        S VisitSpecializedGenericTemplateDeclaration(SpecializedGenericTemplateDeclaration specializedGenericTemplateDeclaration, T data);//*************
        S VisitSpecializedBasicTemplateDeclaration(SpecializedBasicTemplateDeclaration specializedBasicTemplateDeclaration, T data);//*************
        S VisitTemplateTypeDeclaration(TemplateTypeDeclaration templateTypeDeclaration, T data);//*********************
        S VisitBaseTemplateTypeDeclaration(BaseTemplateTypeDeclaration baseTemplateTypeDeclaration, T data);//*********************
        S VisitGenericEntryPointDeclaration(GenericEntryPointDeclaration genericEntryPointDeclaration, T data);//*********************
        //S VisitUsingAliasDeclaration(UsingAliasDeclaration usingAliasDeclaration, T data);
        S VisitIncludeDeclaration(IncludeDeclaration usingDeclaration, T data);
        S VisitExternAliasDeclaration(ExternAliasDeclaration externAliasDeclaration, T data);

        S VisitBlockStatement(BlockStatement blockStatement, T data);
        S VisitBreakStatement(BreakStatement breakStatement, T data);
        S VisitCheckedStatement(CheckedStatement checkedStatement, T data);
        S VisitContinueStatement(ContinueStatement continueStatement, T data);
        S VisitDoWhileStatement(DoWhileStatement doWhileStatement, T data);
        S VisitEmptyStatement(EmptyStatement emptyStatement, T data);
        S VisitExpressionStatement(ExpressionStatement expressionStatement, T data);
        S VisitFixedStatement(FixedStatement fixedStatement, T data);
        S VisitForeachStatement(ForeachStatement foreachStatement, T data);
        S VisitForStatement(ForStatement forStatement, T data);
        S VisitGotoCaseStatement(GotoCaseStatement gotoCaseStatement, T data);
        S VisitGotoDefaultStatement(GotoDefaultStatement gotoDefaultStatement, T data);
        S VisitGotoStatement(GotoStatement gotoStatement, T data);
        S VisitIfElseStatement(IfElseStatement ifElseStatement, T data);
        S VisitLabelStatement(LabelStatement labelStatement, T data);
        S VisitLockStatement(LockStatement lockStatement, T data);
        S VisitReturnStatement(ReturnStatement returnStatement, T data);
        S VisitSwitchStatement(SwitchStatement switchStatement, T data);
        S VisitSwitchSection(SwitchSection switchSection, T data);
        S VisitCaseLabel(CaseLabel caseLabel, T data);
        S VisitThrowStatement(ThrowStatement throwStatement, T data);
        S VisitTryCatchStatement(TryCatchStatement tryCatchStatement, T data);
        S VisitEndScopeStatement(ExitScopeStatement endScopeStatement, T data);//***********************
        S VisitCatchClause(CatchClause catchClause, T data);
        S VisitUncheckedStatement(UncheckedStatement uncheckedStatement, T data);
        S VisitUnsafeStatement(UnsafeStatement unsafeStatement, T data);
        S VisitUsingNamespaceStatement(UsingNamespaceStatement usingStatement, T data);
        S VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement, T data);
        S VisitWhileStatement(WhileStatement whileStatement, T data);
        S VisitYieldBreakStatement(YieldBreakStatement yieldBreakStatement, T data);
        S VisitYieldReturnStatement(YieldReturnStatement yieldReturnStatement, T data);

        S VisitAccessor(Accessor accessor, T data);
        S VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration, T data);
        S VisitHeaderConstructorDeclaration(HeaderConstructorDeclaration headerConstructorDeclaration, T data);//*************
        S VisitConstructorInitializer(ConstructorInitializer constructorInitializer, T data);
        S VisitDestructorDeclaration(DestructorDeclaration destructorDeclaration, T data);
        S VisitHeaderDestructorDeclaration(HeaderDestructorDeclaration headerDestructorDeclaration, T data);//***************
        S VisitEnumMemberDeclaration(EnumMemberDeclaration enumMemberDeclaration, T data);
        S VisitEventDeclaration(EventDeclaration eventDeclaration, T data);
        S VisitCustomEventDeclaration(CustomEventDeclaration customEventDeclaration, T data);
        S VisitFieldDeclaration(FieldDeclaration fieldDeclaration, T data);
        S VisitHeaderFieldDeclaration(HeaderFieldDeclaration headerFieldDeclaration, T data);//**************
        S VisitIndexerDeclaration(IndexerDeclaration indexerDeclaration, T data);
        S VisitMethodDeclaration(MethodDeclaration methodDeclaration, T data);
        S VisitExternMethodDeclaration(ExternMethodDeclaration externMethodDeclaration, T data);//**************
        S VisitHeaderMethodDeclaration(HeaderMethodDeclaration headerMethodDeclaration, T data);//***************
        S VisitHeaderAbstractMethodDeclaration(HeaderAbstractMethodDeclaration headerAbstractMethodDeclaration, T data);//***************
        S VisitOperatorDeclaration(OperatorDeclaration operatorDeclaration, T data);
        S VisitConversionConstructorDeclaration(ConversionConstructorDeclaration conversionConstructorDeclaration, T data);//************
        S VisitHeaderConversionConstructorDeclaration(HeaderConversionConstructorDeclaration headerConversionConstructorDeclaration, T data);//**********
        S VisitParameterDeclaration(ParameterDeclaration parameterDeclaration, T data);
        S VisitVariadicParameterDeclaration(VariadicParameterDeclaration variadicParameterDeclaration, T data);//**************
        S VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration, T data);
        S VisitVariableInitializer(VariableInitializer variableInitializer, T data);
        S VisitFixedFieldDeclaration(FixedFieldDeclaration fixedFieldDeclaration, T data);
        S VisitFixedVariableInitializer(FixedVariableInitializer fixedVariableInitializer, T data);

        S VisitCompilationUnit(CompilationUnit compilationUnit, T data);
        S VisitSimpleType(SimpleType simpleType, T data);
        S VisitPtrType(PtrType ptrType, T data);//**************
        S VisitReferenceType(ReferenceType referenceType, T data);//**************
        //S VisitMemberType(MemberType memberType, T data);
        S VisitExpressionType(ExpressionType expressionType, T data);//************
        S VisitTypeNameType(TypeNameType typeNameType, T data);
        S VisitQualifiedType(QualifiedType qualifiedType, T data);
        S VisitComposedType(ComposedType composedType, T data);
        S VisitArraySpecifier(ArraySpecifier arraySpecifier, T data);
        S VisitPrimitiveType(PrimitiveType primitiveType, T data);

        S VisitComment(Comment comment, T data);
        S VisitPreProcessorDirective(PreProcessorDirective preProcessorDirective, T data);

        S VisitTypeParameterDeclaration(TypeParameterDeclaration typeParameterDeclaration, T data);
        //S VisitConstraint(Constraint constraint, T data);
        S VisitCppTokenNode(CppTokenNode cSharpTokenNode, T data);
        S VisitIdentifier(Identifier identifier, T data);
        S VisitComposedIdentifier(ComposedIdentifier identifier, T data);//****************

        S VisitPatternPlaceholder(AstNode placeholder, PatternMatching.Pattern pattern, T data);

        S VisitDocumentationReference(DocumentationReference documentationReference, T data);
        S VisitNewLine(NewLineNode newLineNode, T data);
        S VisitWhitespace(WhitespaceNode whiteSpaceNode, T data);
        S VisitText(TextNode textNode, T data);
    }
}
