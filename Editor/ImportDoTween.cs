using System;
using System.Linq;
using System.Reflection;
using NTool.Extensions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NTool.Editor
{
    public static class ImportDoTween
    {
        [InitializeOnLoadMethod]
        static void Check()
        {
            var asmdef = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(
                "Assets/Plugins/Demigiant/DOTween/Modules/DOTween.Module.asmdef"
            );
            if (asmdef == null)
            {
                var dotweens = AppDomain
                    .CurrentDomain.GetAssemblies()
                    .Where(t => t.GetName().Name.ToLower().StartsWith("dotween"));
                var assemblies = dotweens as Assembly[] ?? dotweens.ToArray();
                if (assemblies.Any())
                {
                    AssetDatabase.ImportPackage(
                        "Packages/com.bsning.ntool/DoTween.Module.unitypackage",
                        true
                    );
                    assemblies.Select(t => t.GetName().Name).ForEach(Debug.Log);
                }
            }
        }
    }
}