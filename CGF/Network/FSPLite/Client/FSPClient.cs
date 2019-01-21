using System;
using System.Runtime.InteropServices;
using CGF.Network.General.Client;
using SGF.Codec;

namespace CGF.Network.FSPLite.Client
{
    public class FSPClient : TCPConnection
    {
        private Action<FSPFrame> m_RecvListener;

        private FSPDataC2S m_tempDataToServer = new FSPDataC2S();

        private byte[] m_sendBufferTemp = new byte[4096];

        public void Init(uint sid)
        {
            Id = sid;
            m_tempDataToServer.msgs.Add(new FSPMessage());
            base.Init();
        }

        public void SetFSPListener(Action<FSPFrame> listener)
        {
            m_RecvListener = listener;
        }

        public bool SendFSP(int clientFrameId,int cmd, int[] args)
        {
            FSPMessage msg = m_tempDataToServer.msgs[0];
            msg.clientFrameId = clientFrameId;
            msg.cmd = cmd;
            msg.args = args;

            m_tempDataToServer.msgs.Clear();
            m_tempDataToServer.msgs.Add(msg);

            int len = PBSerializer.NSerialize(m_tempDataToServer, m_sendBufferTemp);
            Send(m_sendBufferTemp, len);
            return len > 0;
        }

        protected override void DoReceiveInMain()
        {
            m_recvQueue.Switch();
            while (!m_recvQueue.Empty())
            {
                byte[] buffer = m_recvQueue.Pop();
                if (OnRecive != null)
                {
                    FSPDataS2C data = PBSerializer.NDeserialize<FSPDataS2C>(buffer);
                    for (int i = 0; i < data.frames.Count; i++)
                    {
                        m_RecvListener(data.frames[i]);
                    }
                }
            }
        }
    }
}