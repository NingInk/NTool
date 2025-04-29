using NTool.Config;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace NTool
{
    // public class UObject : Object
    // {
    //     [HideLabel] public object Value;
    // }
    //
    // public class UObject<T> : UObject
    // {
    //     [HideLabel]
    //     public new T Value
    //     {
    //         get => base.Value is T ? (T)base.Value : default;
    //         set => base.Value = value;
    //     }
    // }
    //
    public class ConfigSo : ScriptableObject
    {
        [HideLabel, ShowInInspector] public IConfigBase Config;
    }
}