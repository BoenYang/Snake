using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ProtoBuf;
using UnityEngine;

public class SocketTest : MonoBehaviour
{
    private FPSServer m_server;

    private FPSClient m_client;


    public void onStartServer()
    {
        m_server = new FPSServer();
        m_server.Start(1000);
    }

    public void onStartClient()
    {
        m_client = new FPSClient();
        m_client.Connect(new IPEndPoint(IPAddress.Parse( "127.0.0.1"),1000));
    }

    public void onSendTestData()
    {
        EnterRoomRequest request = new EnterRoomRequest();
        request.uid = 1234;
        byte[] data;
        data = request.Serialize();
        m_client.Send(data);
    }

    public void onCloseClient()
    {

    }

    private void onClientConnect(TCPSocket socket)
    {
    }

    private void onServerRecive(TCPSocket socket, byte[] data)
    {
    }  

    private void onClientRecive(TCPSocket socket, byte[] data)
    {

    }
}
