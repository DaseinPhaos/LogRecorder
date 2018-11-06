using UnityEngine;
using UnityEngine.Events;

namespace Luxko.Logging
{
    [System.Serializable]
    public class DebugCommandProc : UnityEvent<string[]> { }

    [System.Serializable]
    public class DebugCommand
    {
        public string name;
        public DebugCommandProc proc;

        public DebugCommand(string name, DebugCommandProc proc)
        {
            this.name = name;
            this.proc = proc;
        }

        public DebugCommand(UnityAction<string[]> act, string name)
        {
            if (name == null) name = act.Method.Name; // ?
            this.name = name;
            this.proc = new DebugCommandProc();
            this.proc.AddListener(act);
        }

        public void Execute(string[] parameters)
        {
            proc.Invoke(parameters);
        }
        static public DebugCommand From(UnityAction act, string name)
        {
            return new DebugCommand(_ => act(), name);
        }

        static public DebugCommand From<T>(UnityAction<T> act, string name)
        where T : System.IConvertible
        {
            return new DebugCommand(parameters =>
            {
                if (parameters.Length != 2) Debug.LogException(new System.ArgumentException("Invalid number of parameters, expect 1 argument"));
                else
                {
                    try
                    {
                        act((T)System.Convert.ChangeType(parameters[1], typeof(T)));
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }, name);
        }

        static public DebugCommand From<T1, T2>(UnityAction<T1, T2> act, string name)
        where T1 : System.IConvertible
        where T2 : System.IConvertible
        {
            return new DebugCommand(parameters =>
            {
                if (parameters.Length != 3) Debug.LogException(new System.ArgumentException("Invalid number of parameters, expect 2 argument"));
                else
                {
                    try
                    {
                        act(
                            (T1)System.Convert.ChangeType(parameters[1], typeof(T1)),
                            (T2)System.Convert.ChangeType(parameters[2], typeof(T2))
                        );
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }, name);
        }
    }
}
