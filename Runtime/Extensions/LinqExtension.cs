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
            return array.ElementAt(new System.Random().Next(0, array.Length));
        }

        public static T GetAndRemoveRandomItem<T>(this List<T> list)
        {
            var randomIndex = UnityEngine.Random.Range(0, list.Count);
            var randomItem = list[randomIndex];
            list.RemoveAt(randomIndex);
            return randomItem;
        }

        public static IEnumerable<T> Random<T>(this IEnumerable<T> self) =>
            self.OrderBy(x => new System.Guid());

        public static IEnumerable<T> Random<T>(this IEnumerable<T> self, int count)
        {
            var rs = Enumerable.Range(0, count).Random();
            foreach (var r in rs)
            {
                yield return self.ElementAt(r);
            }
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            if (self.IsNotNullAndEmpty())
            {
                foreach (var item in self)
                {
                    action(item);
                }
            }

            return self;
        }

        public static IEnumerable<T> ForEachReverse<T>(this IEnumerable<T> self, Action<T> action)
        {
            if (self.IsNotNullAndEmpty())
            {
                foreach (var item in self.Reverse())
                {
                    action?.Invoke(item);
                }
            }

            return self;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<int, T> action)
        {
            if (self.IsNotNullAndEmpty())
            {
                var index = 0;
                foreach (var item in self)
                {
                    action(index, item);
                    index++;
                }
            }

            return self;
        }

        public static void ForEach<K, V>(this Dictionary<K, V> dict, Action<K, V> action)
        {
            using var dictE = dict.GetEnumerator();

            while (dictE.MoveNext())
            {
                var current = dictE.Current;
                action(current.Key, current.Value);
            }
        }

        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            params Dictionary<TKey, TValue>[] dictionaries
        )
        {
            return dictionaries.Aggregate(
                dictionary,
                (current, dict) => current.Union(dict).ToDictionary(kv => kv.Key, kv => kv.Value)
            );
        }

        public static void AddRange<K, V>(
            this Dictionary<K, V> dict,
            Dictionary<K, V> addInDict,
            bool isOverride = false
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
