using System;
using System.Collections.Generic;
using System.Reflection;
using CGF.Network.Core;
using CGF.Network.Core.RPC;
using SGF.Codec;

namespace CGF.Network.General.Client
{
    public class NetManager
    {
        private IConnection m_connection;

        private RPCManagerBase m_rpc;

        private uint m_uid;

        private Dictionary<uint, GeneralMsgListener> m_generalMsgListeners;

        private Dictionary<uint, NotifyMsgListener> m_notifyMsgListeners;

        public void Init(Type connectType,int connectId,int bindPort)
        {
            //connect id 可以用于区别连接的网络连接的类型
            m_connection = Activator.CreateInstance(connectType) as IConnection;
            m_connection.Init();

            m_generalMsgListeners = new Dictionary<uint, GeneralMsgListener>();
            m_notifyMsgListeners = new Dictionary<uint, NotifyMsgListener>();

            m_rpc = new RPCManagerBase();
            m_rpc.Init();
        }

        public void Connect(string ip,int port)
        {
            m_connection.Connect(ip,port);
            m_connection.OnRecive += OnRecive;
        }

        public void Clean()
        {
            if (m_connection != null)
            {
                m_connection.Clean();
                m_connection = null;
            }

            if (m_rpc != null)
            {
                m_rpc.Clean();
                m_rpc = null;
            }

            if (m_generalMsgListeners != null)
            {
                m_generalMsgListeners.Clear();
                m_generalMsgListeners = null;
            }

            if (m_notifyMsgListeners != null)
            {
                m_notifyMsgListeners.Clear();
                m_notifyMsgListeners = null;
            }
        }

        public void Close()
        {
            m_connection.Close();
        }

        public void SetUserId(uint uid)
        {
            m_uid = uid;
        }

        private void OnRecive(byte[] data, int len)
        {
            //反序列化data，并进行分发
            NetMessage msg = new NetMessage();
            msg.Deserialize(data, len);
            if (msg.head.cmd == 0)
            {
                RPCMessage rpcMsg = new RPCMessage();
                HandleRPCMessage( rpcMsg);
            }
            else
            {
                HandleGeneralMessage( msg);
            }
        }


        public void AddRPCListener(object listener)
        {
            m_rpc.RegisterListener(listener);
        }

        public void RemoveRPCListener(object listener)
        {
            m_rpc.UnregisterListener(listener);
        }

        private void HandleRPCMessage(RPCMessage rpcMsg)
        {
            RPCMethodHelper helper = m_rpc.GetRPCMethodHelper(rpcMsg.name);
            if (helper != null)
            {
                object[] args = new object[rpcMsg.raw_args.Count + 1];
                List<RPCRawArg> rawArgs = rpcMsg.raw_args;
                ParameterInfo[] parameterInfo = helper.method.GetParameters();

                if (parameterInfo.Length == rawArgs.Count)
                {
                    for (int i = 0; i < rawArgs.Count; i++)
                    {
                        if (rawArgs[i].type == RPCArgType.PBObject)
                        {

                        }
                        else
                        {
                            args[i + 1] = rawArgs[i].value;
                        }
                    }

                    try
                    {
                        helper.method.Invoke(helper.listener, BindingFlags.NonPublic, null, args, null);

                    }
                    catch (Exception e)
                    {
                        Debuger.Log(e);
                    }
                }
                else
                {
                    Debuger.LogError("RPC参数不一致");
                }
            }
            else
            {
                Debuger.LogError("没找到{0}的方法", rpcMsg.name);
            }
        }

        public void Invoke(string name, params object[] args)
        {
            RPCMessage rpcMsg = new RPCMessage();
            rpcMsg.name = name;
            rpcMsg.args = args;

            byte[] buffer = PBSerializer.NSerialize(rpcMsg);

            NetMessage msg = new NetMessage();
            msg.content = buffer;
            msg.head = new ProtocalHead();
            msg.head.dataSize = (uint)buffer.Length;
            msg.head.cmd = 0;

            byte[] temp = null;
            int len = msg.Serialize(out temp);

            m_connection.Send(temp, len);
        }


        class GeneralMsgListener
        {
            public uint cmd;
            public float timeout;
            public Type rspType;
            public Delegate onRsp;
            public Delegate onError;
            public uint index;
            public float timestamp;
        }

        class NotifyMsgListener
        {
            public Type msgType;
            public Delegate onNotify;
        }

        static class MessageIndexGenerator
        {
            private static uint m_lastIndex;
            public static uint NewIndex()
            {
                return ++m_lastIndex;
            }
        }

        private void AddGeneralMsgListener(uint cmd,uint index, Type rspType, Delegate onRsp, float timeout, Delegate onError)
        {
            GeneralMsgListener listener = new GeneralMsgListener();
            listener.timeout = timeout;
            listener.rspType = rspType;
            listener.onRsp = onRsp;
            listener.onError = onError;
            listener.index = index;
            listener.cmd = cmd;
            m_generalMsgListeners.Add(index, listener);
        }

        private void AddNotifyMsgListener<TNtf>(uint cmd, Action<TNtf> onNotify)
        {
            NotifyMsgListener listener = new NotifyMsgListener();
            listener.msgType = typeof(TNtf);
            listener.onNotify = onNotify;

        }

        private void Send<TReq, TRsp>(uint cmd, TReq req, Action<TRsp> onRsp, float timeout = 30, Action<uint> onError = null)
        {
            NetMessage msg = new NetMessage();
            msg.head.cmd = cmd;
            msg.head.uid = m_uid;
            msg.head.index = MessageIndexGenerator.NewIndex();
            msg.content = PBSerializer.NSerialize(req);
            msg.head.dataSize = (uint)msg.content.Length;

            byte[] tempBuf;
            int len = msg.Serialize(out tempBuf);
            m_connection.Send(tempBuf, len);

            AddGeneralMsgListener(cmd,msg.head.index, typeof(TRsp), onRsp, timeout, onError);
        }

        private void HandleGeneralMessage(NetMessage msg)
        {
            if (msg.head.index == 0)
            {
                NotifyMsgListener listener = m_notifyMsgListeners[msg.head.cmd];
                if (listener != null)
                {
                    object obj = PBSerializer.NDeserialize(msg.content,listener.msgType);
                    if (obj != null)
                    {
                        listener.onNotify.DynamicInvoke(obj);
                    }
                    else
                    {
                        Debuger.LogError(" 协议解析失败");
                    }
                }
                else
                {
                    Debuger.LogError("找不到协议监听器");
                }
            }
            else
            {
                GeneralMsgListener listener = m_generalMsgListeners[msg.head.index];
                if (listener != null)
                {
                    m_generalMsgListeners.Remove(msg.head.index);

                    object obj = PBSerializer.NDeserialize(msg.content, listener.rspType);
                    if (obj != null)
                    {
                        listener.onRsp.DynamicInvoke(obj);
                    }
                    else
                    {
                        Debuger.LogError(" 协议解析失败");
                    }
                }
                else
                {
                    Debuger.LogError("找不到协议监听器");
                }
            }
        }

        private void CheckTimeOut()
        {

        }


        public void Tick()
        {
            m_connection.Tick();

        }
    }
}
