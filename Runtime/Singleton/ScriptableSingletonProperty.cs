using System;
using UnityEngine;

namespace NTool.Singleton
{
    public class ScriptableSingletonProperty<T> where T : ScriptableObject
    {
        public static Func<string, T> ScriptableLoader = Resources.Load<T>;
        
        private static T _instance;

        public static T InstanceWithLoader(Func<string, T> loader)
        {
            ScriptableLoader = loader;
            return Instance;
        }
        
        public static T Instance
        {
            get
            {
                if (_instance == null) _instance = ScriptableLoader?.Invoke(typeof(T).Name);
                return _instance;
            }
        }

        public static void Dispose()
        {
            if (SingletonCreator.IsUnitTestMode)
            {
                Resources.UnloadAsset(_instance);
            }
            else
            {
                Resources.UnloadAsset(_instance);
            }

            _instance = null;
        }
    }
}
