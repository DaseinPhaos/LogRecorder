using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Luxko.Logging
{
    public class LogItemHolder : MonoBehaviour
    {
        public LogItem item;
        public Text messageDisplay;
        public StackTracePanel stackTracePanel;
        float initialMsgHeight;
        float initialPreferredHeight;
        LayoutElement layout;

        void Awake()
        {
            initialMsgHeight = messageDisplay.preferredHeight;

            layout = GetComponent<LayoutElement>();
            initialPreferredHeight = layout.preferredHeight;
        }

        [ContextMenu("Display Message")]
        public void DisplayMessage()
        {
            messageDisplay.text = item.ToString();
            UpdatePrefferedHeight();
        }

        void UpdatePrefferedHeight()
        {
            layout.preferredHeight = messageDisplay.preferredHeight + initialPreferredHeight - initialMsgHeight;
        }

        [ContextMenu("Display StackTrace")]
        public void DisplayStackTrace()
        {
            if (stackTracePanel != null)
            {
                stackTracePanel.ShowStackTrace(item);
            }
        }
    }

    [System.Serializable]
    public class LogItem
    {
        public string message;
        public string[] stackTrace;
        public LogType type;

        public LogItem(string msg, string st, LogType t)
        {
            message = msg;

            // stackTrace = st.TrimEnd().Replace("\n", "\n\nat ");
            stackTrace = st.Trim().Split('\n');
            type = t;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", type, message);
        }
    }
}
