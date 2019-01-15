using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using CGF.Network.Core;
using SGF.Network.Core;

namespace CGF.Network.General.Server
{
    class TCPSesssion : ISession
    {
        public uint sid { get; private set; }
        public uint uid { get; private set; }

        private Socket m_clientSocket;

        private ISessionListener m_listener;

        private SwitchQueue<byte[]> m_RecvBufQueue = new SwitchQueue<byte[]>();

        public TCPSesssion(uint sid, Socket client, ISessionListener listener)
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
            byte[] packageHead = new byte[8 + len];
            NetBuffer buffer = new NetBuffer();
            buffer.Attach(packageHead, packageHead.Length);
            buffer.WriteUInt(sid);
            buffer.WriteUInt((uint)(8 + len));
            buffer.WriteBytes(bytes);
            m_clientSocket.Send(buffer.GetBytes());
        }


        public void DoReciveInGateway(byte[] bytes, int len)
        {
            byte[] data = new byte[len];
            Buffer.BlockCopy(bytes, 0, data, 0, len);
            m_RecvBufQueue.Push(data);
        }

        private void DoReciveInMain()
        {
            m_RecvBufQueue.Switch();

            while (!m_RecvBufQueue.Empty())
            {
                byte[] recvBufferRaw = m_RecvBufQueue.Pop();
                m_listener.OnReceive(this, recvBufferRaw, recvBufferRaw.Length);

            }
        }

        public void Tick()
        {
            DoReciveInMain();
        }
    }
}
