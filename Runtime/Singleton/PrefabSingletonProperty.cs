using System;
using NTool.Extensions;
using Unity.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NTool.Singleton
{
    public class PrefabSingletonProperty<T> where T : MonoBehaviour
    {
        public static Func<string, GameObject> PrefabLoader = Resources.Load<GameObject>;

        private static T _instance;

        public static T InstanceWithLoader(Func<string, GameObject> loader)
        {
            PrefabLoader = loader;
            return Instance;
        }

        public static T Instance
        {
            get
            {
                if (_instance) return _instance;
                _instance = Object.FindFirstObjectByType<T>();
                if (_instance) return _instance;
                var prefab = PrefabLoader?.Invoke(typeof(T).Name);
                if (!prefab) return _instance;
                _instance = prefab.Instantiate().GetOrAddComponent<T>();
                _instance.DontDestroyOnLoad();

                return _instance;
            }
        }

        public static void Dispose()
        {
            _instance.gameObject.Destroy(SingletonCreator.IsUnitTestMode);
            _instance = null;
        }
    }
}