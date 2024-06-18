#if UNITY_EDITOR
#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
#else
using UnityEditor;
#endif
using UnityEngine;

namespace NTool.Editor
{
#if UNITY_EDITOR
#if ODIN_INSPECTOR
    internal class NTextDrawer : OdinValueDrawer<NText>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (ValueEntry.SmartValue.text == null && ValueEntry.SmartValue.tmpText == null)
            {
                SirenixEditorGUI.BeginBox(label);
                ValueEntry.Property.Children["Text"].Draw();
                ValueEntry.Property.Children["TMPText"].Draw();
                SirenixEditorGUI.EndBox();
            }
            else if (ValueEntry.SmartValue.Text != null)
            {
                ValueEntry.Property.Children["Text"].Draw(label);
            }
            else
            {
                ValueEntry.Property.Children["TMPText"].Draw(label);
            }
        }
    }
#else
    [CustomPropertyDrawer(typeof(NText))]
    public class NTextDrawer : PropertyDrawer
    {
        // private Color _color = new Color(0, 0, 0, 0.5f);
        private SerializedProperty _text,
            _tmpText;

        private bool _fold;

        public override float GetPropertyHeight(
            SerializedProperty property,
            GUIContent label
        )
        {
            _text ??= property.FindPropertyRelative(nameof(NText.text));
            _tmpText ??= property.FindPropertyRelative(nameof(NText.tmpText));
            if (
                _fold
                && _text?.objectReferenceValue == null
                && _tmpText?.objectReferenceValue == null
            )
                return 58;
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(
            Rect position,
            SerializedProperty property,
            GUIContent label
        )
        {
            _text ??= property.FindPropertyRelative(nameof(NText.text));
            _tmpText ??= property.FindPropertyRelative(nameof(NText.tmpText));
            EditorGUI.BeginProperty(position, label, property);
            {
                var rect = new Rect(position) { height = 18, };
                if (null == _text.objectReferenceValue && null == _tmpText.objectReferenceValue)
                {
                    _fold = EditorGUI.BeginFoldoutHeaderGroup(rect, _fold, label);
                    {
                        if (_fold)
                        {
                            rect = new Rect(rect)
                            {
                                x = rect.x + 20,
                                y = rect.y + 20,
                                width = rect.width - 20,
                            };
                            EditorGUI.PropertyField(
                                rect,
                                _text,
                                new GUIContent("Text")
                            );
                            rect = new Rect(rect) { y = rect.y + 20 };
                            EditorGUI.PropertyField(
                                rect,
                                _tmpText,
                                new GUIContent("TmpText")
                            );
                        }
                    }
                    EditorGUI.EndFoldoutHeaderGroup();
                }
                else if (_text.objectReferenceValue != null)
                {
                    EditorGUI.PropertyField(rect, _text, label);
                }
                else
                {
                    EditorGUI.PropertyField(rect, _tmpText, label);
                }
            }
            EditorGUI.EndProperty();
        }
    }
#endif
#endif
}
#endif