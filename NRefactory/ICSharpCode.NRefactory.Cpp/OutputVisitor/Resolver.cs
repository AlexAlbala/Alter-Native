using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace ICSharpCode.NRefactory.Cpp
{
    public class Resolver
    {
        private static List<string> ltmp = new List<string>();
        static Resolver()
        {
            Dictionary<string, string> libraryMap = new Dictionary<string, string>();
            libraryMap.Add("System", "\"System/System.h\""); //ADD CONSOLE, STRING AND SUPPORT            
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


        ///// <summary>
        ///// Returns if a forward declaration is needed between two types
        ///// </summary>
        ///// <param name="fw_dcl_type1">The type to test</param>
        ///// <param name="fw_dcl_type2">If the method is true, the second type is placed here</param>
        ///// <returns></returns>
        //public static bool NeedsForwardDeclaration(string fw_dcl_type1, out string fw_dcl_type2)
        //{
        //    //The type is included ?
        //    //If not... we have a problem !
        //    if (includes.ContainsKey(fw_dcl_type1))
        //    {
        //        //if one of the types is declared in includes...
        //        foreach (string type2_s in includes[fw_dcl_type1])
        //        {
        //            if (includes.ContainsKey(type2_s))
        //            {
        //                //search in type2 dependences if is declared type1
        //                List<string> type2Dep = includes[type2_s];
        //                if (type2Dep.Contains(fw_dcl_type1))
        //                {
        //                    fw_dcl_type2 = type2Dep.ElementAt(type2Dep.IndexOf(fw_dcl_type1));
        //                    return true;
        //                }
        //                else
        //                    continue;
        //            }
        //            else
        //                continue;
        //        }
        //    }
        //    else
        //        throw new InvalidOperationException("Must be included. It is impossible to enter this funcion before the type is included!");

        //    fw_dcl_type2 = String.Empty;
        //    return false;
        //}

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

        private static bool Reaches(string type1, string type2, Dictionary<string,List<string>> includes)
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

        public static void AddExcludedType(string type)
        {
            Cache.AddExcludedType(type);
        }

        public static string[] GetTypeIncludes()
        {
            Dictionary<Ast.AstType, string> visitedTypes = Cache.GetVisitedTypes();
            List<string> tmp = new List<string>();
            foreach (KeyValuePair<Ast.AstType, string> kvp in visitedTypes)
            {
                if (!kvp.Key.IsBasicType)
                {
                    if (!Cache.GetExcluded().Contains(kvp.Value) && !tmp.Contains(GetCppName(kvp.Value)))                    
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
            nameSpace = nameSpace.Replace(".", "_");
            Cache.AddNamespace(nameSpace);
        }

        public static string[] GetNeededNamespaces()
        {
            return Cache.GetNamespaces().ToArray();
        }
    }
}
