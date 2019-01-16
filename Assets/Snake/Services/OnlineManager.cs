using CGF;
using CGF.Common;
using CGF.Network.General.Client;
using GlobalData.Proto;

namespace Snake.Services
{
    public class OnlineManager : Singleton<OnlineManager>
    {
        private NetManager m_net;

        public void Init()
        {
            m_net = new NetManager();
            m_net.Init(typeof(TCPConnection));
            m_net.Connect("127.0.0.1",4540);
        }

        public void Login(string account,string password)
        {
            LoginReq req = new LoginReq();
            req.account = account;
            req.password = password;

            m_net.Send<LoginReq,LoginRsp>(ProtoCmd.LoginReq,req,OnLoginResponse);
        }

        public void OnLoginResponse(LoginRsp rsp)
        {
            Debuger.Log(rsp.name);
        }

        public void Clean()
        {
            m_net.Clean();
        }

        public void Tick()
        {
            m_net.Tick();
        }

    }
}