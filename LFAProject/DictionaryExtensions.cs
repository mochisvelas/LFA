using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFAProject
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, List<int>> ResetValues<K, V>(this Dictionary<string, List<int>> dic)
        {
            dic.Keys.ToList().ForEach(x => dic[x] = new List<int>());
            return dic;
        }

        public static Dictionary<K, V> ResetValuesWithNewDictionary<K, V>(this Dictionary<K, V> dic)
        {
            return dic.ToDictionary(x => x.Key, x => default(V), dic.Comparer);
        }

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
