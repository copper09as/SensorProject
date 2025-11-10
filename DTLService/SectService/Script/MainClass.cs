using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Game.Script.Net;
using Newtonsoft.Json;

namespace Game.Script
{
    internal class MainClass
    {
        public static void Main(string[] arg)
        {
            NetManager.StartLoop(43195);
        }
    }
}
