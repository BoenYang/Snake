using System;
using System.Collections.Generic;
using CGF.Common;

namespace CGF.Core
{
    public class ModuleManager : Singleton<ModuleManager>
    {

        private Dictionary<string, BusinessModule> m_moduleDict = new Dictionary<string, BusinessModule>();

        private string m_domain = "";

        public void Init(string domain)
        {
            m_domain = domain;
        }

        public BusinessModule CreateModule(string moduleName, params object[] args)
        {
            BusinessModule module = GetModule(moduleName);

            if (module != null)
            {
                return module;
            }

            string type = moduleName;

            if (!string.IsNullOrEmpty(m_domain))
            {
                type = m_domain + "." + moduleName;
            }

            Type moduleType = Type.GetType(type);

            if (moduleType != null)
            {
                module = Activator.CreateInstance(moduleType) as BusinessModule;
            }
            else
            {
                return null;
            }

            module.Create(args);
            m_moduleDict.Add(moduleName, module);
            return module;
        }

        public BusinessModule GetModule(string moduleName)
        {
            if (m_moduleDict.ContainsKey(moduleName))
            {
                return m_moduleDict[moduleName];
            }
            return null;
        }

    }
}

