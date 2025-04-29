using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NTool.Config;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace NTool.Editor.Windows
{
    public class ConfigWindow : OdinMenuEditorWindow
    {
        private static readonly HashSet<IConfigBase> Configs = new HashSet<IConfigBase>();

        public static void RegisterConfig(IConfigBase config)
        {
            Configs.Add(config);
        }

        [MenuItem("Window/Config #&C")]
        private static void Open()
        {
            GetWindow<ConfigWindow>();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            foreach (var config in Configs)
            {
                // var cso = CreateInstance<ConfigSo>();
                // cso.Config = config;
                // tree.Add(config.GetType().Name, cso);
                tree.Add(config.GetType().Name, config);
            }

            return tree;
        }

        public override void SaveChanges()
        {
            Debug.Log("Saving changes...");
            base.SaveChanges();
        }

        protected override void OnDestroy()
        {
            foreach (var config in Configs)
            {
                config.Save();
            }

            base.OnDestroy();
        }

        private static bool IsDerivedFromGeneric(Type type, Type generic)
        {
            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (cur == generic)
                    return true;
                type = type.BaseType;
            }

            return false;
        }
    }
}