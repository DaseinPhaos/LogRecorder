using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Luxko.Logging
{
    public class StackTraceItem : MonoBehaviour
    {
        public Text text;
        public LayoutElement layoutElement;
        public float HeightMargin = 2;
        public float WidthMargin = 2;

        public void DisplayText(string t, float preferredWidth)
        {
            layoutElement.preferredWidth = preferredWidth - WidthMargin;
            text.text = t;
            StartCoroutine(AdjustHeight());
        }

        IEnumerator AdjustHeight()
        {
            yield return null;
            yield return null;
            layoutElement.preferredHeight = text.preferredHeight + HeightMargin;
        }
    }
}
