using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace NTool.Config
{
    public abstract class XmlConfigBase<T> : ConfigBase<T> where T : JsonConfigBase<T>, new()
    {
        protected override string ConfigPath =>
#if UNITY_ANDROID || UNITY_IOS
            Application.persistentDataPath;
#elif UNITY_EDITOR
            Application.streamingAssetsPath;
#endif

        protected override string Serialization(T obj)
        {
            var       serializer = new XmlSerializer(typeof(T));
            using var writer     = new StringWriter();
            serializer.Serialize(writer, obj);
            return writer.ToString();
        }

        protected override T Deserialization(string serialized)
        {
            var       serializer = new XmlSerializer(typeof(T));
            using var reader     = new StringReader(serialized);
            return (T)serializer.Deserialize(reader);
        }
    }
}