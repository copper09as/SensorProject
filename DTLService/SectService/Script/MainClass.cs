using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Script.Net;

namespace Game.Script
{
    internal class MainClass
    {
        public static void Main(string[] arg)
        {
            var port = Convert.ToInt32(Console.ReadLine());
            NetManager.StartLoop(port);
        }
    }
}
