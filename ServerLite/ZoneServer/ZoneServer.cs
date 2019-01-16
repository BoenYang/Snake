using CGF.Network.General.Server;
using CGF.Server;
using Snake.Server.ZoneServer.Online;

namespace Snake.Server.ZoneServer
{
    public class ZoneServer : ServerModule
    {
        private NetManager m_net;

        private ServerContext context;

        public override void Start()
        {
            m_net = new NetManager();
            m_net.Init(m_info.port);

            context = new ServerContext();
            context.net = m_net;

            OnlineManager.Instance.Init(context);
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

        public override void Tick()
        {
            base.Tick();
            m_net.Tick();
        }
    }
}