using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Xutils.Extensions
{
    public static class IDictionaryExtensions
    {
        public static void AddOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);
            else
                dictionary[key] = value;
        }

        public static TValue GetOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, valueFactory());

            return dictionary[key];
        }

        public static TOutput GetValue<TOutput>(this IDictionary<string, object> sourceDictionary, string key)
        {
            if (!sourceDictionary.TryGetValue(key, out object outputValue))
                return default(TOutput);
            if (outputValue == null)
                return default(TOutput);
            return (TOutput)outputValue;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }

        public static async Task<TValue> GetOrSetAsync<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<Task<TValue>> valueFactory)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, await valueFactory());
            return dictionary[key];
        }
    }
}
