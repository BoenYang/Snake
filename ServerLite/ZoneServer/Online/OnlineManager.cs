using CGF;
using CGF.Common;
using CGF.Network.General.Server;
using GlobalData.Proto;

namespace Snake.Server.ZoneServer.Online
{
    public class OnlineManager : Singleton<OnlineManager>
    {
        private NetManager m_net;

        public void Init(ServerContext context)
        {
            m_net = context.net;
            m_net.AddGeneralMsgListener<LoginReq>(ProtoCmd.LoginReq , OnLoginReq);

            m_net.AddRPCListener(this);
        }

        private void OnLoginReq(ISession session, uint index, LoginReq req)
        {
            Debuger.Log(req.account);
            LoginRsp rsp = new LoginRsp();
            rsp.name = "Login Success";
            m_net.Send(session,ProtoCmd.LoginReq,index,rsp);
        }

        public void Tick()
        {
            m_net.Tick();
        }


        public void Test(ISession session,string test)
        {
            m_net.Invoke(session,"OnTest",test);
        }
    }
}