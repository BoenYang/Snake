
using System.Collections.Generic;

public class FSPSession
{
    private TCPSocket m_socket;

    private Queue<byte[]> m_recvQueue = new Queue<byte[]>(); 


    public FSPSession(TCPSocket socket)
    {
        m_socket = socket;
    }

    public void OnRecive(byte[] data)
    {
        m_recvQueue.Enqueue(data);
    }


}
