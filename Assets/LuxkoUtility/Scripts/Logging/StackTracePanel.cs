using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Luxko.Logging
{
    public class StackTracePanel : MonoBehaviour
    {
        public RectTransform holderTransform;
        public GameObject StackTraceItemTemplate;
        List<StackTraceItem> Children = new List<StackTraceItem>();
        LogItem currentItem;

        public void ShowStackTrace(LogItem item)
        {
            if (Object.ReferenceEquals(item, currentItem)) return;
            currentItem = item;
            var trace = item.stackTrace;
            while (Children.Count < trace.Length)
            {
                AddNewChildren();
            }

            for (int i = trace.Length; i < Children.Count; ++i)
            {
                Children[i].gameObject.SetActive(false);
            }

            var holderWidth = holderTransform.sizeDelta.x;

            for (int i = 0; i < trace.Length; ++i)
            {
                Children[i].gameObject.SetActive(true);
                Children[i].DisplayText(trace[i], holderWidth);
            }
        }

        void AddNewChildren()
        {
            var child = Instantiate(StackTraceItemTemplate, transform).GetComponent<StackTraceItem>();
            Children.Add(child);
        }
    }
}
