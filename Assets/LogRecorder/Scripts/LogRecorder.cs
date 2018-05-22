using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luxko.Logging
{
    public class LogRecorder : MonoBehaviour
    {
        public Transform ItemsPanel;
        public StackTracePanel stackTracePanel;
        public GameObject LogItemTemplate;

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
            StartCoroutine(DisplayMessageNextFrame(holder));
            holder.stackTracePanel = stackTracePanel;
        }

        IEnumerator DisplayMessageNextFrame(LogItemHolder holder)
        {
            yield return null;
            yield return null;
            holder.DisplayMessage();
        }
    }
}

