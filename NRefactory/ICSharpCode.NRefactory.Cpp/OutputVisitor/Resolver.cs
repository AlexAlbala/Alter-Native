using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using ICSharpCode.NRefactory.Cpp.Ast;

namespace ICSharpCode.NRefactory.Cpp
{
    public class Resolver
    {
        private static List<string> ltmp = new List<string>();

        static Resolver()
        {
            Dictionary<string, string> libraryMap = new Dictionary<string, string>();
            libraryMap.Add("System", "\"System/System.h\"");
            libraryMap.Add("Console", "\"System/Console.h\"");
            libraryMap.Add("Random", "\"System/Random.h\"");
            libraryMap.Add("GC", "\"System/GC.h\"");
            libraryMap.Add("List_T", "\"System/Collections/Generic/List.h\"");
            libraryMap.Add("IEnumerable_T", "\"System/Collections/Generic/IEnumerable.h\"");
            libraryMap.Add("IEnumerator_T", "\"System/Collections/Generic/IEnumeratorCXX.h\"");
            libraryMap.Add("IEnumerable", "\"System/Collections/IEnumerable.h\"");
            libraryMap.Add("IEnumerator", "\"System/Collections/IEnumeratorCXX.h\"");
            libraryMap.Add("IDisposable", "\"System/IDisposable.h\"");
            libraryMap.Add("StreamReader", "\"System/IO/StreamReaderCXX.h\"");
            libraryMap.Add("StreamWriter", "\"System/IO/StreamWriterCXX.h\"");
            libraryMap.Add("FileStream", "\"System/IO/FileStream.h\"");
            libraryMap.Add("Convert", "\"System/Convert.h\"");
            libraryMap.Add("File", "\"System/IO/File.h\"");
            libraryMap.Add("UTF8Encoding", "\"System/Text/UTF8Encoding.h\"");
            Cache.InitLibrary(libraryMap);
        }

        /// <summary>
        /// Adds a new include definition
        /// </summary>        
        /// <param name="included">The type included</param>
        public static void AddInclude(string included)
        {
            string owner = "N/P";
            Cache.AddInclude(owner, included);
        }

        /// <summary>
        /// Makes the preprocessing of the includes list
        /// </summary>
        /// <param name="typeDeclarationName"></param>
        public static void ProcessIncludes(string typeDeclarationName)
        {
            Dictionary<string, List<string>> includes = Cache.GetIncludes();

            for (int i = 0; i < includes.Count; i++)
            {
                KeyValuePair<string, List<string>> kvp = includes.ElementAt(i);
                if (kvp.Key == "N/P")
                {
                    if (!includes.ContainsKey(typeDeclarationName))
                        includes.Add(typeDeclarationName, kvp.Value);
                    includes.Remove("N/P");
                    i--;
                }
            }

            //Remove itself for each type
            foreach (KeyValuePair<string, List<string>> kvp in includes)
            {
                if (kvp.Value.Contains(kvp.Key))
                    kvp.Value.Remove(kvp.Key);
            }
            Cache.SaveIncludes(includes);
        }

        /// <summary>
        /// Returns if a forward declaration is needed between two types
        /// </summary>
        /// <param name="fw_dcl_type1">The type to test</param>
        /// <param name="fw_dcl_type2">If the method is true, the second type is placed here</param>
        /// <returns></returns>
        public static bool NeedsForwardDeclaration(string fw_dcl_type1, out string fw_dcl_type2)
        {
            ltmp.Clear();

            Dictionary<string, List<string>> includes = Cache.GetIncludes();
            //The type is included ?
            //If not... we have a problem !
            if (includes.ContainsKey(fw_dcl_type1))
            {
                //if one of the types is declared in includes...
                foreach (string type2_s in includes[fw_dcl_type1])
                {
                    bool tmp = Reaches(type2_s, fw_dcl_type1, includes);
                    if (tmp)
                    {
                        fw_dcl_type2 = type2_s;
                        return true;
                    }
                }
            }
            else
                throw new InvalidOperationException("Must be included. It is impossible to enter this funcion before the type is included!");

            fw_dcl_type2 = String.Empty;
            return false;
        }

