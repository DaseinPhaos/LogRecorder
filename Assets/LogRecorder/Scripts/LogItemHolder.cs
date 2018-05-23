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
        public float WidthMargin = 2;
        float initialMsgHeight;
        float initialPreferredHeight;
        LayoutElement layout;

        void Awake()
        {
            initialMsgHeight = messageDisplay.preferredHeight;

            layout = GetComponent<LayoutElement>();
            initialPreferredHeight = layout.preferredHeight;
        }

        public void SetPreferredWidth(float width)
        {
            layout.preferredWidth = width - WidthMargin;
        }

        public void SetColors(Color bg, Color fg)
        {
            messageDisplay.color = fg;
            GetComponent<Image>().color = bg;
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

        public LogItem(string msg, string st, LogType t, bool logTime)
        {
            if (logTime)
            {
                var ts = System.TimeSpan.FromSeconds(Time.time);
                message = string.Format("[{0}:{1:D2}.{2:D3}] {3}", ts.Minutes + ts.Hours * 60, ts.Seconds, ts.Milliseconds, msg);
            }
            else
            {
                message = msg;
            }

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
