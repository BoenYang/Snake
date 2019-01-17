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

            m_net.AddRPCListener(this);
        }

        public void Login(string account,string password)
        {
            LoginReq req = new LoginReq();
            req.account = account;
            req.password = password;

            m_net.Send<LoginReq,LoginRsp>(ProtoCmd.LoginReq,req,OnLoginResponse);
        }

        private void OnLoginResponse(LoginRsp rsp)
        {
            Debuger.Log("登录返回");
            Debuger.Log(rsp.name);
            RPCTest();
        }

        private void RPCTest()
        {
            m_net.Invoke("Test","RPCTest");
        }

        private void OnTest(string content)
        {
            Debuger.Log(content);
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