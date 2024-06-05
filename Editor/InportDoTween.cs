using NTool.Extensions;
using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NTool
{
    public class InportDoTween
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
                if (dotweens.Any())
                {
                    AssetDatabase.ImportPackage(
                        "Packages/com.bsning.ntool/DoTween.Moudle.unitypackage",
                        true
                    );
                    dotweens.Select(t => t.GetName().Name).ForEach(Debug.Log);
                }
            }
        }
    }
}
