namespace CGF.Module
{
    public class GeneralModule : ModuleBase , ILogTag
    {
        private string m_name;

        public string LOG_TAG { get; protected set; }

        public string Name
        {
            get
            {
                return m_name;
            }
        }

        public string Title;


        public GeneralModule()
        {
            m_name = this.GetType().Name;
            LOG_TAG = m_name;
        }


        public virtual void Create(params object[] args)
        {
            this.Log("args:{0}", args);
        }

        public virtual void Show(object arg)
        {
            this.Log("args:{0}", arg);
        }

        public override void Release()
        {
            base.Release();
            this.Log();
        }


    }
}