        /// <summary>
        /// Returns if an identifier is a pointer expression. This method checks if the identifier (declared in a method parameter, method variable, or class field) is a pointer.
        /// The method will search the identifier depending of the filled parameters or the null parameters
        /// CurrentType + currentFieldVarialbe
        /// CurrentMethod + CurrentParameter
        /// CurrentMethod + CurrentField_Variable
        /// </summary>
        /// <param name="currentType">The current type name</param>
        /// <param name="currentField_Variable">The variable or field that is being checked</param>
        /// <param name="currentMethod">The current method</param>
        /// <param name="currentParameter">The parameter that is being checked</param>currentType
        /// <returns></returns>
        public static bool IsPointer(string currentField_Variable, string currentType, string currentMethod, string currentParameter)
        {
            if (currentField_Variable != null && currentType != null)
            {
                Dictionary<string, List<FieldDeclaration>> fields = Cache.GetFields();
                if (fields.ContainsKey(currentType))
                {
                    foreach (FieldDeclaration fd in fields[currentType])
                    {
                        if (fd.ReturnType is Cpp.Ast.PtrType)
                        {
                            var col = fd.Variables;
                            if (col.FirstOrNullObject(x => x.Name == currentField_Variable) != null)
                                return true;
                        }
                    }
                }
            }

            if (currentMethod != null && currentParameter != null)
            {
                Dictionary<string, List<ParameterDeclaration>> parameters = Cache.GetParameters();
                if (parameters.ContainsKey(currentMethod))
                {
                    foreach (ParameterDeclaration pd in parameters[currentMethod])
                    {
                        if (pd.Type is Cpp.Ast.PtrType)
                        {
                            if (pd.Name == currentParameter)
                                return true;
                        }
                    }
                }
            }

            if (currentMethod != null && currentField_Variable != null)
            {
                Dictionary<string, List<VariableDeclarationStatement>> variablesMethod = Cache.GetVariablesMethod();
                if (variablesMethod.ContainsKey(currentMethod))
                {
                    foreach (VariableDeclarationStatement fd in variablesMethod[currentMethod])
                    {
                        if (fd.Type is Cpp.Ast.PtrType)
                        {
                            var col = fd.Variables;
                            if (col.FirstOrNullObject(x => x.Name == currentField_Variable) != null)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public static AstType GetType(string currentField_Variable, string currentType, string currentMethod, string currentParameter)
        {
            if (currentField_Variable != null && currentType != null)
            {
                Dictionary<string, List<FieldDeclaration>> fields = Cache.GetFields();
                if (fields.ContainsKey(currentType))
                {
                    foreach (FieldDeclaration fd in fields[currentType])
                    {
                        foreach (VariableInitializer vi in fd.Variables)
                        {
                            if (vi.Name == currentField_Variable)
                                return fd.ReturnType;
                        }
                    }
                }
            }

            if (currentMethod != null && currentParameter != null)
            {
                Dictionary<string, List<ParameterDeclaration>> parameters = Cache.GetParameters();
                if (parameters.ContainsKey(currentMethod))
                {
                    foreach (ParameterDeclaration pd in parameters[currentMethod])
                    {
                        if (pd.Name == currentParameter)
                            return pd.Type;
                    }
                }
            }

            if (currentMethod != null && currentField_Variable != null)
            {
                Dictionary<string, List<VariableDeclarationStatement>> variablesMethod = Cache.GetVariablesMethod();
                if (variablesMethod.ContainsKey(currentMethod))
                {
                    foreach (VariableDeclarationStatement fd in variablesMethod[currentMethod])
                    {
                        foreach (VariableInitializer vi in fd.Variables)
                        {
                            if (vi.Name == currentField_Variable)
                                return fd.Type;
                        }

                    }
                }
            }
            return AstType.Null;
        }

        private static bool Reaches(string type1, string type2, Dictionary<string, List<string>> includes)
        {
            if (includes.ContainsKey(type1))
            {
                foreach (string _t2 in includes[type1])
                {
                    //Save the visited types to avoid loops
                    if (ltmp.Contains(_t2))
                        continue;//if it is a visited type, skip it
                    else
                        ltmp.Add(_t2);

                    if (_t2 == type2)
                        return true;

                    bool tmp = Reaches(_t2, type2, includes);
                    if (tmp)
                        return true;
                }
                return false;
            }
            return false;
        }

        public static string GetCppName(string CSharpName)
        {
            Dictionary<string, string> libraryMap = Cache.GetLibraryMap();
            if (libraryMap.ContainsKey(CSharpName))
                return libraryMap[CSharpName];
            else
                return "\"" + CSharpName.Replace('.', '/') + ".h\"";
        }

        public static void AddVistedType(Ast.AstType type, string name)
        {
            Cache.AddVisitedType(type, name);
            AddInclude(name);
        }

        public static string[] GetTypeIncludes()
        {
            Dictionary<Ast.AstType, string> visitedTypes = Cache.GetVisitedTypes();
            List<string> tmp = new List<string>();
            foreach (KeyValuePair<Ast.AstType, string> kvp in visitedTypes)
            {
                if (!kvp.Key.IsBasicType)
                {
                    if (!Cache.GetTemplateTypes().Contains(kvp.Value) && !tmp.Contains(GetCppName(kvp.Value)))
                        tmp.Add(GetCppName(kvp.Value));
                }
            }
            return tmp.ToArray();
        }

        public static void Restart()
        {
            Cache.ClearResolver();
        }

        public static void AddSymbol(string type, TypeReference reference)
        {
            Cache.AddSymbol(type, reference);

            string namesp = reference.Namespace;
            AddNamespace(namesp);
        }

        private static void AddNamespace(string nameSpace)
        {
            nameSpace = nameSpace.Replace(".", "::");
            Cache.AddNamespace(nameSpace);
        }

        public static string[] GetNeededNamespaces()
        {
            return Cache.GetNamespaces().ToArray();
        }

        public static string GetTypeName(Ast.AstType type)
        {
            if (type is SimpleType)
                return (type as SimpleType).Identifier;
            else if (type is PtrType)
            {
                PtrType p = type as PtrType;
                return GetTypeName(p.Target);
            }
            else if (type == AstType.Null)
                return String.Empty;
            else if (type is PrimitiveType)
                return (type as PrimitiveType).Keyword;
            else
                throw new NotImplementedException(type.ToString());
        }

        public static string GetTypeName(CSharp.AstType type)
        {
            if (type is CSharp.SimpleType)
                return (type as CSharp.SimpleType).Identifier;
            else if (type == CSharp.AstType.Null)
                return String.Empty;
            else if (type is CSharp.PrimitiveType)
                return (type as CSharp.PrimitiveType).Keyword;
            else if (type is CSharp.ComposedType)
            {
                CSharp.ComposedType ct = type as CSharp.ComposedType;
                return GetTypeName(ct.BaseType);
            }
            else
                throw new NotImplementedException(type.ToString());
        }

        public static void RemoveHeaderNode(AstNode node)
        {
            if (node is ConstructorDeclaration)
            {
                for (int i = 0; i < Cache.GetHeaderNodes().Count; i++)
                {
                    if (Cache.GetHeaderNodes().ElementAt(i) is HeaderConstructorDeclaration)
                    {
                        var hc = Cache.GetHeaderNodes().ElementAt(i) as HeaderConstructorDeclaration;
                        if (hc.Name == (node as ConstructorDeclaration).Name)
                            Cache.GetHeaderNodes().RemoveAt(i);

                    }
                }
            }
            else if (node is DestructorDeclaration)
            {
                for (int i = 0; i < Cache.GetHeaderNodes().Count; i++)
                {
                    if (Cache.GetHeaderNodes().ElementAt(i) is HeaderDestructorDeclaration)
                    {
                        var hc = Cache.GetHeaderNodes().ElementAt(i) as HeaderDestructorDeclaration;
                        if (hc.Name == (node as DestructorDeclaration).Name)
                            Cache.GetHeaderNodes().RemoveAt(i);
                    }
                }
            }
            else if (node is FieldDeclaration)
            {
                for (int i = 0; i < Cache.GetHeaderNodes().Count; i++)
                {
                    if (Cache.GetHeaderNodes().ElementAt(i) is HeaderFieldDeclaration)
                    {
                        var hc = Cache.GetHeaderNodes().ElementAt(i) as HeaderFieldDeclaration;
                        foreach (VariableInitializer v in (node as FieldDeclaration).Variables)
                        {
                            var n = hc.Variables.FirstOrNullObject(x => x.Name == v.Name);
                            if (n != null)
                                hc.Variables.Remove(n);

                            if (!hc.Variables.Any())
                                Cache.GetHeaderNodes().RemoveAt(i);

                        }
                    }
                }
            }
            else if (node is MethodDeclaration)
            {
                for (int i = 0; i < Cache.GetHeaderNodes().Count; i++)
                {
                    if (Cache.GetHeaderNodes().ElementAt(i) is HeaderMethodDeclaration)
                    {
                        var hc = Cache.GetHeaderNodes().ElementAt(i) as HeaderMethodDeclaration;
                        if (hc.Name == (node as MethodDeclaration).Name &&
                            GetTypeName(hc.PrivateImplementationType) == GetTypeName((node as MethodDeclaration).PrivateImplementationType))
                        {
                            Cache.GetHeaderNodes().RemoveAt(i);
                        }
                    }
                }
            }
            else if (node is ConversionConstructorDeclaration)
            {
                for (int i = 0; i < Cache.GetHeaderNodes().Count; i++)
                {
                    if (Cache.GetHeaderNodes().ElementAt(i) is HeaderConversionConstructorDeclaration)
                    {
                        var hc = Cache.GetHeaderNodes().ElementAt(i) as HeaderConversionConstructorDeclaration;
                        if (GetTypeName(hc.ReturnType) == GetTypeName((node as ConversionConstructorDeclaration).ReturnType))
                        {
                            Cache.GetHeaderNodes().RemoveAt(i);
                        }
                    }
                }
            }
        }

        public static void GetNestedTypes(TypeDeclaration currentType)
        {
            //Trim the type name to avoid errors with generic types
            string currentTypeName = currentType.Name.TrimEnd("_Base".ToCharArray()).TrimEnd("_T".ToCharArray());

            //SEARCH FOR THE METHODS IN THE CURRENT TYPE THAT SHOULD BE IMPLEMENTED IN A NESTED CLASS
            List<MethodDeclaration> methodsOfCurrentType = new List<MethodDeclaration>();
            foreach (KeyValuePair<AstType, List<MethodDeclaration>> kvp in Cache.GetPrivateImplementation())
            {
                foreach (MethodDeclaration m in kvp.Value)
                {
                    if (m.TypeMember.Name == currentTypeName)
                        methodsOfCurrentType.Add(m);
                }
            }

            //CREATE A DICTIONARY WITH THE NESTEDCLASS AND METHODS NEEDED (SORTED)
            Dictionary<AstType, List<MethodDeclaration>> privateImplClass = new Dictionary<AstType, List<MethodDeclaration>>();
            foreach (MethodDeclaration m in methodsOfCurrentType)
            {
                foreach (KeyValuePair<AstType, List<MethodDeclaration>> kvp in Cache.GetPrivateImplementation())
                {
                    if (kvp.Value.Contains(m))
                    {
                        //If the method private implementation is corresponding to the current key type in the loop, continue
                        if (GetTypeName(m.PrivateImplementationType) == GetTypeName(kvp.Key))
                        {
                            //REMOVE THE METHOD FROM THE CURRENT TYPE                            
                            currentType.Members.Remove(m);
                            RemoveHeaderNode(m);

                            //REMOVE THE PRIVATE IMPLEMENTATION
                            m.PrivateImplementationType = Ast.AstType.Null;

                            //Add the method in the sorted dictionary
                            if (privateImplClass.ContainsKey(kvp.Key))
                                privateImplClass[kvp.Key].Add((MethodDeclaration)m.Clone());

                            else
                                privateImplClass.Add(kvp.Key, new List<MethodDeclaration>() { (MethodDeclaration)m.Clone() });

                        }
                    }
                }
            }

            //CREATE FROM THE NESTED CLASS THE NESTEDTYPEDECLARATION NODE
            foreach (KeyValuePair<AstType, List<MethodDeclaration>> kvp in privateImplClass)
            {
                TypeDeclaration type = new TypeDeclaration();
                string nestedTypeName = "_interface_" + GetTypeName(kvp.Key);
                type.NameToken = new Identifier(nestedTypeName, TextLocation.Empty);
                type.Name = nestedTypeName;
                type.ModifierTokens.Add(new CppModifierToken(TextLocation.Empty, Modifiers.Public));

                if (kvp.Key is SimpleType)
                {
                    foreach (AstType tp in (kvp.Key as SimpleType).TypeArguments)
                        type.TypeParameters.Add(new TypeParameterDeclaration() { NameToken = new Identifier(GetTypeName(tp), TextLocation.Empty) });
                }

                //ADD BASE TYPES
                SimpleType baseType = new SimpleType(GetTypeName(kvp.Key));
                if (kvp.Key is SimpleType)
                {
                    foreach (AstType tp in (kvp.Key as SimpleType).TypeArguments)
                        baseType.TypeArguments.Add((AstType)tp.Clone());
                }

                type.AddChild(baseType, TypeDeclaration.BaseTypeRole);

                //REMOVE THE BASE TYPE BECAUSE THE NESTED TYPE WILL INHERIT FROM IT
                currentType.BaseTypes.Remove(currentType.BaseTypes.First(x => GetTypeName(x) == GetTypeName(baseType)));

                //ADD METHODS
                type.Members.AddRange(kvp.Value);



                //ADD FIELD
                HeaderFieldDeclaration fdecl = new HeaderFieldDeclaration();
                SimpleType nestedType = new SimpleType(nestedTypeName);
                foreach (TypeParameterDeclaration tp in type.TypeParameters)
                    nestedType.TypeArguments.Add(new SimpleType(tp.Name));

                ExplicitInterfaceTypeDeclaration ntype = new Ast.ExplicitInterfaceTypeDeclaration(type);

                fdecl.ReturnType = nestedType;
                string _tmp = "_" + GetTypeName(fdecl.ReturnType).ToLower();
                fdecl.Variables.Add(new VariableInitializer(_tmp));
                //ADD FIELD TO THE GLOBAL CLASS
                ntype.OutMembers.Add(fdecl);


                //ADD OPERATORS TO THE GLOBAL CLASS
                //ADD OPERATOR HEADER NODE
                ConversionConstructorDeclaration op = new ConversionConstructorDeclaration();
                op.ReturnType = new PtrType((AstType)kvp.Key.Clone());
                op.ModifierTokens.Add(new CppModifierToken(TextLocation.Empty, Modifiers.Public));

                //In the first line of this method we have trimed end the string with the _T and the _Base for searching the lists
                //At this point we have to add that string
                if (currentType.TypeParameters.Any())
                    op.type = currentTypeName + "_T_Base";
                else
                    op.type = currentTypeName;
                BlockStatement blck = new BlockStatement();
                Statement st = new ReturnStatement(new AddressOfExpression(new IdentifierExpression(_tmp)));
                blck.Add(st);
                op.Body = blck;

                currentType.Members.Add(op);


                //If is generic type, we have to implement the operator inside of the templates header
                if (currentType.TypeParameters.Any())
                {
                    op.type = String.Empty;
                    ntype.OutMembers.Add((AttributedNode)op.Clone());
                    //The method is implemented, we must delete the node from the header nodes
                    RemoveHeaderNode(op);
                }
                else
                {
                    HeaderConversionConstructorDeclaration hc = new HeaderConversionConstructorDeclaration();
                    GetHeaderNode(op, hc);
                    //Add the header member to the out members of the nested type
                    ntype.OutMembers.Add(hc);
                }


                //ADD NESTED TYPE TO THE HEADER DECLARATION
                Cache.AddHeaderNode(ntype);
            }
        }

        public static void GetHeaderNode(AstNode node, AstNode headerNode)
        {
            if (node is ConstructorDeclaration)
            {
                var _node = node as ConstructorDeclaration;
                var _header = headerNode as HeaderConstructorDeclaration;

                foreach (var token in _node.ModifierTokens)
                    headerNode.AddChild((CppModifierToken)token.Clone(), HeaderConstructorDeclaration.ModifierRole);

                foreach (var att in _node.Attributes)
                    headerNode.AddChild((AttributeSection)att.Clone(), HeaderConstructorDeclaration.AttributeRole);

                foreach (var param in _node.Parameters)
                    headerNode.AddChild((ParameterDeclaration)param.Clone(), HeaderConstructorDeclaration.Roles.Parameter);

                _header.Name = _node.Name;
                _header.IdentifierToken = (Identifier)_node.IdentifierToken.Clone();

            }
            if (node is DestructorDeclaration)
            {
                var _node = node as DestructorDeclaration;
                var _header = headerNode as HeaderDestructorDeclaration;

                foreach (var token in _node.ModifierTokens)
                    headerNode.AddChild((CppModifierToken)token.Clone(), HeaderConstructorDeclaration.ModifierRole);

                foreach (var att in _node.Attributes)
                    headerNode.AddChild((AttributeSection)att.Clone(), HeaderConstructorDeclaration.AttributeRole);

                _header.Name = _node.Name;
            }
            if (node is FieldDeclaration)
            {
                var _node = node as FieldDeclaration;
                var _header = headerNode as HeaderFieldDeclaration;

                _header.ReturnType = (AstType)_node.ReturnType.Clone();

                foreach (var token in _node.ModifierTokens)
                    headerNode.AddChild((CppModifierToken)token.Clone(), HeaderConstructorDeclaration.ModifierRole);

                foreach (var att in _node.Attributes)
                    headerNode.AddChild((AttributeSection)att.Clone(), HeaderConstructorDeclaration.AttributeRole);

                for (int i = 0; i < _node.Variables.Count; i++)
                {
                    VariableInitializer vi = _node.Variables.ElementAt(i);

                    _header.Variables.Add(_node.HasModifier(Modifiers.Static) ? new VariableInitializer(vi.Name) : (VariableInitializer)vi.Clone());
                }
            }
            if (node is MethodDeclaration)
            {
                var _node = node as MethodDeclaration;
                var _header = headerNode as HeaderMethodDeclaration;

                foreach (var token in _node.ModifierTokens)
                    headerNode.AddChild((CppModifierToken)token.Clone(), HeaderConstructorDeclaration.ModifierRole);

                foreach (var att in _node.Attributes)
                    headerNode.AddChild((AttributeSection)att.Clone(), HeaderConstructorDeclaration.AttributeRole);

                foreach (var param in _node.Parameters)
                    headerNode.AddChild((ParameterDeclaration)param.Clone(), HeaderConstructorDeclaration.Roles.Parameter);

                foreach (var tparam in _node.TypeParameters)
                    _header.TypeParameters.Add((TypeParameterDeclaration)tparam.Clone());

                _header.ReturnType = (AstType)_node.ReturnType.Clone();
                _header.PrivateImplementationType = (AstType)_node.PrivateImplementationType.Clone();
                _header.TypeMember = (Identifier)_node.TypeMember.Clone();
                _header.NameToken = (Identifier)_node.NameToken.Clone();
            }
            if (node is ConversionConstructorDeclaration)
            {
                var _node = node as ConversionConstructorDeclaration;
                var _header = headerNode as HeaderConversionConstructorDeclaration;

                foreach (var token in _node.ModifierTokens)
                    headerNode.AddChild((CppModifierToken)token.Clone(), HeaderConstructorDeclaration.ModifierRole);

                foreach (var att in _node.Attributes)
                    headerNode.AddChild((AttributeSection)att.Clone(), HeaderConstructorDeclaration.AttributeRole);

                _header.ReturnType = (AstType)_node.ReturnType.Clone();
            }
        }

        public static bool NeedsDereference(CSharp.AstNode node, string currentType, string currentMethod)
        {
            //This method can be implemented in a more optimized way, but I prefer distinguish all the cases for control better the process
            if (node is CSharp.IdentifierExpression)
            {
                //I THINK THAT WITH IDENTIFIER EXPRESSIONS WE DO NOT NEED TO CHECK ANYTHING...
                return false;
                #region unused code
                var identifierExpression = node as CSharp.IdentifierExpression;
                //We must check if the return type is different to the original type, if that is, it is necessar or maybe a cast, or maybe there is an operator. In both cases the identifier must be de-referenced:
                //From IA* a = c; we must obtain IA* a = *c;
                //TODO: MAYBE we should add a cast for security ?
                if (IsChildOf(node, typeof(CSharp.VariableInitializer)))
                {
                    //TODO: IS CHILD OF INVOCATION EXPRESSION, SO, THE IDENTIFIEREXPRESSION IS A PARAMETER, 
                    //IS THE METHOD PARAMETER DECLARATION TYPE EQUAL TO THE IDENTIFIER EXPRESSION TYPE?
                    if (IsChildOf(node, typeof(CSharp.InvocationExpression)))
                    {
                        return false;
                    }
                    //IS CHILD OF MEMBERREFERENCEEXPRESSION, SO, THE IDENTIFIEREXPRESSION IS A TARGET MEMBER (i.e. Target->Member()),
                    //I THINK THAT IT IS NOT NECESSARY A POINTER
                    else if (IsChildOf(node, typeof(CSharp.MemberReferenceExpression)))
                    {
                        return false;
                    }
                    //Expression like a[j] cannot be neither a[*j] nor *a[*j] ...
                    else if (IsChildOf(node, typeof(CSharp.IndexerExpression)))
                    {
                        return false;
                    }
                    //ASEXPRESSION OR ISEXPRESSION WILL BE PROCESSED AFTER
                    else if (IsChildOf(node, typeof(CSharp.AsExpression)) || IsChildOf(node, typeof(CSharp.IsExpression)))
                    {
                        return false;
                    }
                    //THE PARAMETERS IN THE OBJECTCREATEEXPRESSION (new Object(parameters))
                    else if (IsChildOf(node, typeof(CSharp.ObjectCreateExpression)))
                    {
                        return false;
                    }
                    else
                    {
                        if (IsChildOf(node, typeof(CSharp.FieldDeclaration)))
                        {
                            var fdecl = (CSharp.FieldDeclaration)GetParentOf(node, typeof(CSharp.FieldDeclaration));
                            if (Resolver.GetTypeName(fdecl.ReturnType) != Resolver.GetTypeName(Resolver.GetType(identifierExpression.Identifier, currentType, null, null)))
                            {
                                return true;
                            }
                        }
                        else if (IsChildOf(node, typeof(CSharp.VariableDeclarationStatement)))
                        {
                            var vdecl = (CSharp.VariableDeclarationStatement)GetParentOf(node, typeof(CSharp.VariableDeclarationStatement));
                            if (Resolver.GetTypeName(vdecl.Type) != Resolver.GetTypeName(Resolver.GetType(identifierExpression.Identifier, null, currentMethod, null)))
                            {
                                CSharp.MethodDeclaration m = (CSharp.MethodDeclaration)Resolver.GetParentOf(node, typeof(CSharp.MethodDeclaration));
                                return IsPointer(identifierExpression.Identifier, null, m == null ? String.Empty : m.Name, null);
                            }
                        }
                        else if (IsChildOf(node, typeof(CSharp.ParameterDeclaration)))
                        {
                            var pdecl = (CSharp.ParameterDeclaration)GetParentOf(node, typeof(CSharp.ParameterDeclaration));
                            if (Resolver.GetTypeName(pdecl.Type) != Resolver.GetTypeName(Resolver.GetType(identifierExpression.Identifier, null, currentMethod, pdecl.Name)))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
                else//! IS CHILD OF VARIABLEINITIALIZER
                {
                    return false;
                }
                #endregion
            }
            else if (node is CSharp.IndexerExpression)
            {
                var indexerExpression = node as CSharp.IndexerExpression;
                if (indexerExpression.Target is CSharp.MemberReferenceExpression)
                {
                    var iexpTar = indexerExpression.Target as CSharp.MemberReferenceExpression;
                    if (iexpTar.Target is CSharp.ThisReferenceExpression)
                    {
                        return Resolver.IsPointer(iexpTar.MemberName, currentType, null, null);
                    }
                    else if (iexpTar.Target is CSharp.IdentifierExpression)
                    {
                        var id = iexpTar.Target as CSharp.IdentifierExpression;
                        return Resolver.IsPointer(iexpTar.MemberName, id.Identifier, null, null);
                    }
                    else
                        return false;
                }
                else if (indexerExpression.Target is CSharp.IdentifierExpression)
                {
                    var iexpTar = indexerExpression.Target as CSharp.IdentifierExpression;
                    return Resolver.IsPointer(iexpTar.Identifier, null, currentMethod, iexpTar.Identifier);
                }
                else
                    return false;
            }
            else //! IS IDENTIFIEREXPRESSION
                return false;
        }

        public static bool IsTemplateType(AstType type)
        {
            return Cache.GetTemplateTypes().FirstOrDefault(x => x == GetTypeName(type)) != null;


        }

        /// <summary>
        /// Checks if the node is child of other node of the specified type
        /// </summary>
        /// <param name="member">Node</param>
        /// <param name="type">Type of the parent node</param>
        /// <returns>Bool indicating if is child or not</returns>
        public static bool IsChildOf(AstNode member, Type type)
        {
            AstNode m = member as AstNode;
            while (m.Parent != null)
            {
                if (m.Parent.GetType() == type)
                {
                    return true;
                }
                m = m.Parent;
            }
            return false;
        }

        /// <summary>
        /// Checks if the node is child of other node of the specified type
        /// </summary>
        /// <param name="member">Node</param>
        /// <param name="type">Type of the parent node</param>
        /// <returns>Bool indicating if is child or not</returns>
        public static bool IsChildOf(CSharp.AstNode member, Type type)
        {
            CSharp.AstNode m = member as CSharp.AstNode;
            while (m.Parent != null)
            {
                if (m.Parent.GetType() == type)
                {
                    return true;
                }
                m = m.Parent;
            }
            return false;
        }

        /// <summary>
        /// Returns the first parent node of type specified by variable type
        /// </summary>
        /// <param name="member">Original node</param>
        /// <param name="type">Target type of</param>
        /// <returns>The resulting node</returns>
        public static AstNode GetParentOf(AstNode member, Type type)
        {
            AstNode m = member as AstNode;
            while (m.Parent != null)
            {
                if (m.Parent.GetType() == type)
                {
                    return m.Parent;
                }
                m = m.Parent;
            }
            return AstNode.Null;
        }

        /// <summary>
        /// Returns the first parent node of type specified by variable type
        /// </summary>
        /// <param name="member">Original node</param>
        /// <param name="type">Target type of</param>
        /// <returns>The resulting node</returns>
        public static CSharp.AstNode GetParentOf(CSharp.AstNode member, Type type)
        {
            CSharp.AstNode m = member as CSharp.AstNode;
            while (m.Parent != null)
            {
                if (m.Parent.GetType() == type)
                {
                    return m.Parent;
                }
                m = m.Parent;
            }
            return CSharp.AstNode.Null;
        }

        //TODO: move this method to C#2CPPCONVERTERVISITOR
        public static bool TryPatchTemplateToObjectType(AstType type, out AstType newType)
        {
            newType = (AstType)type.Clone();
            string name = "";
            if (type is SimpleType)
            {
                SimpleType st = type as SimpleType;
                name = st.Identifier;
                if (Cache.GetTemplateTypes().Contains(name))
                {
                    newType = new SimpleType("Object");
                    //if (Resolver.IsChildOf(type, typeof(TypeParameterDeclaration)))
                    //    newType = new SimpleType("Object");
                    //else
                    //    newType = new PtrType(new SimpleType("Object"));
                    return true;
                }
                else
                {
                    if (st.TypeArguments.Any())
                    {
                        List<AstType> args = new List<AstType>();
                        bool converted = false;
                        foreach (AstType t in st.TypeArguments)
                        {
                            AstType discard;
                            if (TryPatchTemplateToObjectType(t, out discard))
                            {
                                converted = true;
                                args.Add(new SimpleType("Object"));
                            }
                            else
                                args.Add((AstType)t.Clone());
                        }

                        SimpleType nType = (SimpleType)st.Clone();
                        nType.TypeArguments.Clear();
                        nType.TypeArguments.AddRange(args.ToArray());
                        newType = nType;
                        return converted;
                    }
                }
            }
            if (type is PtrType)
            {
                if ((type as PtrType).Target is SimpleType)
                {
                    SimpleType pst = (type as PtrType).Target as SimpleType;
                    AstType tmp;
                    bool converted = TryPatchTemplateToObjectType(pst, out tmp);
                    newType = new PtrType(tmp);
                    return converted;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the member string for calling a conversion constructor declaration: i.e. operator IB<Object>*();
        /// </summary>
        /// <param name="conv">The conversion constructor declaration</param>
        /// <returns>The member string</returns>
        public static string GetInlineConversionConstructorDeclarationCall(ConversionConstructorDeclaration conv)
        {
            //ESTA FUNCION NO ME GUSTA NADA !
            //HAY QUE CREAR NODOS INLINE    
            string ret = "operator ";
            ret += Resolver.GetTypeName(conv.ReturnType);

            if (conv.ReturnType is SimpleType)
            {
                if ((conv.ReturnType as SimpleType).TypeArguments.Any())
                {
                    bool first = true;
                    ret += "<";
                    foreach (AstType t in (conv.ReturnType as SimpleType).TypeArguments)
                    {
                        if (first)
                            first = false;
                        else
                            ret += ",";

                        AstType tmp;
                        TryPatchTemplateToObjectType(t, out tmp);
                        ret += Resolver.GetTypeName(tmp);
                    }
                    ret += "*>";
                }
            }
            else if (conv.ReturnType is PtrType)
            {
                PtrType ptr = conv.ReturnType as PtrType;
                if (ptr.Target is SimpleType)
                {
                    if ((ptr.Target as SimpleType).TypeArguments.Any())
                    {
                        bool first = true;
                        ret += "<";
                        foreach (AstType t in (ptr.Target as SimpleType).TypeArguments)
                        {
                            if (first)
                                first = false;
                            else
                                ret += ",";

                            AstType tmp;
                            TryPatchTemplateToObjectType(t, out tmp);
                            ret += Resolver.GetTypeName(tmp);
                        }
                        ret += "*>";
                    }
                }
                ret += "*";
            }
            return ret;

        }

        public static bool IsPropertyCall(MemberReferenceExpression memberReferenceExpression, string currentTypeName)
        {
            Dictionary<string, List<string>> properties = Cache.GetPropertiesList();

            //The member reference is a property reference ?
            if (properties.ContainsKey(memberReferenceExpression.MemberName))
            {
                if (memberReferenceExpression.Target is ThisReferenceExpression)
                {
                    return (properties[memberReferenceExpression.MemberName].Contains(currentTypeName));
                }
                else
                {
                    if (memberReferenceExpression.Target is IdentifierExpression)
                    {
                        IdentifierExpression tmp = memberReferenceExpression.Target as IdentifierExpression;
                        ICSharpCode.Decompiler.Ast.TypeInformation ann = (ICSharpCode.Decompiler.Ast.TypeInformation)tmp.Annotation(typeof(ICSharpCode.Decompiler.Ast.TypeInformation));
                        return (properties[memberReferenceExpression.MemberName].Contains(ann.InferredType.Name));
                    }
                }
            }

            return false;
        }
    }
}
