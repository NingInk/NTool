using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NTool
{

    [Serializable]
    public struct NText
    {
        public Text Text;
        public TMP_Text TMPText;

        public string text
        {
            get =>
                Text
                    ? Text.text
                    : TMPText
                        ? TMPText.text
                        : null;
            set
            {
                if (Text)
                    Text.text = value;
                if (TMPText)
                    TMPText.text = value;
            }
        }

        public Graphic Graphic => Text ? Text : TMPText ? TMPText : null;
    }
}