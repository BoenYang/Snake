using System.Collections.Generic;

namespace CGF.Core
{

    public class ModuleEvent
    {

    }

    public class ModuleEvent<T>
    {

    }

    public class EventTable
    {
        private Dictionary<string, ModuleEvent> m_eventMap; 

        public ModuleEvent GetEvent(string type)
        {
            if (m_eventMap == null)
            {
                m_eventMap = new Dictionary<string, ModuleEvent>();
            }

            if (!m_eventMap.ContainsKey(type))
            {
                m_eventMap.Add(type,new ModuleEvent());
            }

            return m_eventMap[type];
        }


        public void Clear()
        {
            m_eventMap.Clear();
            m_eventMap = null;
        }
    }
}
