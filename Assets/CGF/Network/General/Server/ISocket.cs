using System;
using System.Net;

namespace CGF.Network.General.Server
{
    public interface ISocket
    {
        Action<uint, byte[], int, object> OnReceive { set; get; }

        void Start(int port);

        void ShutDown();

        ISession CreateSession(ISessionListener listener,uint sid, object arg);
    }
}