namespace CGF.CGF.Server
{
    public class ServerModule : ILogTag
    {
        public string LOG_TAG { get; private set; }

        private ServerModule m_info;

        internal void Create(ServerModule info)
        {
            m_info = info;
            LOG_TAG = this.GetType().Name + "[" + "]";
            this.Log();
        }

        internal virtual void Release()
        {
        }

        internal virtual void Start()
        {

        }

        internal virtual void Stop()
        {

        }

        internal virtual void Tick()
        {

        }

    }
}