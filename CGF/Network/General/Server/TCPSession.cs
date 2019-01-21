using System;
using System.Net.Sockets;
using CGF.Network.Core;
using SGF.Network.Core;

namespace CGF.Network.General.Server
{
    public class TCPSession : ISession
    {
        public uint sid { get; private set; }
        public uint uid { get; private set; }

        private Socket m_clientSocket;

        private ISessionListener m_listener;

        private SwitchQueue<byte[]> m_RecvBufQueue = new SwitchQueue<byte[]>();

        public TCPSession(uint sid,ISessionListener listener)
        {
            m_listener = listener;
            this.sid = sid;
        }

        public void Active(object arg)
        {
            m_clientSocket = arg as Socket;
        }

        public bool IsActive()
        {
            return m_clientSocket != null;
        }

        public bool Send(byte[] bytes, int len)
        {
            if (m_clientSocket == null)
            {
                return false;
            }

            Buffer.BlockCopy(bytes, 0, bytes, 8, len);
            NetBuffer buffer = new NetBuffer();
            buffer.Attach(bytes, 8 + len);
            buffer.WriteUInt(sid, 0);
            buffer.WriteUInt((uint)(8 + len), 4);
            int ret = m_clientSocket.Send(buffer.GetBytes(),len + 8,SocketFlags.None);
            return ret > 0;
        }

        public void SetReceiveListener(ISessionListener listener)
        {
            this.m_listener = listener;
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
                if (m_listener != null)
                {
                    m_listener.OnReceive(this, recvBufferRaw, recvBufferRaw.Length);
                }
            }
        }

        public void Tick()
        {
            DoReciveInMain();
        }
    }
}
