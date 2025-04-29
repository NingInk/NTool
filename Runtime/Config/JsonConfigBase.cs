using Newtonsoft.Json;
using UnityEngine;

namespace NTool.Config
{
    public abstract class JsonConfigBase<T> : ConfigBase<T>
        where T : JsonConfigBase<T>, new()
    {
        protected override string ConfigPath =>
#if UNITY_ANDROID || UNITY_IOS
            Application.persistentDataPath;
#elif UNITY_EDITOR
            Application.streamingAssetsPath;
#endif

        protected override string Serialization(T obj) =>
            JsonConvert.SerializeObject(
                obj,
                new JsonSerializerSettings
                {
                    Formatting            = Formatting.Indented,
                    TypeNameHandling      = TypeNameHandling.Auto,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            );

        protected override T Deserialization(string serialized) =>
            JsonConvert.DeserializeObject<T>(serialized);
    }
}