using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class FPSClient
{

    private TCPSocket m_socket;

    public void Connect(IPEndPoint remoteAddress)
    {
        m_socket = new TCPSocket();
        m_socket.Connect(remoteAddress);
        m_socket.AddRecivedListener(this.onClientRecive);
    }

    private void onClientRecive(TCPSocket socket, byte[] data)
    {
  
    }

    public void Send(byte[] data)
    {
        m_socket.Send(data);
    }
}
