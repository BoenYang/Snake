using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace CGF.Network.Client
{
    public class TCPConnection : IConnection
    {
        public Action<byte[],int> OnRecive;
       
        public bool Connected{ get { return m_connected; } }

        public string Ip { get { return m_ip; }}
        public int Port { get { return m_port; }}

        private bool m_connected;

        private Socket m_socket;

        private EndPoint m_remoteEndPoint;

        private Thread m_reciveThread;

        private string m_ip;

        private int m_port;

        public void Connect(string ip, int port)
        {
            m_ip = ip;
            m_port = port;
            m_remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip),port);
            m_socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.IPv4);
            m_socket.Connect(m_remoteEndPoint);
            m_connected = true;
            m_reciveThread = new Thread(Thread_Recive);
            m_reciveThread.Start();
        }

        public void Close()
        {
            m_connected = false;
            
            if (m_socket != null)
            {
                m_socket.Shutdown(SocketShutdown.Both);
                m_socket = null;
            }

            if (m_reciveThread != null)
            {
                m_reciveThread.Interrupt();
            }
        }

        public bool Send(byte[] bytes, int len)
        {
            return m_socket.Send(bytes, SocketFlags.None) > 0;
        }

        public void Tick()
        {
            if (Connected)
            {
                DoReciveInMain();
            }
            else
            {
                //TODO 重连
            }
        }

        private void DoReciveInMain()
        {
            SwitchQueue();
            while (m_consumeQueue.Count > 0)
            {
                byte[] buffer = m_consumeQueue.Dequeue();
                OnRecive(buffer, buffer.Length);
            }
        }

        private byte[] m_reciveBuffer = new byte[4096];

        private Queue<byte[]> m_productQueue = new Queue<byte[]>();

        private Queue<byte[]> m_consumeQueue = new Queue<byte[]>();


        private void Thread_Recive()
        {
            while (Connected)
            {
                try
                {
                    DoReciveInThread();
                }
                catch (Exception e)
                {
                    Thread.Sleep(1);
                }
             
            }
        }

        private void DoReciveInThread()
        {
            EndPoint remotePoint = new IPEndPoint(IPAddress.Any, 0);
            int len = m_socket.ReceiveFrom(m_reciveBuffer, SocketFlags.None, ref remotePoint);

            if (len > 0)
            {
                if (m_remoteEndPoint.Equals(remotePoint))
                {

                    byte[] buffer = new byte[len];
                    Buffer.BlockCopy(m_reciveBuffer, 0, buffer, 0, len);
                    m_productQueue.Enqueue(buffer);
                }
                else
                {

                }
            }
        }

        private void SwitchQueue()
        {
            Queue<byte[]> temp = m_consumeQueue;
            m_consumeQueue = m_productQueue;
            m_productQueue = temp;
        }
    }
}
