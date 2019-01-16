using System.Collections.Generic;
using System.IO;
using MiniJSON;

namespace CGF.Server
{
    public class ServerModuleInfo
    {
        public int id;
        public string name;
        public string assembly;
        public int port;
    }

    public class ServerConfig
    {
        internal static string NameSpace = "";
        internal static string ServerConfigFile = "./serverconfig.json";
        internal static Dictionary<int, ServerModuleInfo> serverModuleMap;

        public static ServerModuleInfo GetServerModuleInfo(int id)
        {
            if (serverModuleMap == null)
            {
                GetServerConfigs();
            }
            return serverModuleMap[id];
        }

        private static void GetServerConfigs()
        {
            serverModuleMap = new Dictionary<int, ServerModuleInfo>();
            string jsonStr = File.ReadAllText(ServerConfigFile);
            var obj = Json.Deserialize(jsonStr) as List<object>;
            for (int i = 0; i < obj.Count; i++)
            {
                Dictionary<string, object> jObj = obj[i] as Dictionary<string, object>;
                ServerModuleInfo info = new ServerModuleInfo();
                info.id = (int) (long) jObj["id"];
                info.name = (string)jObj["name"];
                info.assembly = (string)jObj["assembly"];
                info.port = (int)(long)jObj["port"];
                serverModuleMap.Add(info.id,info);
            }
        }
    }
}