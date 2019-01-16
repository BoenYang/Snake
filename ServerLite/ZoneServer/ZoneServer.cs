using CGF.Network.General.Server;
using CGF.Server;

namespace Snake.Server.ZoneServer
{
    public class ZoneServer : ServerModule
    {
        private NetManager m_net;

        public override void Start()
        {
            m_net = new NetManager();
            m_net.Init(m_info.port);
        }

        public override void Stop()
        {

            if (m_net != null)
            {
                m_net.Clean();
                m_net = null;
            }

            base.Stop();
        }
    }
}