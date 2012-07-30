using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace ICSharpCode.NRefactory.Cpp
{
    public class Cache
    {
        //OUTPUTVISITOR
        private static List<CSharp.ArraySpecifier> arraySpecifiers = new List<CSharp.ArraySpecifier>();
        private static Dictionary<string, List<string>> properties = new Dictionary<string, List<string>>();
        private static Dictionary<string, Ast.AstType> auxVariables = new Dictionary<string, Ast.AstType>();
        private static List<Ast.Statement> addedConstructorStatements = new List<Ast.Statement>();
        private static Dictionary<string, List<FieldDeclaration>> fields = new Dictionary<string, List<FieldDeclaration>>();

        //RESOLVER
        private static Dictionary<string, string> libraryMap = new Dictionary<string, string>();
        private static Dictionary<Ast.AstType, string> visitedTypes = new Dictionary<Ast.AstType, string>();
        private static List<string> namespaces = new List<string>();
        private static Dictionary<string, TypeReference> symbols = new Dictionary<string, TypeReference>();
        private static Dictionary<string, List<string>> includes = new Dictionary<string, List<string>>();
        private static List<string> excluded = new List<string>();

        #region RESOLVER

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

        public static void AddAuxVariable(Ast.AstType type, string identifier)
        {
            if (!auxVariables.ContainsKey(identifier))
                auxVariables.Add(identifier, type);
        }

        public static Dictionary<string, Ast.AstType> GetAuxVariables()
        {
            return auxVariables;
        }

        public static void ClearTmpVariables()
        {
            auxVariables.Clear();
        }

        public static void ClearResolver()
        {
            visitedTypes.Clear();
            namespaces.Clear();
        }

        public static List<string> GetExcluded()
        {
            return excluded;
        }

        public static List<string> GetNamespaces()
        {
            return namespaces;
        }

        public static void AddExcludedType(string type)
        {
            if (!excluded.Contains(type))
                excluded.Add(type);
        }

        public static Dictionary<Ast.AstType, string> GetVisitedTypes()
        {
            return visitedTypes;
        }

        public static void AddVisitedType(Ast.AstType type, string name)
        {
            if (!visitedTypes.ContainsValue(name) && !visitedTypes.ContainsKey(type))
                visitedTypes.Add(type, name);
        }

        public static void AddSymbol(string type, TypeReference reference)
        {
            if (!symbols.ContainsKey(type))
                symbols.Add(type, reference);
        }

        public static void AddNamespace(string nameSpace)
        {
            if (!namespaces.Contains(nameSpace))
                namespaces.Add(nameSpace);
        }

        public static Dictionary<string, string> GetLibraryMap()
        {
            return libraryMap;
        }

        public static Dictionary<string, List<string>> GetIncludes()
        {
            return includes;
        }

        public static void SaveIncludes(Dictionary<string, List<string>> _includes)
        {
            includes = _includes;
        }

        public static void AddInclude(string owner, string included)
        {
            if (includes.ContainsKey(owner))
            {
                if (!includes[owner].Contains(included))
                    includes[owner].Add(included);
            }
            else
            {
                includes.Add(owner, new List<string>());
                includes[owner].Add(included);
            }
        }

        public static void InitLibrary(Dictionary<string, string> map)
        {
            libraryMap = map;
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
        #endregion

        #region OutputVisitor

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

        public static bool IsPointer(string currentType, string currentField)
        {
            if (fields.ContainsKey(currentType))
            {
                foreach (FieldDeclaration fd in fields[currentType])
                {
                    if (fd.ReturnType is Cpp.Ast.PtrType)
                    {
                        var col = fd.Variables;
                        if (col.First(x => x.Name == currentField) != null)
                            return true;
                    }
                }                
            }
            return false;
        }

        public static Dictionary<string, List<string>> GetPropertiesList()
        {
            return properties;
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
