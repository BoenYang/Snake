using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CGF.Network.General;
using UnityEngine;

namespace CGF.Network.General.Server
{

    public class Gateway
    {
        private Dictionary<uint, ISession> m_sessionMap;

        private int m_port;

        private ISocket m_systemSocket;


        private ISessionListener m_sessionListener;

        public void Init(int port,ISessionListener listener)
        {
            m_port = port;
            m_sessionListener = listener;
            m_sessionMap = new Dictionary<uint, ISession>();
            Start();
        }

        public void Start()
        {
            m_systemSocket = new TCPSocket();
            m_systemSocket.Start(m_port);
            m_systemSocket.OnReceive = OnRecive;
        }

        private void OnRecive(uint sid, byte[] bytes,int len , object arg)
        {
            if (len > 0)
            {
            lock (m_sessionMap)
                {
                    ISession session = null;

                    if (sid == 0)
                    {
                        uint newSid = SessionID.NewID();
                        session = m_systemSocket.CreateSession(m_sessionListener,newSid, arg);
                        m_sessionMap.Add(newSid, session);
                    }
                    else
                    {
                        session.DoReciveInGateway(bytes,len);
                    }

                    if (session != null)
                    {
                        session.Active(arg);
                    }
                }
            }
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



