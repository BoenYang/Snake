using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CGF.Network.Client
{
    public class NetManager
    {
        private IConnection m_connection;

        public NetManager()
        {

        }

        public void Init(string ip, int port)
        {
            m_connection = new TCPConnection();
        }
    }
}
