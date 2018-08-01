

using System.Collections.Generic;

public class TCPProxy
{
    private TCPSocket m_socket;

    private Queue<byte[]> m_recvQueue; 

    public TCPProxy(TCPSocket socket)
    {
        m_socket = socket;
    }

}
