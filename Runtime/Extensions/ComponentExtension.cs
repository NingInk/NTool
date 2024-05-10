using System.Linq;
using Unity.Linq;
using UnityEngine;

namespace NTool.Extensions
{
    public static class ComponentExtension
    {
        /// <summary>
        /// 获取目标在Hierarchy中的全路径
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns>全路径</returns>
        public static string GetPath(this Component target) =>
            string.Join("/", target.gameObject.AncestorsAndSelf().Reverse().Select(g => g.name));

        /// <summary>
        /// 获取目标在Hierarchy中的全路径
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns>全路径</returns>
        public static string GetPath(this GameObject target) =>
            string.Join("/", target.AncestorsAndSelf().Reverse().Select(g => g.name));

        /// <summary>
        /// 保证目标具有 T[<see cref="Component"/>] 脚本并获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Component comp)
            where T : Component
        {
            return comp.GetComponent<T>() ?? comp.gameObject.AddComponent<T>();
        }

        /// <summary>
        /// 保证目标具有 T[<see cref="Component"/>] 脚本并获取
        /// </summary>
        /// <typeparam name="T">组件[<see cref="Component"/>]</typeparam>
        /// <param name="target">目标对象</param>
        /// <returns>获取到的组件</returns>
        public static T GetOrAddComponent<T>(this GameObject target)
            where T : Component
        {
            return target.gameObject.GetComponent<T>() ?? target.gameObject.AddComponent<T>();
        }

        /// <summary>
        /// 移除 T[<see cref="Component"/>] 组件
        /// </summary>
        /// <typeparam name="T">组件[<see cref="Component"/>]</typeparam>
        /// <param name="target">目标对象</param>
        /// <param name="immediate">立即执行</param>
        public static void RemoveComponent<T>(this GameObject target, bool immediate = false)
            where T : Component
        {
            if (target.TryGetComponent<T>(out var component))
            {
                if (immediate)
                {
                    Object.Destroy(component);
                }
                else
                {
                    Object.DestroyImmediate(component);
                }
            }
        }
    }
}
