using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine.Assertions.Must;

namespace CGF.Network.General.Client
{
    public class TCPConnection : IConnection
    {
        public Action<byte[], int> OnRecive { get; set; }

        public bool Connected { get; private set; }
        public int BindPort { get; private set; }
        public int Id { get; private set; }


        private Socket m_socket;

        private EndPoint m_remoteEndPoint;

        private Thread m_reciveThread;

        private string m_ip;

        private int m_port;

        public void Init(int connId, int bindPort)
        {
            BindPort = bindPort;
            Id = connId;
        }

        public void Connect(string ip, int port)
        {
            Connected = false;
            m_ip = ip;
            m_port = port;
            m_remoteEndPoint = new IPEndPoint(IPAddress.Parse(m_ip),m_port);
            m_socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.IPv4);
            m_socket.Connect(m_remoteEndPoint);
            m_reciveThread = new Thread(Thread_Recive);
            m_reciveThread.Start();
            Connected = true;
        }

        public void Clean()
        {
            this.Close();
        }

        public void Close()
        {
            Connected = false;
            
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
                //TODO 重连逻辑
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
