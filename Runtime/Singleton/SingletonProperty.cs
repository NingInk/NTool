namespace NTool.Singleton
{
    public static class SingletonProperty<T> where T : class, ISingleton
    {
        /// <summary>
        /// 静态实例
        /// </summary>
        private static T _instance;

        /// <summary>
        /// 标签锁
        /// </summary>
        private static readonly object mLock = new object();

        /// <summary>
        /// 静态属性
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (mLock)
                {
                    _instance ??= SingletonCreator.CreateSingleton<T>();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public static void Dispose()
        {
            _instance = null;
        }
    }
}