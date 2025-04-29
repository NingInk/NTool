using System;
using System.Collections.Generic;
using System.Linq;

namespace NTool.Extensions
{
    public static class LinqExtension
    {
        public static T Choose<T>(params T[] args)
        {
            return args[UnityEngine.Random.Range(0, args.Length)];
        }

        public static T GetRandomItem<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();
            return array.ElementAt(new Random().Next(0, array.Length));
        }

        public static T GetAndRemoveRandomItem<T>(this List<T> list)
        {
            var randomIndex = UnityEngine.Random.Range(0, list.Count);
            var randomItem  = list[randomIndex];
            list.RemoveAt(randomIndex);
            return randomItem;
        }

        public static IEnumerable<T> Random<T>(this IEnumerable<T> self) =>
            self.OrderBy(_ => Guid.NewGuid());

        public static IEnumerable<T> Random<T>(this IEnumerable<T> self, int count)
        {
            var list    = self.ToList(); // 缓存，避免多次枚举
            var indices = Enumerable.Range(0, list.Count).OrderBy(_ => Guid.NewGuid()).Take(count);
            foreach (var i in indices)
                yield return list[i];
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            var items = self as T[] ?? self.ToArray();
            if (items.IsNullOrEmpty()) return items;
            foreach (var item in items)
            {
                action(item);
            }

            return items;
        }

        public static IEnumerable<T> ForEachReverse<T>(this IEnumerable<T> self, Action<T> action)
        {
            var array = self as T[] ?? self.ToArray();
            if (array.IsNullOrEmpty()) return array;
            foreach (var item in array.Reverse())
            {
                action?.Invoke(item);
            }

            return array;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<int, T> action)
        {
            var items = self as T[] ?? self.ToArray();
            if (items.IsNullOrEmpty()) return items;
            var index = 0;
            foreach (var item in items)
            {
                action(index, item);
                index++;
            }

            return items;
        }

        public static void ForEach<TK, TV>(this Dictionary<TK, TV> dict, Action<TK, TV> action)
        {
            using var dictE = dict.GetEnumerator();

            while (dictE.MoveNext())
            {
                var current = dictE.Current;
                action(current.Key, current.Value);
            }
        }

        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
            this   Dictionary<TKey, TValue>   dictionary,
            params Dictionary<TKey, TValue>[] dictionaries
        )
        {
            return new[] { dictionary }
                   .Concat(dictionaries)
                   .SelectMany(d => d)                             // 平铺所有 kv
                   .GroupBy(kv => kv.Key)                          // 按 key 分组
                   .ToDictionary(g => g.Key, g => g.Last().Value); // 使用最后一个（即最后出现的字典）作为值
        }


        public static void AddRange<TK, TV>(
            this Dictionary<TK, TV> dict,
            Dictionary<TK, TV>      addInDict,
            bool                  isOverride = false
        )
        {
            using var enumerator = addInDict.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (dict.ContainsKey(current.Key))
                {
                    if (isOverride)
                        dict[current.Key] = current.Value;
                    continue;
                }

                dict.Add(current.Key, current.Value);
            }
        }

        // TODO:
        public static bool IsNullOrEmpty<T>(this T[] collection) =>
            collection == null || collection.Length == 0;

        // TODO:
        public static bool IsNullOrEmpty<T>(this IList<T> collection) =>
            collection == null || collection.Count == 0;

        // TODO:
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) =>
            collection == null || !collection.Any();

        // TODO:
        public static bool IsNotNullAndEmpty<T>(this T[] collection) => !IsNullOrEmpty(collection);

        // TODO:
        public static bool IsNotNullAndEmpty<T>(this IList<T> collection) =>
            !IsNullOrEmpty(collection);

        // TODO:
        public static bool IsNotNullAndEmpty<T>(this IEnumerable<T> collection) =>
            !IsNullOrEmpty(collection);
    }
}