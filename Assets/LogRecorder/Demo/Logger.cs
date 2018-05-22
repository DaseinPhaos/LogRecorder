using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luxko.Logging
{
    public class Logger : MonoBehaviour
    {
        public string logMessage;
        public LogType logType;
        [ContextMenu("Log")]
        public void Log()
        {
            switch (logType)
            {
                case LogType.Assert:
                    Debug.LogAssertion(logMessage);
                    break;
                case LogType.Error:
                    Debug.LogError(logMessage);
                    break;
                case LogType.Exception:
                    Debug.LogException(new System.Exception(logMessage));
                    break;
                case LogType.Log:
                    Debug.Log(logMessage);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(logMessage);
                    break;
            }
        }

        [ContextMenu("Log Deep")]
        public void LogDeep()
        {
            Log();
        }
    }
}
