namespace NTool.Extensions
{
    public static class EnumMethods
    {
        /// <summary>
        /// 将字符串转换成指定枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string str)
        {
            try
            {
                return (T)System.Enum.Parse(typeof(T), str);
            }
            catch (System.Exception)
            {
                UnityEngine.Debug.Log(str.Color());
                throw;
            }
        }
    }
}
