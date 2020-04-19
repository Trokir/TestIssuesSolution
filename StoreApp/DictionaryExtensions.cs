using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace StoreApp
{
    public static class DictionaryExtensions
    {

        /// <summary>
        /// Gets the converted dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">dict</exception>
        public static ConcurrentDictionary<TKey, TValue> GetConvertedDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            var dictionary = new ConcurrentDictionary<TKey, TValue>(dict);
            return dictionary;
        }


        private static readonly List<int> Keys = new List<int>();

        /// <summary>
        /// Gets the unique key for dictionary.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">dict</exception>
        public static int GetUniqueKey<TValue>(this ConcurrentDictionary<int, TValue> dict)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            var newKey = Keys.Count;
            switch (Type.GetTypeCode(typeof(TValue)))
            {
                case TypeCode.Int32:
                    newKey++;
                    if (!Keys.Contains(newKey))
                        Keys.Add(newKey);
                    break;
                case TypeCode.String:
                    newKey++;
                    if (!Keys.Contains(newKey))
                        Keys.Add(newKey);
                    break;
            }
            return newKey;
        }
    }
}