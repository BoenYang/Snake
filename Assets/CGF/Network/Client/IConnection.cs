using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGF.Network.Client { 

    public interface IConnection
    {

        bool Connected { get; }

        string Ip { get; }

        int Port { get; }

        void Connect(string ip,int port);

        void Close();

        bool Send(byte[] bytes, int len);

        void Tick();
    }
}


