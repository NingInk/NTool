using UnityEngine;

namespace NTool.Singleton
{
    public class ReplaceableMonoSingleton<T> : MonoBehaviour where T : ReplaceableMonoSingleton<T>
    {
        private static T _instance;

        public float InitializationTime;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    if (_instance == null)
                    {
                        var obj = new GameObject
                        {
                            hideFlags = HideFlags.HideAndDontSave
                        };
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            InitializationTime = Time.time;

            DontDestroyOnLoad(this.gameObject);

            var check = FindObjectsByType<T>(FindObjectsSortMode.None);
            foreach (var searched in check)
            {
                if (searched == this) continue;
                if (searched.InitializationTime < InitializationTime)
                {
                    Destroy(searched.gameObject);
                }
            }

            if (_instance == null)
            {
                _instance = this as T;
            }
        }
    }
}