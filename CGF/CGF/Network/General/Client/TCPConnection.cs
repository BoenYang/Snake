using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CGF.Network.Core;
using SGF.Network.Core;

namespace CGF.Network.General.Client
{
    public class TCPConnection : IConnection
    {
        public Action<byte[], int> OnRecive { get; set; }

        public bool Connected { get; private set; }

        public uint Id { get; private set; }

        private Socket m_socket;

        private EndPoint m_remoteEndPoint;

        private Thread m_reciveThread;

        private string m_ip;

        private int m_port;

        private SwitchQueue<byte[]> m_recvQueue;

        private NetBuffer m_recvNetBuffer;

        private byte[] m_recvBuffer = new byte[4096];

        public void Init()
        {
            m_recvQueue = new SwitchQueue<byte[]>();
            m_recvNetBuffer = new NetBuffer();
        }

        public void Connect(string ip, int port)
        {
            Connected = false;
            m_ip = ip;
            m_port = port;
            m_remoteEndPoint = new IPEndPoint(IPAddress.Parse(m_ip),m_port);
            m_socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            m_socket.Connect(m_remoteEndPoint);
            m_reciveThread = new Thread(Thread_Recive);
            m_reciveThread.Start();
            Connected = true;
            Debuger.Log("Connect to {0} port {1} success",ip,port);
        }

        public void Clean()
        {
            this.Close();
        }

        public void Close()
        {
            Debuger.Log();
            Connected = false;

            if (m_reciveThread != null)
            {
                m_reciveThread.Interrupt();
                m_reciveThread = null;
            }

            if (m_socket != null)
            {
                m_socket.Shutdown(SocketShutdown.Both);
                m_socket.Close();
                m_socket = null;
            }
        }

        public bool Send(byte[] bytes, int len)
        {
            try
            {
                Buffer.BlockCopy(bytes,0,bytes,8,len);
                NetBuffer buffer = new NetBuffer();
                buffer.Attach(bytes, 8 + len);
                buffer.WriteUInt(Id,0);
                buffer.WriteUInt((uint)(8 + len),4);
                return m_socket.Send(buffer.GetBytes(),buffer.Length,SocketFlags.None) > 0;
            }
            catch (Exception e)
            {
                Debuger.Log("exception ");
                return false;
            }
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
            m_recvQueue.Switch();
            while (!m_recvQueue.Empty())
            {
                byte[] buffer = m_recvQueue.Pop();
                OnRecive(buffer, buffer.Length);
            }
        }


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
                    Debuger.LogError(e.Message);
                    Thread.Sleep(1);
                }
             
            }
        }

        private void DoReciveInThread()
        {
            EndPoint remotePoint = new IPEndPoint(IPAddress.Any, 0);
            int len = m_socket.Receive(m_recvBuffer, SocketFlags.None);

            if (len > 0)
            {
                Debuger.Log("Recive Data len = " + len);
                if (m_remoteEndPoint.Equals(remotePoint))
                {
                    if (m_recvNetBuffer.Length == 0)
                    {
                        m_recvNetBuffer.Attach(m_recvBuffer, len);
                    }
                    else
                    {
                        if (m_recvNetBuffer.Length + len > m_recvNetBuffer.Capacity)
                        {
                            m_recvNetBuffer.AdjustCapacity(m_recvNetBuffer.Length + len);
                        }
                        m_recvNetBuffer.AddBytes(m_recvBuffer, m_recvNetBuffer.Length - 1, len);
                    }
                    ReadPackage();
                }
                else
                {
                    //不是远程服务器的数据
                }
            }
        }

        private void ReadPackage()
        {
            uint sid = m_recvNetBuffer.ReadUInt();
            uint packageSize = m_recvNetBuffer.ReadUInt();
            while (m_recvNetBuffer.Length >= packageSize)
            {
                long restBufferLen = m_recvNetBuffer.Length - packageSize;
                bool continueRead = restBufferLen > 0;
                byte[] package = new byte[packageSize - 8];
                m_recvNetBuffer.ReadBytes(package, 0, (int)packageSize - 8);
                m_recvQueue.Push(package);
                Debuger.Log("read a package ");
                if (continueRead)
                {
                    m_recvNetBuffer.CopyWith(m_recvNetBuffer, 0, true);
                    ReadPackage();
                }
                else
                {
                    m_recvNetBuffer.Clear();
                }
            }
        }
    }
}
