using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CGF.Network.General.Client
{
    public class NetManager
    {
        private IConnection m_connection;

        private uint m_uid;

        public void Init(Type connectType,int connectId,int bindPort)
        {
            //connect id 可以用于区别连接的网络连接的类型
            m_connection = Activator.CreateInstance(connectType) as IConnection;
            m_connection.Init(connectId,bindPort);
        }

        public void Connect(string ip,int port)
        {
            m_connection.Connect(ip,port);
            m_connection.OnRecive += OnRecive;
        }

        public void Clean()
        {
            m_connection = null;
            this.Close();
        }

        public void Close()
        {
            m_connection.Close();
        }

        public void SetUserId(uint uid)
        {
            m_uid = uid;
        }

        public void Send(byte[] data,int len)
        {
            m_connection.Send(data, len);
        }

        private void OnRecive(byte[] data, int len)
        {
            //反序列化data，并进行分发
        }

        public void Tick()
        {
            
        }
    }
}
