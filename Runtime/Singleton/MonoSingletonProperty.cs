using UnityEngine;

namespace NTool.Singleton
{
    public static class MonoSingletonProperty<T> where T : MonoBehaviour, ISingleton
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = SingletonCreator.CreateMonoSingleton<T>();
                }

                return _instance;
            }
        }

        public static void Dispose()
        {
            if (SingletonCreator.IsUnitTestMode)
            {
                UnityEngine.Object.DestroyImmediate(_instance.gameObject);
            }
            else
            {
                UnityEngine.Object.Destroy(_instance.gameObject);
            }

            _instance = null;
        }
    }
}