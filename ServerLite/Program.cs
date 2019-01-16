using System;
using CGF;
using CGF.Server;

namespace Snake.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            InitDebuger();

            ServerManager.Instance.Init("Snake.Server");
            ServerManager.Instance.StarServer(1);
            MainLoop.Run();
            ServerManager.Instance.StopAllServer();
            Console.WriteLine("GameOver");
            Console.ReadKey();
        }

        private static void InitDebuger()
        {
            Debuger.Init(AppDomain.CurrentDomain.BaseDirectory + "/ServerLog/");
            Debuger.EnableLog = true;
            Debuger.EnableSave = true;
            Debuger.Log();
        }
    }
}
