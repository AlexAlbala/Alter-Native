using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace ICSharpCode.NRefactory.Cpp
{
    public class Cache
    {
        public static String entryPointNamespace = String.Empty;

        //OUTPUTVISITOR
        private static List<CSharp.ArraySpecifier> arraySpecifiers = new List<CSharp.ArraySpecifier>();
        private static Dictionary<string, List<string>> properties = new Dictionary<string, List<string>>();
        private static Dictionary<string, FieldDeclaration> auxVariables = new Dictionary<string, FieldDeclaration>();
        private static List<Ast.Statement> addedConstructorStatements = new List<Ast.Statement>();
        private static Dictionary<string, List<FieldDeclaration>> fields = new Dictionary<string, List<FieldDeclaration>>();
        private static Dictionary<string, List<VariableDeclarationStatement>> variablesMethod = new Dictionary<string, List<VariableDeclarationStatement>>();
        private static Dictionary<string, List<ParameterDeclaration>> parameters = new Dictionary<string, List<ParameterDeclaration>>();
        private static List<IncludeDeclaration> includeDeclaration = new List<IncludeDeclaration>();
        private static List<String> removedIncludes = new List<String>();
        private static Dictionary<Ast.AstType, List<MethodDeclaration>> privateImplementations = new Dictionary<Ast.AstType, List<MethodDeclaration>>();
        private static Dictionary<String, List<String>> templatizedAbstractMethods = new Dictionary<string, List<String>>();
        private static List<AstNode> extraHeadernodes = new List<AstNode>();

        //RESOLVER
        private static Dictionary<string, string> libraryMap = new Dictionary<string, string>();
        private static Dictionary<Ast.AstType, string> visitedTypes = new Dictionary<Ast.AstType, string>();
        private static List<string> namespaces = new List<string>();
        private static Dictionary<string, TypeReference> symbols = new Dictionary<string, TypeReference>();
        private static Dictionary<string, List<string>> includes = new Dictionary<string, List<string>>();
        private static List<string> templateTypes = new List<string>();

        //DELEGATES & EVENTS

        //Type -> customEvents
        private static Dictionary<string, List<string>> customEvents = new Dictionary<string, List<string>>();

        //Name->Args
        private static Dictionary<string, ParameterDeclaration[]> delegatesArgs = new Dictionary<string, ParameterDeclaration[]>();

        //Name->ReturnType
        private static Dictionary<string, string> delegateReturnType = new Dictionary<string, string>();

        //Name->Type
        private static Dictionary<string, string> delegatesIdentifiers = new Dictionary<string, string>();

        //Name->Type
        private static Dictionary<string, string> eventIdentifiers = new Dictionary<string, string>();




        //PINVOKE AND DLLIMPORTS*********************************************************
        private static Dictionary<string, List<ExternMethodDeclaration>> dllImports = new Dictionary<string, List<ExternMethodDeclaration>>();        

        #region RESOLVER

        public static void AddPrivateImplementation(Ast.AstType type, MethodDeclaration method)
        {
            if (privateImplementations.ContainsKey(type))
                privateImplementations[type].Add(method);
            else
                privateImplementations.Add(type, new List<MethodDeclaration>() { method });
        }

        public static Dictionary<Ast.AstType, List<MethodDeclaration>> GetPrivateImplementation()
        {
            return privateImplementations;
        }

        public static void AddDllImport(String name, ExternMethodDeclaration method)
        {
            if (dllImports.ContainsKey(name))
                dllImports[name].Add(method);
            else
                dllImports.Add(name, new List<ExternMethodDeclaration>() { method });
        }

        public static Dictionary<String, List<ExternMethodDeclaration>> GetDllImport()
        {
            return dllImports;
        }

        public static void ClearDllImport()
        {
            dllImports.Clear();
        }

        public static void ClearPrivateImplementations()
        {
            privateImplementations.Clear();
        }

        public static void AddConstructorStatement(Ast.Statement statement)
        {
            addedConstructorStatements.Add(statement);
        }

        public static List<Ast.Statement> GetConstructorStatements()
        {
            return addedConstructorStatements;
        }

        public static void ClearConstructorStatements()
        {
            addedConstructorStatements.Clear();
        }

        public static void AddExtraHeaderNode(AstNode node)
        {
            extraHeadernodes.Add(node);
        }

        public static List<AstNode> GetExtraHeaderNode()
        {
            return extraHeadernodes;
        }

        public static void ClearExtraHeaderNode()
        {
            extraHeadernodes.Clear();
        }

        public static void AddAuxVariable(FieldDeclaration type, string identifier)
        {
            if (!auxVariables.ContainsKey(identifier))
                auxVariables.Add(identifier, type);
        }

        public static void AddDelegateType(String type, ParameterDeclaration[] arguments)
        {
            if (!delegatesArgs.ContainsKey(type))
            {
                delegatesArgs.Add(type, arguments);
            }
        }

        public static Dictionary<string, ParameterDeclaration[]> GetDelegateTypes()
        {
            return delegatesArgs;
        }

        public static void AddDelegateReturnType(String type, string arguments)
        {
            if (!delegateReturnType.ContainsKey(type))
            {
                delegateReturnType.Add(type, arguments);
            }
        }

        public static Dictionary<string, string> GetDelegateReturnType()
        {
            return delegateReturnType;
        }

        public static void AddDelegateIdentifiers(String identifier, String ofType)
        {
            if (!delegatesIdentifiers.ContainsKey(identifier))
            {
                delegatesIdentifiers.Add(identifier, ofType);
            }
        }

        public static void AddEventIdentifiers(String identifier, String ofType)
        {
            if (!eventIdentifiers.ContainsKey(identifier))
            {
                eventIdentifiers.Add(identifier, ofType);
            }
        }

        public static Dictionary<string, string> GetEventIdentifiers()
        {
            return eventIdentifiers;
        }

        public static Dictionary<string, string> GetDelegateIdentifiers()
        {
            return delegatesIdentifiers;
        }

        public static Dictionary<string, FieldDeclaration> GetAuxVariables()
        {
            return auxVariables;
        }

        public static void ClearAuxVariables()
        {
            auxVariables.Clear();
        }

        public static void ClearResolver()
        {
            visitedTypes.Clear();
            namespaces.Clear();
            extraHeadernodes.Clear();
            addedConstructorStatements.Clear();
        }

        public static void AddTemplateType(string type)
        {
            if (!templateTypes.Contains(type))
                templateTypes.Add(type);
        }

        public static List<string> GetTemplateTypes()
        {
            return templateTypes;
        }

        /// <summary>
        /// Adds a namespace of the current type
        /// </summary>
        /// <param name="nameSpace"></param>          
        public static void AddNamespace(string nameSpace)
        {
            nameSpace = nameSpace.Replace(".", "::");

            if (!namespaces.Contains(nameSpace))
                namespaces.Add(nameSpace);
        }

        public static List<string> GetNamespaces()
        {
            return namespaces;
        }

        /// <summary>
        /// Adds a type as a Visited type in order to track all the types that a class may include
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public static void AddVistedType(Ast.AstType type, string name)
        {
            if (!visitedTypes.ContainsValue(name) && !visitedTypes.ContainsKey(type))
                visitedTypes.Add(type, name);

            if (name == "Depth1")
            { }
            AddInclude(name);
        }

        public static Dictionary<Ast.AstType, string> GetVisitedTypes()
        {
            return visitedTypes;
        }

        /// <summary>
        /// Adds the symbol of a type in order to extract all the neessary information when needed
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reference"></param>
        public static void AddSymbol(string type, TypeReference reference)
        {
            if (!symbols.ContainsKey(type))
                symbols.Add(type, reference); ;

            string namesp = reference.Namespace;
            AddNamespace(namesp);
        }

        public static Dictionary<string, TypeReference> GetSymbols()
        {
            return symbols;
        }

        /// <summary>
        /// Adds a new include definition
        /// </summary>        
        /// <param name="included">The type included</param>
        public static void AddInclude(string included)
        {
            if (included == "Depth1")
            { }
            string owner = "N/P";

            if (includes.ContainsKey(owner))
            {
                if (!includes[owner].Contains(included))
                    includes[owner].Add(included);
            }
            else
            {
                includes.Add(owner, new List<string>(){"Sysetm"});
                includes[owner].Add(included);
            }
        }

        public static void RemoveIncldue(string name)
        {
            if (!removedIncludes.Contains(name))
            {
                removedIncludes.Add(name);
            }
        }

        public static List<string> GetRemovedIncludes()
        {
            return removedIncludes;
        }

        public static void SaveIncludes(Dictionary<string, List<string>> _includes)
        {
            includes = _includes;
        }

        public static Dictionary<string, List<string>> GetIncludes()
        {
            return includes;
        }

        public static void InitLibrary(Dictionary<string, string> map)
        {
            libraryMap = map;
        }

        public static Dictionary<string, string> GetLibraryMap()
        {
            return libraryMap;
        }
        #endregion

        #region OutputVisitor

        public static void AddTemplatizedAbstractMethod(string type, string method)
        {
            if (!templatizedAbstractMethods.ContainsKey(type))
                templatizedAbstractMethods.Add(type, new List<String>() { method });
            else
            {
                if (!templatizedAbstractMethods[type].Contains(method))
                    templatizedAbstractMethods[type].Add(method);
            }
        }

        public static Dictionary<string, List<string>> GetTemplatizedAbstractMethods()
        {
            return templatizedAbstractMethods;
        }

        /*public static void AddIncludeDeclaration(IncludeDeclaration node)
        {
            if (!includeDeclaration.Contains(node))
                includeDeclaration.Add(node);
        }*/

        /*public static List<IncludeDeclaration> GetIncludeDeclaration()
        {
            return includeDeclaration;
        }*/

        public static void ClearIncludeDeclaration()
        {
            includeDeclaration.Clear();
        }

        public static void AddField(string type, FieldDeclaration field)
        {
            if (!fields.ContainsKey(type))
            {
                fields.Add(type, new List<FieldDeclaration>() { field });
                return;
            }
            else
            {
                if (!fields[type].Contains(field))
                    fields[type].Add(field);
            }
        }

        public static Dictionary<string, List<FieldDeclaration>> GetFields()
        {
            return fields;
        }

        public static void AddParameterDeclaration(string methodName, ParameterDeclaration parameter)
        {
            if (!parameters.ContainsKey(methodName))
            {
                parameters.Add(methodName, new List<ParameterDeclaration>() { parameter });
                return;
            }
            else
            {
                if (!parameters[methodName].Contains(parameter))
                    parameters[methodName].Add(parameter);
            }
        }

        public static Dictionary<string, List<ParameterDeclaration>> GetParameters()
        {
            return parameters;
        }

        public static void AddMethodVariableDeclaration(string methodName, VariableDeclarationStatement field)
        {
            if (!variablesMethod.ContainsKey(methodName))
            {
                variablesMethod.Add(methodName, new List<VariableDeclarationStatement>() { field });
                return;
            }
            else
            {
                if (!variablesMethod[methodName].Contains(field))
                    variablesMethod[methodName].Add(field);
            }
        }

        public static void ClearParametersAndFieldsDeclarations()
        {
            parameters.Clear();
            variablesMethod.Clear();
        }

        public static Dictionary<string, List<VariableDeclarationStatement>> GetVariablesMethod()
        {
            return variablesMethod;
        }

        public static void AddProperty(string propertyName, string typeName)
        {
            if (properties.ContainsKey(propertyName))
            {
                List<string> tmpTypes = properties[propertyName];

                if (!tmpTypes.Contains(typeName))
                    tmpTypes.Add(typeName);
            }
            else
                properties.Add(propertyName, new List<string>() { typeName });
        }

        public static Dictionary<string, List<string>> GetPropertiesList()
        {
            return properties;
        }

        public static void AddCustomEvent(string customEventName, string typeName)
        {
            if (customEvents.ContainsKey(customEventName))
            {
                List<string> tmpTypes = customEvents[customEventName];

                if (!tmpTypes.Contains(typeName))
                    tmpTypes.Add(typeName);
            }
            else
                customEvents.Add(customEventName, new List<string>() { typeName });
        }

        public static Dictionary<string, List<string>> GetCustomEventsList()
        {
            return customEvents;
        }

        public static void AddRangeArraySpecifiers(IEnumerable<CSharp.ArraySpecifier> range)
        {
            arraySpecifiers.AddRange(range);
        }

        public static bool ArraySpecifiersAny()
        {
            return arraySpecifiers.Any();
        }

        public static List<CSharp.ArraySpecifier> GetArraySpecifiers()
        {
            return arraySpecifiers;
        }
        #endregion
    }
}
