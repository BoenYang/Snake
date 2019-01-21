using System;
using System.Net;
using System.Net.Sockets;
using CGF.Network.Core;

namespace CGF.Network.General.Server
{
    public class TCPSocket : ISocket
    {
        public Action<uint,byte[], int, object> OnReceive { get; set; }

        private Socket m_Socket;

        private byte[] m_recvBuf = new byte[4096];

        private NetBuffer m_recvNetBuffer;

        public void Start(int port)
        {
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_Socket.Bind(new IPEndPoint(IPAddress.Any, port));
            m_Socket.Listen(port);
            Debuger.Log("Listener at port " + port);
            m_Socket.BeginAccept(OnAccept, m_Socket);

            m_recvNetBuffer = new NetBuffer();
        }

        public void ShutDown()
        {
            m_Socket.Shutdown(SocketShutdown.Both);
        }

        public virtual ISession CreateSession(ISessionListener listener,uint sid, object arg)
        {
            return new TCPSession(sid,listener);
        }

        private void OnAccept(IAsyncResult result)
        {
            Socket client = m_Socket.EndAccept(result);
            Debuger.Log("Accept a client " + client.RemoteEndPoint);
            client.BeginReceive(m_recvBuf, 0, m_recvBuf.Length, SocketFlags.None, OnReceivePacket, client);
            m_Socket.BeginAccept(OnAccept, m_Socket);
        }


        private void OnReceivePacket(IAsyncResult result)
        {
            Socket client = result.AsyncState as Socket;
            int len = client.EndReceive(result);
            if (len > 0)
            {
                Debuger.Log("Recive Data len = " + len);
                client.BeginReceive(m_recvBuf, 0, m_recvBuf.Length, SocketFlags.None, OnReceivePacket, client);
                lock (m_recvNetBuffer)
                {
                    if (m_recvNetBuffer.Length == 0)
                    {
                        m_recvNetBuffer.Attach(m_recvBuf, len);
                    }
                    else
                    {
                        if (m_recvNetBuffer.Length + len > m_recvNetBuffer.Capacity)
                        {
                            m_recvNetBuffer.AdjustCapacity(m_recvNetBuffer.Length + len);
                        }
                        m_recvNetBuffer.AddBytes(m_recvBuf, m_recvNetBuffer.Length - 1, len);
                    }
                    ReadPackage(client);
                }
              
            }
         
        }

        private void ReadPackage(Socket client)
        {
            uint sid = m_recvNetBuffer.ReadUInt();
            uint packageSize = m_recvNetBuffer.ReadUInt();
            while (m_recvNetBuffer.Length >= packageSize)
            {
                long restBufferLen = m_recvNetBuffer.Length - packageSize;
                bool continueRead = restBufferLen > 0;
                byte[] package = new byte[packageSize - 8];
                m_recvNetBuffer.ReadBytes(package, 0, (int)packageSize - 8);
                OnReceive(sid, package, (int)(packageSize - 8), client);

                if (continueRead)
                {
                    m_recvNetBuffer.CopyWith(m_recvNetBuffer, 0,true);
                    ReadPackage(client);
                }
                else
                {
                    m_recvNetBuffer.Clear();
                }
            }
        }

    }
}