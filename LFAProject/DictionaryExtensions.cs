using System.Collections.Generic;
using System.Linq;

namespace LFAProject
{
    public static class DictionaryExtensions
    {
        /// <summary>Resets all values of dictionary</summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dic"></param>
        /// <returns>Dictionary with values reiniciated</returns>
        public static Dictionary<string, List<int>> ResetValues<K, V>(this Dictionary<string, List<int>> dic)
        {
            dic.Keys.ToList().ForEach(x => dic[x] = new List<int>());
            return dic;
        }

        /// <summary>Checks if there are any false values in the dictionary</summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dic"></param>
        /// <returns>Key of entry that has a false value, otherwise an empty key</returns>
        public static List<int> FinishTransitions<K, V>(this Dictionary<List<int>, bool> dic)
        {
            var keyList = new List<int>();
            var keys = new List<List<int>>(dic.Keys);
            foreach (var key in keys)
            {
                if (dic[key] == false)
                {
                    keyList = key;
                    break;
                }
            }
            return keyList;
        }
    }
}
