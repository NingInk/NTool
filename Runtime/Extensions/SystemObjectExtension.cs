using System;

namespace NTool.Extensions
{
    public static class SystemObjectExtension
    {
        public static T Self<T>(this T self, Action<T> onDo)
        {
            onDo?.Invoke(self);
            return self;
        }

        public static T Self<T>(this T self, Func<T, T> onDo)
        {
            return onDo.Invoke(self);
        }

        /// <summary>
        /// 判空
        /// </summary>
        /// <param name="selfObj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>是否为空</returns>
        public static bool IsNull<T>(this T selfObj)
            where T : class
        {
            return null == selfObj;
        }

        /// <summary>
        /// 非空判断
        /// </summary>
        /// <param name="selfObj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>是否不为空</returns>
        public static bool IsNotNull<T>(this T selfObj)
            where T : class
        {
            return null != selfObj;
        }

        public static T As<T>(this object selfObj)
            where T : class
        {
            return selfObj as T;
        }
    }
}
