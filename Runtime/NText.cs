using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NTool
{
    [Serializable]
    public struct NText
    {
        [SerializeField] internal Text text;
        [SerializeField] internal TMP_Text tmpText;

        public string Text
        {
            get =>
                text
                    ? text.text
                    : tmpText
                        ? tmpText.text
                        : null;
            set
            {
                if (text)
                    text.text = value;
                if (tmpText)
                    tmpText.text = value;
            }
        }

        public Graphic Graphic => text ? text : tmpText ? tmpText : null;
    }
}