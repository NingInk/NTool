
using UnityEditor;
using UnityEngine;

namespace NTool.Extensions
{
    public static class EditorUtils
    {
        public static void EditorSelect(this Object obj)
        {
#if UNITY_EDITOR
            Selection.activeObject = obj;
#endif
        }

        static void EditorPing(this Object obj)
        {
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(obj);
#endif
        }
    }
}