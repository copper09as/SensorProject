using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Game.Script.Net
{
    public class ClientState
    {
        public Socket socket;
        public ByteArray readBuff = new ByteArray();

        public long lastPingTime = 0;
    }
}
