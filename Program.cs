using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMQServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Init();
            if (args.Length != 1)
            {
                Logger.Log("no arguments!");
                Environment.Exit(0);
            }
            Server server = new Server(args[0]);

            server.StartLoops();
        }
    }
}
