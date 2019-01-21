using System;
using System.Collections.Generic;
using CGF.Network.General.Server;
using SGF.Codec;

namespace CGF.Network.FSPLite.Server
{
    public class FSPPlayer : ISessionListener
    {

        public uint uid;

        private ISession m_session;

        private Action<FSPPlayer, FSPMessage> m_msgListener;

        private Queue<FSPFrame> m_frameCache = new Queue<FSPFrame>();

        private FSPDataS2C temp_SendData = new FSPDataS2C();

        private byte[] m_sendBuffer = new byte[4096];

        public void Create(uint uid, ISession session, Action<FSPPlayer,FSPMessage> listener)
        {
            this.uid = uid;
            m_session = session;
            m_session.SetReceiveListener(this);
            m_msgListener = listener;
        }

        public void Release()
        {
            m_session.SetReceiveListener(null);
            m_frameCache.Clear();

        }

        public void OnReceive(ISession session, byte[] bytes, int len)
        {
            if (len > 0)
            {
                FSPDataC2S data = PBSerializer.NDeserialize<FSPDataC2S>(bytes);
                for (int i = 0; i < data.msgs.Count; i++)
                {
                    if (m_msgListener != null)
                    {
                        m_msgListener(this, data.msgs[i]);
                    }
                }
            }
        }

        public void SendToClient(FSPFrame frame)
        {
            if (frame != null)
            {
                m_frameCache.Enqueue(frame);
            }

            while (m_frameCache.Count > 0)
            {
                if (SendInternal(m_frameCache.Peek()))
                {
                    m_frameCache.Dequeue();
                }
            }
        }

        private bool SendInternal(FSPFrame frame)
        {
            temp_SendData.frames.Clear();
            temp_SendData.frames.Add(frame);

            int len = PBSerializer.NSerialize(temp_SendData, m_sendBuffer);
            return m_session.Send(m_sendBuffer,len);
        }
    }
}