using System.Collections.Generic;

namespace CGF.Network.General.Server
{

    public class Gateway
    {
        protected int m_port;

        protected Dictionary<uint, ISession> m_sessionMap;

        protected ISocket m_systemSocket;

        protected ISessionListener m_sessionListener;

        public void Init(int port,ISessionListener listener)
        {
            m_port = port;
            m_sessionListener = listener;
            m_sessionMap = new Dictionary<uint, ISession>();
            Start();
        }

        public virtual void Start()
        {
            m_systemSocket = new TCPSocket();
            m_systemSocket.Start(m_port);
            m_systemSocket.OnReceive = OnRecive;
        }

        protected virtual void OnRecive(uint sid, byte[] bytes,int len , object arg)
        {
            if (len > 0)
            {
                lock (m_sessionMap)
                {
                    ISession session = GetSession(sid);

                    if (sid == 0)
                    {
                        session = CreateSession();
                        m_sessionMap.Add(session.sid, session);
                        Debuger.Log("Create a Session sid = " + session.sid);
                    }

                    if (session != null)
                    {

                        Debuger.Log("Recive session data sid = " + sid);
                        session.Active(arg);
                        session.DoReciveInGateway(bytes, len);
                    }
                }
            }
        }

        public ISession CreateSession()
        {
            uint newSid = SessionID.NewID();
            return m_systemSocket.CreateSession(m_sessionListener, newSid, m_systemSocket);
        }

        public ISession GetSession(uint sid)
        {
            if (m_sessionMap.ContainsKey(sid))
            {
                return m_sessionMap[sid];
            }

            return null;
        }

        public void Tick()
        {
            foreach (ISession session in m_sessionMap.Values)
            {
                if (session.IsActive())
                {
                    session.Tick();
                }
            }
        }

        private void ClearNotActiveSession()
        {

        }

        public void Stop()
        {
            if (m_systemSocket != null)
            {
                m_systemSocket.ShutDown();
                m_systemSocket = null;
            }
        }

        public void Clean()
        {
            this.m_sessionMap.Clear();
            this.m_sessionMap = null;
        }
    }
}



