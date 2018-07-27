using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class TCPSocket
{

    public delegate void ClientConnectCallBack(TCPSocket socket);
    public delegate void ReciveCallBack(TCPSocket socket,byte[] data);

    private Socket m_socket;

    private event ClientConnectCallBack m_OnClientConnect;

    private event ReciveCallBack m_OnRecive;

    private byte[] m_recive_buffer = new byte[4096];

    private Dictionary<EndPoint,TCPSocket> m_client_sockets = new Dictionary<EndPoint, TCPSocket>();

    private bool m_isClient = false;

    public TCPSocket(AddressFamily family = AddressFamily.InterNetwork)
    {
        m_socket = new Socket(family, SocketType.Stream, ProtocolType.Tcp);
    }

    private TCPSocket(Socket socket)
    {
        m_socket = socket;
    }

    public void Bind(int port)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
        m_socket.Bind(endPoint);
        m_socket.Listen(int.MaxValue);
        m_socket.BeginAccept(this.OnClientConnect, null);
        Debug.Log("ser start");
    }

    public void Connect(IPEndPoint remotePoint)
    {
        m_isClient = true;
        m_socket.Connect(remotePoint);
        m_socket.BeginReceive(m_recive_buffer, 0, m_recive_buffer.Length, SocketFlags.None, this.OnRecived, m_socket);
    }


    public void Send(byte[] data)
    {
        m_socket.Send(data);
    }


    public void Close()
    {
        m_socket.Shutdown(SocketShutdown.Both);

        m_socket.Close();
    }

    public void AddRecivedListener(ReciveCallBack listener)
    {
        this.m_OnRecive += listener;
    }

    public void AddClientConnectedListener(ClientConnectCallBack listener)
    {
        this.m_OnClientConnect += listener;
    }

    public void AddClientDisconnectListener()
    {

    }

    private void OnClientConnect(IAsyncResult result)
    {
        Socket client = m_socket.EndAccept(result);

        client.BeginReceive(m_recive_buffer, 0, m_recive_buffer.Length, SocketFlags.None, this.OnRecived, client);

        TCPSocket socket = new TCPSocket(client);

        m_client_sockets.Add(client.RemoteEndPoint,socket);

        if (m_OnClientConnect != null)
        {
            m_OnClientConnect(socket);
        }
        Debug.Log("client connected");
    }

    private void OnRecived(IAsyncResult result)
    {
        Socket client = result.AsyncState as Socket;
        int len = client.EndReceive(result);

   

        byte[] dst = new byte[len];
        Array.Copy(m_recive_buffer,dst,dst.Length);

        if (!this.m_isClient)
        {
            if (len == 0)
            {
                Debug.Log("client disconnect");
                return;
            }

            if (m_client_sockets.ContainsKey(client.RemoteEndPoint))
            {
                TCPSocket socket = m_client_sockets[client.RemoteEndPoint];

                if (m_OnRecive != null)
                {
                    m_OnRecive(socket, dst);
                }
            }
        }
        else
        {
            if (m_OnRecive != null)
            {
                m_OnRecive(null, dst);
            }
        }


        client.BeginReceive(m_recive_buffer, 0, m_recive_buffer.Length, SocketFlags.None, this.OnRecived, client);

    }

}
