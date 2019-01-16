using CGF.Common;
using CGF.Network.General.Client;

namespace Snake.Services
{
    public class OnlineManager : Singleton<OnlineManager>
    {
        private NetManager m_net;

        public void Init()
        {
            m_net = new NetManager();
            m_net.Init(typeof(TCPConnection));
            m_net.Connect("127.0.0.1",4540);
        }
    }
}