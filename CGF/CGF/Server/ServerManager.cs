using System.Collections.Generic;
using CGF.Common;

namespace CGF.CGF.Server
{
    public class ServerManager : Singleton<ServerManager>
    {

        private Dictionary<int,ServerModule> m_mapModule = new Dictionary<int, ServerModule>();

        public void Init(string _namespace)
        {
            
        }

        public void StarServer(int id)
        {

        }

        public void StopServer(int id)
        {

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