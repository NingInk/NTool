using System.IO;
using NTool.Extensions;
using UnityEngine;

namespace NTool.Config
{
    public interface IConfigBase
    {
        void Save();
    }

    public abstract class ConfigBase<T> : IConfigBase where T : ConfigBase<T>, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.Load(); // 尝试自动加载配置
                }

                return _instance;
            }
        }

        protected abstract string ConfigPath { get; }
        protected abstract string ConfigName { get; }

        // 序列化配置
        protected abstract string Serialization(T        obj);
        protected abstract T      Deserialization(string serialized);

        public virtual void Save()
        {
            var path = Path.Combine(ConfigPath, ConfigName);
            Debug.Log($"Save {typeof(T).Name.Color()} 到路径：{path.Color(Color.red)}");
            var text = Serialization((T)this);
            File.WriteAllText(path, text);
        }

        public virtual void Load()
        {
            var path = Path.Combine(ConfigPath, ConfigName);

            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                _instance = Deserialization(text);
            }
            else
            {
                _instance = (T)this;
                Save(); // 如果没找到配置，保存默认配置
            }
        }
    }
}