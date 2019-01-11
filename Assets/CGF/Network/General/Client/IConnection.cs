using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGF.Network.General.Client { 

    public interface IConnection
    {
        Action<byte[], int> OnRecive { get; set; }

        bool Connected { get; }

        int BindPort { get; }

        int Id { get; }

        void Init(int connId, int bindPort);

        void Connect(string ip,int port);

        void Clean();

        void Close();

        bool Send(byte[] bytes, int len);

        void Tick();
    }
}


