using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Game.Script.Net
{
    internal class NetManager
    {
        public static Socket listenfd;

        public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();

        public static long pingInterval = 40;

        static List<Socket> checkRead = new List<Socket>();

        public static void StartLoop(int listenPort)
        {
            listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAdr = IPAddress.Parse("127.0.0.1");

            IPEndPoint ipEp = new IPEndPoint(ipAdr, listenPort);

            listenfd.Bind(ipEp);

            listenfd.Listen(0);

            Console.WriteLine("[服务器]启动成功");

            while (true)
            {
                ResetCheakRead();

                Socket.Select(checkRead, null, null, 1000);

                for (int i = checkRead.Count - 1; i >= 0; i--)
                {
                    var s = checkRead[i];
                    if (s == listenfd)
                    {
                        ReadListenfd(s);
                    }
                    else
                    {
                        ReadClientfd(s);
                    }
                }
                Timer();
            }

        }

        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        static void Timer()
        {
            MethodInfo mei = typeof(EventHandler).GetMethod("OnTimer");
            object[] ob = { };
            mei.Invoke(null, ob);
        }

        private static void ReadClientfd(Socket clientfd)
        {
            ClientState state = clients[clientfd];
            ByteArray readBuff = state.readBuff;
            int count = 0;
            if (readBuff.remain <= 0)
            {
                OnReceiveData(state);
                readBuff.MoveBytes();
            }
            if (readBuff.remain <= 0)
            {
                Console.WriteLine("Receive Fail , maybe meg length > buff capacity");
                Close(state);
                return;
            }
            try
            {
                count = clientfd.Receive(readBuff.bytes, readBuff.writeIdx, readBuff.remain, 0);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Receive SocketException" + ex.ToString());
            }
            if (count <= 0)
            {
                Console.WriteLine("Socket Close" + clientfd.RemoteEndPoint.ToString());
                Close(state);
                return;
            }
            readBuff.writeIdx += count;
            OnReceiveData(state);
            readBuff.CheckAndMoveBytes();
        }
        public static void Close(ClientState state)
        {
            MethodInfo mei = typeof(EventHandler).GetMethod("OnDisconnect");
            object[] ob = { state };
            mei.Invoke(null, ob);
            state.socket.Close();
            clients.Remove(state.socket);
        }
        public static void OnReceiveData(ClientState state)
        {
            ByteArray readBuff = state.readBuff ;
            byte[] bytes = readBuff.bytes;
            int readIdx = readBuff.readIdx;
            if (readBuff.length <= 2)
            {
                Console.WriteLine("OnReceiveDataTooShort");
                return;
            }
            Int16 bodyLength = (Int16)((bytes[readIdx + 1] << 8) | bytes[readIdx]);
            if (readBuff.length < bodyLength + 2)
            {
                Console.WriteLine("OnReceiveDataTooShort2");
                return;
            }
            readBuff.readIdx += 2;
            int nameCount = 0;
            string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIdx, out nameCount);
            Console.WriteLine(protoName);
            if (protoName == "")
            {
                Console.WriteLine("OnReceiveData MsgBase.DecodeName fail");
                Close(state);
            }
            readBuff.readIdx += nameCount;
            int bodyCount = bodyLength - nameCount;
            MsgBase msgBase = MsgBase.Decode(protoName, readBuff.bytes, readBuff.readIdx, bodyCount);
            readBuff.readIdx += bodyCount;
            readBuff.CheckAndMoveBytes();
            MethodInfo mi = typeof(MsgHandler).GetMethod(protoName);
            object[] o = { state, msgBase };
            if (mi != null)
            {
                mi.Invoke(null, o);
            }
            else
            {
                Console.WriteLine("OnReceiveData Invoke Fail" + protoName);
            }
            if (readBuff.length > 2)
            {
                OnReceiveData(state);
            }

        }

        private static void ReadListenfd(Socket listenfd)
        {
            try
            {
                Socket clientfd = listenfd.Accept();
                Console.WriteLine("Accept" + clientfd.RemoteEndPoint.ToString());
                ClientState state = new ClientState();
                state.socket = clientfd;
                state.lastPingTime = GetTimeStamp(); // 新增初始化
                clients.Add(clientfd, state);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Accept Fail" + ex.ToString());
            }
        }

        private static void ResetCheakRead()
        {
            checkRead.Clear();
            checkRead.Add(listenfd);
            foreach (ClientState s in clients.Values)
            {
                checkRead.Add(s.socket);
            }
        }
        public static void Send(ClientState cs, MsgBase msg)
        {
            if (cs == null)
                return;
            if (!cs.socket.Connected)
                return;
            byte[] nameBytes = MsgBase.EncodeName(msg);
            byte[] bodyBytes = MsgBase.Encode(msg);
            int len = nameBytes.Length + bodyBytes.Length;
            byte[] sendBytes = new byte[2 + len];

            sendBytes[0] = (byte)(len % 256);
            sendBytes[1] = (byte)(len / 256);
            Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
            Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);
            try
            {
                cs.socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, null, null);
            }
            catch(SocketException ex)
            {
                Console.WriteLine("Socket Close On Begin Send" + ex.ToString());
            }
        }
    }
}
