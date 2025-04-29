using System.Linq;
using Unity.Linq;
using UnityEngine;

namespace NTool.Extensions
{
    public static class ComponentExtension
    {
        public static T SetEnable<T>(this T component, bool enable)
            where T : Component
        {
            switch (component)
            {
                case Behaviour b:
                    b.enabled = enable;
                    break;
                case Collider collider:
                    collider.enabled = enable;
                    break;
                default:
                    Debug.LogWarning($"{component} 不是指定类型，不可以设置Enabled", component);
                    break;
            }

            return component;
        }

        /// <summary>
        /// 获取目标在Hierarchy中的全路径
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="includeSelf">包含自己</param>
        /// <returns>全路径</returns>
        public static string GetPath(this Component target, bool includeSelf = true) =>
            string.Join(
                "/",
                (includeSelf ? target.gameObject.AncestorsAndSelf() : target.gameObject.Ancestors())
                .Reverse()
                .Select(g => g.name)
            );

        public static string GetPath(
            this Component target,
            Object         until,
            bool           includeSelf = true
        ) =>
            string.Join(
                "/",
                (includeSelf ? target.gameObject.AncestorsAndSelf() : target.gameObject.Ancestors())
                .TakeWhile(g => g != until)
                .Reverse()
                .Select(g => g.name)
            );

        /// <summary>
        /// 获取目标在Hierarchy中的全路径
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="includeSelf">包含自己</param>
        /// <returns>全路径</returns>
        public static string GetPath(this GameObject target, bool includeSelf = true) =>
            string.Join(
                "/",
                (includeSelf ? target.AncestorsAndSelf() : target.Ancestors())
                .Reverse()
                .Select(g => g.name)
            );

        public static string GetPath(
            this GameObject target,
            Object          until,
            bool            includeSelf = true
        ) =>
            string.Join(
                "/",
                (includeSelf ? target.AncestorsAndSelf() : target.Ancestors())
                .TakeWhile(g => g != until)
                .Reverse()
                .Select(g => g.name)
            );

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
            return target.GetComponent<T>() ?? target.AddComponent<T>();
        }

        /// <summary>
        /// 移除 T[<see cref="Component"/>] 组件
        /// </summary>
        /// <typeparam name="T">组件[<see cref="Component"/>]</typeparam>
        /// <param name="target">目标对象</param>
        /// <param name="immediate">立即执行</param>
        public static GameObject RemoveComponent<T>(this GameObject target, bool immediate = false)
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

            return target;
        }
    }
}