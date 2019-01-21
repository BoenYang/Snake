using System;
using System.Collections.Generic;
using CGF.Common;

namespace CGF.Server
{
    public class ServerManager : Singleton<ServerManager>
    {

        private Dictionary<int,ServerModule> m_mapModule = new Dictionary<int, ServerModule>();

        public void Init(string _namespace)
        {
            ServerConfig.NameSpace = _namespace;
        }

        public void StarServer(int id)
        {
            Debuger.Log(id);

            ServerModuleInfo moduleInfo = ServerConfig.GetServerModuleInfo(id);
            string fullName = ServerConfig.NameSpace + "." + moduleInfo.name + "." + moduleInfo.name;
            Debuger.Log(fullName + "," + moduleInfo.assembly);

            try
            {
                Type type = Type.GetType(fullName + "," + moduleInfo.assembly);
                ServerModule module = Activator.CreateInstance(type) as ServerModule;
                if (module != null)
                {
                    module.Create(moduleInfo);
                    m_mapModule.Add(id,module);
                    module.Start();
                }
            }
            catch (Exception e)
            {
                Debuger.LogError(e.Message);
            }
        }

        public void StopServer(int id)
        {
            Debuger.Log(id);
            ServerModule module = m_mapModule[id];
            if (module != null)
            {
                module.Stop();
                module.Release();
                m_mapModule.Remove(id);
            }
        }

        public void StopAllServer()
        {
            Debuger.Log();
            foreach (ServerModule server in m_mapModule.Values)
            {
                server.Stop();
                server.Release();
            }
            m_mapModule.Clear();
        }

        public void Tick()
        {
            foreach (ServerModule server in m_mapModule.Values)
            {
                server.Tick();
            }
        }
    }
}