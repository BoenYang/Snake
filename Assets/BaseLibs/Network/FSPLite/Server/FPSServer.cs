using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FPSServer
{

    private TCPSocket m_Socket;

    private long FRAME_TICK_INTERVAL = 666666;

    private long m_LastTicks;

    private Thread m_ThreadMain;

    private bool m_IsRunning = false;

    private FSPGame m_Game;

    private Dictionary<TCPSocket,FSPSession> m_sessions = new Dictionary<TCPSocket,FSPSession>(); 

    public bool Start(int port)
    {
        m_Socket = new TCPSocket();
        m_Socket.Bind(port);
        m_Socket.AddClientConnectedListener(OnClientConnect);
        m_Socket.AddRecivedListener(OnClientRecive);
        m_Socket.AddClientDisconnectListener(OnClientDisconnect);


        m_IsRunning = true;
        m_ThreadMain = new Thread(Thead_Main) { IsBackground = true};
        m_ThreadMain.Start();

        return true;
    }

    private void OnClientConnect(TCPSocket socket)
    {
        FSPSession session = new FSPSession(socket);
        m_sessions.Add(socket,session);
    }

    private void OnClientRecive(TCPSocket socket, byte[] data)
    {
        if (m_sessions.ContainsKey(socket))
        {
            m_sessions[socket].OnRecive(data);
        }
    }

    private void OnClientDisconnect(TCPSocket socket)
    {
        if (m_sessions.ContainsKey(socket))
        {
            m_sessions.Remove(socket);
        }
    }

    private void Thead_Main()
    {
        while (m_IsRunning)
        {
            HandleRecive();
            DoMainLoop();
            Thread.Sleep(10);
        }
    }

    private void HandleRecive()
    {

    }

    private void DoMainLoop()
    {
        long nowticks = DateTime.Now.Ticks;
        long interval = nowticks - m_LastTicks;

        if (interval > FRAME_TICK_INTERVAL)
        {
            m_LastTicks = nowticks;
            if (m_Game != null)
            {
                m_Game.EnterFrame();
            }
        }
    }

}
