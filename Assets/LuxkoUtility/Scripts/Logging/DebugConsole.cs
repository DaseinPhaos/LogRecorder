using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Luxko.Logging
{
    public class DebugConsole : MonoBehaviour
    {
        public List<DebugCommand> _commands = new List<DebugCommand>();

        #region Public Methods
        public void RunCommand(string commandStr)
        {
            Debug.LogFormat("Running \"{0}\"", commandStr);
            var parameters = commandStr.Split(' ');
            var cmdName = parameters[0];
            var found = false;
            foreach (var command in _commands)
            {
                if (command.name == cmdName)
                {
                    command.Execute(parameters);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Debug.LogErrorFormat("Command \"{0}\" not found!", cmdName);
            }
        }

        public bool AddCommand(string name, UnityAction cmd)
        {
            if (name == null) return false;
            if (_commands.Find(c => c.name == name) != null) return false;
            _commands.Add(DebugCommand.From(cmd, name));
            return true;
        }

        public bool AddCommand<T>(string name, UnityAction<T> cmd)
        where T : System.IConvertible
        {
            if (name == null) return false;
            if (_commands.Find(c => c.name == name) != null) return false;
            _commands.Add(DebugCommand.From<T>(cmd, name));
            return true;
        }

        public bool AddCommand<T1, T2>(string name, UnityAction<T1, T2> cmd)
        where T1 : System.IConvertible
        where T2 : System.IConvertible
        {
            if (name == null) return false;
            if (_commands.Find(c => c.name == name) != null) return false;
            _commands.Add(DebugCommand.From<T1, T2>(cmd, name));
            return true;
        }
        #endregion

        static DebugConsole _instance;
        public static DebugConsole Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DebugConsole>();
                    if (_instance == null)
                    {
                        _instance = new GameObject("DebugConsole", typeof(DebugConsole)).GetComponent<DebugConsole>();
                    }
                }
                return _instance;
            }
        }

        // TODO: command history, on enter text buffer cleanup, common commands on awake, etc. etc.
    }
}
