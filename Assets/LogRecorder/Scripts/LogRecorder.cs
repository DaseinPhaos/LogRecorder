using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luxko.Logging
{
    public class LogRecorder : MonoBehaviour
    {
        public RectTransform HolderTransform;
        public Transform ItemsPanel;
        public StackTracePanel stackTracePanel;
        public GameObject LogItemTemplate;

        [Space(20)]
        public Color InfoBg = new Color(1, 1, 1, 0.145f);
        public Color InfoFg = new Color(0.19f, 0.19f, 0.19f, 1f);
        [Space(10)]
        public Color WarningBg = new Color(1, 1, 0, 0.145f);
        public Color WarningFg = new Color(0.19f, 0.19f, 0.19f, 1f);
        [Space(10)]
        public Color ErrorBg = new Color(1, 0, 0, 0.145f);
        public Color ErrorFg = new Color(0.19f, 0.19f, 0.19f, 1f);


        Queue<LogItem> logs = new Queue<LogItem>();

        void OnEnable()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        void OnLogMessageReceived(string msg, string st, LogType t)
        {
            var log = new LogItem(msg, st, t);
            logs.Enqueue(log);
            // TODO: handle maximum queued
            // TODO: construct (or reuse) LogItemHolder from template
            var holder = Instantiate(LogItemTemplate, ItemsPanel).GetComponent<LogItemHolder>();
            holder.item = log;
            switch (t)
            {
                case LogType.Log:
                    holder.SetColors(InfoBg, InfoFg);
                    break;
                case LogType.Warning:
                    holder.SetColors(WarningBg, WarningFg);
                    break;
                default:
                    holder.SetColors(ErrorBg, ErrorFg);
                    break;
            }
            float holderWidth = -1;
            if (HolderTransform != null)
            {
                holderWidth = HolderTransform.sizeDelta.x;
            }
            StartCoroutine(DisplayMessageNextFrame(holder, holderWidth));
            holder.stackTracePanel = stackTracePanel;
        }

        IEnumerator DisplayMessageNextFrame(LogItemHolder holder, float holderWidth)
        {
            if (holderWidth > 0) holder.SetPreferredWidth(holderWidth);
            yield return null;
            yield return null;
            holder.DisplayMessage();
        }
    }
}

