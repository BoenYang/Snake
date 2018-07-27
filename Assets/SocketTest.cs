using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class SocketTest : MonoBehaviour
{
    private TCPSocket m_server;

    private TCPSocket m_client;

    private TCPSocket m_service_client;


    public void onStartServer()
    {
        if (m_server == null)
        {
            m_server = new TCPSocket();
            m_server.Bind(1000);
            m_server.AddClientConnectedListener(this.onClientConnect);
            m_server.AddRecivedListener(this.onServerRecive);
        }
    }

    public void onStartClient()
    {
        if (m_client == null)
        {
            m_client = new TCPSocket();
            m_client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1000));
            m_client.AddRecivedListener(this.onClientRecive);
            Debug.Log("client connected");
        }
    }

    public void onSendTestData()
    {
        string str = "hello world";
        byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
        m_client.Send(data);
    }

    public void onCloseClient()
    {
        m_client.Close();
        Debug.Log("client close");

    }

    private void onClientConnect(TCPSocket socket)
    {
        m_service_client = socket;
        Debug.Log("server : client connected");
    }

    private void onServerRecive(TCPSocket socket, byte[] data)
    {
        string s = System.Text.Encoding.UTF8.GetString(data);
        Debug.Log("server revice " + s);
        s += " client";
        byte[] d = System.Text.Encoding.UTF8.GetBytes(s);
        m_service_client.Send(d);
    }  

    private void onClientRecive(TCPSocket socket, byte[] data)
    {
        string s = System.Text.Encoding.UTF8.GetString(data);
        Debug.Log("server revice " + s);
    }
}
