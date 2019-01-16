namespace CGF.Server
{
    public class ServerModule : ILogTag
    {
        public string LOG_TAG { get; private set; }

        protected ServerModuleInfo m_info;

        internal void Create(ServerModuleInfo info)
        {
            m_info = info;
            LOG_TAG = this.GetType().Name + "[" + info.port + "]";
            this.Log();
        }

        public virtual void Release()
        {
        }

        public virtual void Start()
        {

        }

        public virtual void Stop()
        {

        }

        public virtual void Tick()
        {

        }

    }
}