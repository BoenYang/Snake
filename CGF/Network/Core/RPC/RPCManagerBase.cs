using System.Collections.Generic;
using System.Reflection;

namespace CGF.Network.Core.RPC
{
    public class RPCMethodHelper
    {
        public object listener;
        public MethodInfo method;
    }

    public class RPCManagerBase
    {
        private List<object> m_listeners;

        private Dictionary<string,RPCMethodHelper> m_methodMap = new Dictionary<string, RPCMethodHelper>();

        public void Init()
        {
            m_listeners = new List<object>();
        }

        public void RegisterListener(object listener)
        {
            if (!m_listeners.Contains(listener))
            {
                m_listeners.Add(listener);
            }
        }

        public void UnregisterListener(object listener)
        {
            if (m_listeners.Contains(listener))
            {
                m_listeners.Remove(listener);
            }
        }

        public RPCMethodHelper GetRPCMethodHelper(string methodName)
        {
            if (m_methodMap.ContainsKey(methodName))
            {
                return m_methodMap[methodName];
            }
            else
            {
                MethodInfo mi = null;
                object listener = null;
                for (int i = 0; i < m_listeners.Count; i++)
                {
                    listener  = m_listeners[i];
                    mi = listener.GetType().GetMethod(methodName,
                        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    if (mi != null)
                    {
                        break;
                    }
                }

                if (mi != null)
                {
                    RPCMethodHelper helper = new RPCMethodHelper();
                    helper.listener = listener;
                    helper.method = mi;
                    m_methodMap.Add(methodName, helper);
                    return helper;
                }

                return null;
            }
        }

        public void Clean()
        {
            m_listeners.Clear();
            m_listeners = null;
        }
    }
}