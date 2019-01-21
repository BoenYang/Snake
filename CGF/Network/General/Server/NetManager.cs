using System;
using System.Collections.Generic;
using System.Reflection;
using CGF.Network.Core;
using CGF.Network.Core.RPC;
using SGF.Codec;

namespace CGF.Network.General.Server
{
    public class NetManager : ISessionListener
    {

        private Gateway m_gateWay;

        private RPCManagerBase m_rpc;

        private Dictionary<uint,GeneralMsgListener> m_generalMsgListeners;

        public void Init(int port)
        {
            Debuger.Log();
            m_generalMsgListeners = new Dictionary<uint, GeneralMsgListener>();

            m_gateWay = new Gateway();
            m_gateWay.Init(port,this);

            m_rpc = new RPCManagerBase();
            m_rpc.Init();
        }

        public void Clean()
        {
            if(m_gateWay != null){
                m_gateWay.Clean();
                m_gateWay = null;
            }

            if (m_rpc != null)
            {
                m_rpc.Clean();
                m_rpc = null;
            }
        }

        public void OnReceive(ISession session, byte[] bytes, int len)
        {
            NetMessage msg = new NetMessage();
            msg.Deserialize(bytes, len);

            if (msg.head.cmd == 0)
            {
                RPCMessage rpcMsg = PBSerializer.NDeserialize<RPCMessage>(msg.content);
                HandleRPCMessage(session,rpcMsg);
            }
            else
            {
                HandleGeneralMessage(session,msg);
            }
        }

        public void Send<TRsp>(ISession session, uint cmd, uint index,TRsp rsp)
        {
            NetMessage msg = new NetMessage();
            msg.head.cmd = cmd;
            msg.head.index = index;
            msg.head.uid = session.uid;
            msg.content = PBSerializer.NSerialize(rsp);
            msg.head.dataSize = (uint)msg.content.Length;

            byte[] temp;
            int len = msg.Serialize(out temp);
            session.Send(temp,len);
        }

        public void AddRPCListener(object listener)
        {
            m_rpc.RegisterListener(listener);
        }

        public void RemoveRPCListener(object listener)
        {
            m_rpc.UnregisterListener(listener);
        }

        private void HandleRPCMessage(ISession session, RPCMessage rpcMsg)
        {
            RPCMethodHelper helper = m_rpc.GetRPCMethodHelper(rpcMsg.name);
            if (helper != null)
            {
                object[] args = new object[rpcMsg.raw_args.Count + 1];
                List<RPCRawArg> rawArgs = rpcMsg.raw_args;
                ParameterInfo[] parameterInfo = helper.method.GetParameters();
                args[0] = session;
                if (parameterInfo.Length == (rawArgs.Count + 1))
                {
                    for (int i = 0; i < rawArgs.Count; i++)
                    {
                        if (rawArgs[i].type == RPCArgType.PBObject)
                        {
                            Type type = parameterInfo.GetType();
                            args[i + 1] = PBSerializer.NDeserialize(rawArgs[i].raw_value,type);
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
                        Debuger.LogError(e);
                    }
                }
                else
                {
                    Debuger.LogError("参数数量不一致");
                }
            }
            else
            {
                Debuger.LogError("没找到{0}的方法",rpcMsg.name);
            }
        }

        public void Invoke(ISession session, string name, params object[] args)
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
            session.Send(temp,len);
        }

        public void AddGeneralMsgListener<TMsg>(uint cmd, Action<ISession, uint, TMsg> onMsg)
        {
            GeneralMsgListener listener = new GeneralMsgListener();
            listener.msgType = typeof(TMsg);
            listener.onMsg = onMsg;
            m_generalMsgListeners.Add(cmd, listener);
        }

        class GeneralMsgListener
        {
            public Type msgType;
            public Delegate onMsg;
        }

        private void HandleGeneralMessage(ISession session,NetMessage msg)
        {
            GeneralMsgListener listener = m_generalMsgListeners[msg.head.cmd];
            if (listener != null)
            {
                object obj = PBSerializer.NDeserialize(msg.content, listener.msgType);
                if (obj != null)
                {
                    try
                    {
                        listener.onMsg.DynamicInvoke(session, msg.head.index, obj);
                    }
                    catch (Exception e)
                    {
                        Debuger.LogError(e.Message);
                    }
                }
                else
                {
                    Debuger.LogError("协议解析失败");
                }
            }
        }

        public void Tick()
        {
            m_gateWay.Tick();
        }

    }
}