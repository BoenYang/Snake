using System;
using System.Threading;

namespace CGF.Server
{
    public class MainLoop
    {
        public static void Run()
        {
            Debuger.Log();
            while (true)
            {
                ServerManager.Instance.Tick();
                Thread.Sleep(1);
            }
          
        }
    }
}