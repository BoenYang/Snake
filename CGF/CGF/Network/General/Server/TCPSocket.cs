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

        public ISession CreateSession(ISessionListener listener,uint sid , object args)
        {
            return new TCPSesssion(sid, args as Socket, listener);
        }

        private void OnAccept(IAsyncResult result)
        {
            Socket client = m_Socket.EndAccept(result);
            Debuger.Log("Accept a client " + client.RemoteEndPoint);
            m_Socket.BeginAccept(OnAccept, m_Socket);
            client.BeginReceive(m_recvBuf, 0, m_recvBuf.Length, SocketFlags.None, OnReceivePacket, client);
        }


        private void OnReceivePacket(IAsyncResult result)
        {
            Socket client = result.AsyncState as Socket;
            int len = client.EndReceive(result);
            if (len > 0)
            {
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
            byte[] temp = new byte[8];
            m_recvNetBuffer.ReadBytes(temp, 0, 8);
            uint sid = BitConverter.ToUInt32(temp, 0);
            uint packageSize = BitConverter.ToUInt32(temp, 4);
            while (m_recvNetBuffer.Length > packageSize)
            {
                byte[] package = new byte[packageSize - 8];
                m_recvNetBuffer.ReadBytes(package, 0, (int)packageSize - 8);
                m_recvNetBuffer.Arrangement();
                OnReceive(sid, package,(int)(packageSize - 8), client);
                ReadPackage(client);
            }
        }

    }
}