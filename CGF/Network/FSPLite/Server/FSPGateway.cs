using CGF.Network.General.Server;
using CGF.Utils;

namespace CGF.Network.FSPLite.Server
{
    public class FSPGateway : Gateway
    {
        public int Port
        {
            get { return m_port; }
        }

        public string Host
        {
            get { return NetworkUtils.SelfIP; }
        }

        protected override void OnRecive(uint sid, byte[] bytes, int len, object arg)
        {
            if (len > 0)
            {
                lock (m_sessionMap)
                {
                    ISession session = GetSession(sid);

                    if (session != null)
                    {
                        Debuger.Log("Recive session data sid = " + sid);
                        session.Active(arg);
                        session.DoReciveInGateway(bytes, len);
                    }
                }
            }
        }
    }
}