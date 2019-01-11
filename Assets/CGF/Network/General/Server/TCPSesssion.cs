using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CGF.Network.General.Server
{
    class TCPSesssion : ISession
    {
        public uint sid { get; private set; }
        public uint uid { get; private set; }

        private Socket m_clientSocket;

        public TCPSesssion(uint sid, Socket client)
        {
            m_clientSocket = client;
            this.sid = sid;
        }

        public void Active(object arg)
        {
            if (m_clientSocket != null)
            {
                m_clientSocket.Shutdown(SocketShutdown.Both);
            }
            m_clientSocket = arg as Socket;
        }

        public bool IsActive()
        {
            return m_clientSocket != null;
        }

        public void Send(byte[] bytes, int len)
        {
            m_clientSocket.Send(bytes);
        }
       

        public void DoReciveInGateway(byte[] bytes, int len)
        {

        }

        public void Tick()
        {

        }
    }
}
