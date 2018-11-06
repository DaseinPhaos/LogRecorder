using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luxko.Collections;

namespace Luxko.Logging
{
    public class LogRecorder : MonoBehaviour
    {
        public RectTransform HolderTransform;
        public Transform ItemsPanel;
        public StackTracePanel stackTracePanel;
        public GameObject LogItemTemplate;
        public bool LogTimeInMessage = true;

        public int MaximumLogCount = 32;
        public int MaximumLogHolderCount = 32;

        [Space(20)]
        public Color InfoBg = new Color(1, 1, 1, 0.145f);
        public Color InfoFg = new Color(0.19f, 0.19f, 0.19f, 1f);
        [Space(10)]
        public Color WarningBg = new Color(1, 1, 0, 0.145f);
        public Color WarningFg = new Color(0.19f, 0.19f, 0.19f, 1f);
        [Space(10)]
        public Color ErrorBg = new Color(1, 0, 0, 0.145f);
        public Color ErrorFg = new Color(0.19f, 0.19f, 0.19f, 1f);

        [Space(20)]
        public bool LogInfo = true;
        public bool LogWarning = true;
        public bool LogError = true;

        RingBuffer<LogItemHolder> logHolders;
        RingBuffer<LogItem> logItems;
        UnityEngine.UI.ScrollRect _containerScrollRect;

        int holderHeadIndex = 0;

        void OnEnable()
        {
            logHolders = new RingBuffer<LogItemHolder>(MaximumLogHolderCount);
            for (int i = 0; i < MaximumLogHolderCount; ++i)
            {
                var holder = Instantiate(LogItemTemplate, ItemsPanel).GetComponent<LogItemHolder>();
                holder.gameObject.SetActive(false);
                logHolders.PushBack(holder);
            }
            logItems = new RingBuffer<LogItem>(MaximumLogCount);
            Application.logMessageReceived += OnLogMessageReceived;
            if (HolderTransform != null)
            {
                _containerScrollRect = HolderTransform.GetComponent<UnityEngine.UI.ScrollRect>();
            }
        }

        void OnDisable()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        void OnLogMessageReceived(string msg, string st, LogType t)
        {
            var log = new LogItem(msg, st, t, LogTimeInMessage);
            logItems.PushBack(log);
            DisplayLog(log);
        }

        void DisplayLog(LogItem log)
        {
            switch (log.type)
            {
                case LogType.Log: if (!LogInfo) return; break;
                case LogType.Warning: if (!LogWarning) return; break;
                default: if (!LogError) return; break;
            }
            LogItemHolder holder = logHolders[holderHeadIndex];
            holder.gameObject.SetActive(true);
            holder.transform.SetAsLastSibling();
            holderHeadIndex = (holderHeadIndex + 1) % logHolders.Count;
            holder.item = log;
            switch (log.type)
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
            if (_containerScrollRect != null)
            {
                _containerScrollRect.verticalNormalizedPosition = 0f;
            }
        }

        public void ToggleLogInfo() { LogInfo = !LogInfo; RefreshLogDisplay(); }
        public void ToggleLogWarning() { LogWarning = !LogWarning; RefreshLogDisplay(); }
        public void ToggleLogError() { LogError = !LogError; RefreshLogDisplay(); }


        public void ClearAllLogs()
        {
            ClearDisplay();
            logItems.Clear();
        }

        void ClearDisplay()
        {
            foreach (var holder in logHolders)
            {
                holder.gameObject.SetActive(false);
            }
            holderHeadIndex = 0;
        }

        void RefreshLogDisplay()
        {
            ClearDisplay();
            foreach (var item in logItems)
            {
                DisplayLog(item);
            }
        }
    }
}

