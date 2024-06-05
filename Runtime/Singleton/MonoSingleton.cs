using Unity.Linq;
using UnityEngine;

namespace NTool.Singleton
{
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        /// <summary>
        /// 静态实例
        /// </summary>
        private static T _instance;

        /// <summary>
        /// 静态属性：封装相关实例对象
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SingletonCreator.CreateMonoSingleton<T>();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 实现接口的单例初始化
        /// </summary>
        public virtual void OnSingletonInit()
        {
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public virtual void Dispose()
        {
            if (SingletonCreator.IsUnitTestMode)
            {
                gameObject.AncestorsAndSelf().Destroy(true);
                _instance = null;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        /// <summary>
        /// 应用程序退出：释放当前对象并销毁相关GameObject
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            if (_instance == null) return;
            Destroy(_instance.gameObject);
            _instance = null;
        }

        /// <summary>
        /// 释放当前对象
        /// </summary>
        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}