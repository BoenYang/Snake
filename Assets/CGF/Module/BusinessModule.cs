
namespace FrameWork.Core
{
    public abstract class BusinessModule : Module
    {
        private string _name;

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    _name = GetType().Name;
                }
                return _name;
            }
        }

        public string Title;


        public BusinessModule()
        {

        }

        internal BusinessModule(string name)
        {
            this._name = name;
        }

        private EventTable _eventTable;

        public ModuleEvent Event(string type)
        {
            if (_eventTable == null)
            {
                _eventTable = new EventTable();
            }
            return _eventTable.GetEvent(type);
        }

        public virtual void Create(params object[] args)
        {

        }

        public virtual void Show()
        {

        }

        public override void Release()
        {
            if (_eventTable != null)
            {
                _eventTable.Clear();
                _eventTable = null;
            }
            base.Release();
        }
    }
}
