using UnityEngine;

namespace NTool.Singleton
{
    public abstract class PersistentMonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        protected bool mEnabled;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindFirstObjectByType<T>();
                if (_instance != null) return _instance;
                var obj = new GameObject();
                _instance = obj.AddComponent<T>();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(transform.gameObject);
                mEnabled = this.enabled;
            }
            else
            {
                if (this != _instance)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}