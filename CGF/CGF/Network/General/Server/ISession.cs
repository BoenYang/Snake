
namespace CGF.Network.General.Server
{

    public interface ISessionListener
    {
        void OnReceive(ISession session, byte[] bytes, int len);
    }

    public static class SessionID
    {
        private static uint ms_lastSid = 0;
        public static uint NewID()
        {
            return ++ms_lastSid;
        }
    }


    public interface ISession
    {
        uint sid { get; }

        uint uid { get; }

        void Active(object arg);

        bool IsActive();

        void Send(byte[] bytes, int len);

        void Tick();

        void DoReciveInGateway(byte[] bytes, int len);
    }
}

