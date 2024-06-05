using System;
using System.Linq;
using System.Reflection;

namespace NTool.Extensions
{
    public static class SystemReflectionExtension
    {
        /// <summary>
        /// 反射调用方法
        /// </summary>
        /// <param name="self">指定对象</param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <returns>方法返回结果</returns>
        public static object ReflectionCallPrivateMethod<T>(
            this T self,
            string methodName,
            params object[] args
        )
        {
            var type = typeof(T);
            var methodInfo = type.GetMethod(
                methodName,
                BindingFlags.Instance | BindingFlags.NonPublic
            );

            return methodInfo?.Invoke(self, args);
        }

        /// <summary>
        /// 反射调用私有方法
        /// </summary>
        /// <param name="self">指定对象</param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <typeparam name="TReturnType">返回类型</typeparam>
        /// <returns>方法返回结果</returns>
        public static TReturnType ReflectionCallPrivateMethod<T, TReturnType>(
            this T self,
            string methodName,
            params object[] args
        )
        {
            return (TReturnType)self.ReflectionCallPrivateMethod(methodName, args);
        }

        /// <summary>
        /// 指定类型是否存在指定特性
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="inherit">继承</param>
        /// <typeparam name="T">特性</typeparam>
        /// <returns>是否存在</returns>
        public static bool HasAttribute<T>(this Type type, bool inherit = false)
            where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), inherit).Any();
        }

        /// <summary>
        /// 指定类型是否存在指定特性
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="attributeType">特性</param>
        /// <param name="inherit">继承</param>
        /// <returns>是否存在</returns>
        public static bool HasAttribute(this Type type, Type attributeType, bool inherit = false)
        {
            return type.GetCustomAttributes(attributeType, inherit).Any();
        }

        /// <summary>
        /// 指定属性是否存在指定特性
        /// </summary>
        /// <param name="prop">属性</param>
        /// <param name="inherit">继承</param>
        /// <typeparam name="T">特性</typeparam>
        /// <returns>是否存在</returns>
        public static bool HasAttribute<T>(this PropertyInfo prop, bool inherit = false)
            where T : Attribute
        {
            return prop.GetCustomAttributes(typeof(T), inherit).Any();
        }

        /// <summary>
        /// 指定属性是否存在指定特性
        /// </summary>
        /// <param name="prop">属性</param>
        /// <param name="attributeType">特性</param>
        /// <param name="inherit">继承</param>
        /// <returns>是否存在</returns>
        public static bool HasAttribute(
            this PropertyInfo prop,
            Type attributeType,
            bool inherit = false
        )
        {
            return prop.GetCustomAttributes(attributeType, inherit).Any();
        }

        /// <summary>
        /// 指定字段是否存在指定特性
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="inherit">继承</param>
        /// <typeparam name="T">特性</typeparam>
        /// <returns>是否存在</returns>
        public static bool HasAttribute<T>(this FieldInfo field, bool inherit = false)
            where T : Attribute
        {
            return field.GetCustomAttributes(typeof(T), inherit).Any();
        }

        /// <summary>
        /// 指定字段是否存在指定特性
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="attributeType">特性</param>
        /// <param name="inherit">继承</param>
        /// <returns>是否存在</returns>
        public static bool HasAttribute(this FieldInfo field, Type attributeType, bool inherit)
        {
            return field.GetCustomAttributes(attributeType, inherit).Any();
        }

        /// <summary>
        /// 指定方法是否存在指定特性
        /// </summary>
        /// <param name="method">字段</param>
        /// <param name="inherit">继承</param>
        /// <typeparam name="T">特性</typeparam>
        /// <returns>是否存在</returns>
        public static bool HasAttribute<T>(this MethodInfo method, bool inherit = false)
            where T : Attribute
        {
            return method.GetCustomAttributes(typeof(T), inherit).Any();
        }

        /// <summary>
        /// 指定方法是否存在指定特性
        /// </summary>
        /// <param name="method">字段</param>
        /// <param name="attributeType">特性</param>
        /// <param name="inherit">继承</param>
        /// <returns>是否存在</returns>
        public static bool HasAttribute(
            this MethodInfo method,
            Type attributeType,
            bool inherit = false
        )
        {
            return method.GetCustomAttributes(attributeType, inherit).Any();
        }

        public static T GetAttribute<T>(this Type type, bool inherit = false)
            where T : Attribute
        {
            return type.GetCustomAttributes<T>(inherit).FirstOrDefault();
        }

        public static object GetAttribute(this Type type, Type attributeType, bool inherit = false)
        {
            return type.GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        public static T GetAttribute<T>(this MethodInfo method, bool inherit = false)
            where T : Attribute
        {
            return method.GetCustomAttributes<T>(inherit).FirstOrDefault();
        }

        public static object GetAttribute(
            this MethodInfo method,
            Type attributeType,
            bool inherit = false
        )
        {
            return method.GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        public static T GetAttribute<T>(this FieldInfo field, bool inherit = false)
            where T : Attribute
        {
            return field.GetCustomAttributes<T>(inherit).FirstOrDefault();
        }

        public static object GetAttribute(
            this FieldInfo field,
            Type attributeType,
            bool inherit = false
        )
        {
            return field.GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        public static T GetAttribute<T>(this PropertyInfo prop, bool inherit = false)
            where T : Attribute
        {
            return prop.GetCustomAttributes<T>(inherit).FirstOrDefault();
        }

        public static object GetAttribute(
            this PropertyInfo prop,
            Type attributeType,
            bool inherit = false
        )
        {
            return prop.GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }
    }
}
