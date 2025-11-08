using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Godot;

public static class NetManager
{
    
    public static Socket socket;
	static ByteArray readBuff;
	static Queue<ByteArray> writeQueue;
    static bool isClosing = false;
	static List<MsgBase> msgList = new List<MsgBase>();
	static int msgCount = 0;
	readonly static int MAX_MESSAGE_FIRE = 10;
    public delegate void EventListener(string err);
    private static Dictionary<NetEvent, EventListener> eventListeners =
		new Dictionary<NetEvent, EventListener>();
    public delegate void MsgListener(MsgBase msgBase);
	private static Dictionary<string, MsgListener> msgListener =
		new Dictionary<string, MsgListener>();
	public static bool isUsePing = true;
	public static int pingInterval = 10;
    private static DateTime lastPingTime = DateTime.MinValue;
    private static DateTime lastPongTime = DateTime.MinValue;


    public static void AddMsgListener(string msgName,MsgListener listener)
	{
		if(msgListener.ContainsKey(msgName))
		{
			msgListener[msgName] += listener;
		}
		else
		{
			msgListener[msgName] = listener;
		}
	}
	public static void RemoveListenear(string msgName,MsgListener listener)
	{
        if (msgListener.ContainsKey(msgName))
        {
            msgListener[msgName] -= listener;
			if (msgListener[msgName]==null)
			{
				msgListener.Remove(msgName);
			}
        }
    }
    private static void FireMsg(string msgName, MsgBase msgBase)
    {
        if (msgListener.ContainsKey(msgName))
        {
            msgListener[msgName](msgBase);
        }
    }
    public static void AddEventListener(NetEvent netEvent,EventListener Listener)
	{
		if(eventListeners.ContainsKey(netEvent))
		{
			eventListeners[netEvent] += Listener;
		}
		else
		{
            eventListeners[netEvent] = Listener;
        }
	}
	public static void RemoveEventListener(NetEvent netEvent, EventListener Listener)
	{
		if(eventListeners.ContainsKey(netEvent))
		{
            eventListeners[netEvent] -= Listener;
            if (eventListeners[netEvent] == null)
            {
                eventListeners.Remove(netEvent);
            }
        }

    }
    private static void FireEvent(NetEvent netEvent,string err)
    {
        if (eventListeners.ContainsKey(netEvent))
        {
			eventListeners[netEvent](err);
        }

    }
	static bool isConnecting = false;
	public static void Connect(string ip,int port)
	{
		if(socket!=null&&socket.Connected)
		{
			return;
		}
		if (isConnecting)
		{
            return;
        }
		InitState();
		socket.NoDelay = true;
		isConnecting = true;
		socket.BeginConnect(ip,port,ConnectCallback,socket);
	}

    private static void ConnectCallback(IAsyncResult ar)
    {
        try
		{
			Socket socket = (Socket)ar.AsyncState;
			socket.EndConnect(ar);
            FireEvent(NetEvent.ConnectSucc,"连接成功");
			isConnecting = false;
			socket.BeginReceive(readBuff.bytes, readBuff.writeIdx, readBuff.remain, 0, ReceiveCallback, socket);
		}
        catch (Exception ex)
        {
            FireEvent(NetEvent.ConnectFail, ex.ToString());
            isConnecting = false;
        }
    }
    private static void ReceiveCallback(IAsyncResult ar)
    {
		try
		{
            Socket socket = (Socket)ar.AsyncState;
			int count = socket.EndReceive(ar);
			if(count==0)
			{
				Close();
				return;
			}
			readBuff.writeIdx += count;
			OnReceiveDate();
			if(readBuff.remain<8)
			{
				readBuff.MoveBytes();
				readBuff.ReSize(readBuff.length * 2);
			}
            socket.BeginReceive(readBuff.bytes, readBuff.writeIdx, readBuff.remain, 0, ReceiveCallback, socket);
        }
		catch(SocketException ex)
		{
            throw new Exception("Socket Receive Fail" + ex.ToString());
			
		}

    }
    public static async Task UpdateAsync()
    {
        await Task.Run(() =>
        {
            MsgUpdate();
        });
    }
    public static void Update()
	{
		MsgUpdate();
		//PingUpdate();
	}
    private static void OnReceiveDate()
    {

        if (readBuff.length<=2)
		{
            return;
		}
		int readIdx = readBuff.readIdx;
		byte[] bytes = readBuff.bytes;
		Int16 bodyLength = (Int16)((bytes[readIdx + 1] << 8) | bytes[readIdx]);
        if (readBuff.length<bodyLength+2)
		{
			return;
		}
		readBuff.readIdx += 2;
		int nameCount = 0;
		string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIdx, out nameCount);
        
        if (protoName == "")
		{
            throw new Exception("OnReceiveData Msg.DecodeName Fail");
		}
        readBuff.readIdx += nameCount;
		int bodyCount = bodyLength - nameCount;
		MsgBase msgBase = MsgBase.Decode(protoName, readBuff.bytes, readBuff.readIdx, bodyCount);
		readBuff.readIdx += bodyCount;
		readBuff.CheckAndMoveBytes();
		lock(msgList)
		{
			msgList.Add(msgBase);
		}
		msgCount++;
		if(readBuff.length>2)
		{
			OnReceiveDate();
		}
    }
    public static void MsgUpdate()
	{
		if (msgCount == 0)
		{
            return;
		}
        for (int i = 0; i < MAX_MESSAGE_FIRE; i++)
		{
            MsgBase msgBase = null;
			lock(msgList)
			{
				if(msgList.Count>0)
				{
					msgBase = msgList[0];
					msgList.RemoveAt(0);
					msgCount--;
				}
			}
			if(msgBase!=null)
			{
                FireMsg(msgBase.protoName, msgBase);
                GD.Print(msgBase.protoName);
			}
			else
			{
				break;
			}
		}
	}

    public static void Close()
    {
        if (socket == null) return;

        try
        {
            isClosing = true;
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket.Dispose();
        }
        catch (Exception ex)
        {
            throw new Exception($"关闭连接异常: {ex.Message}");
        }
        finally
        {
            socket = null;
            FireEvent(NetEvent.Close, "");
        }
    }
	public static void Send(MsgBase msg)
	{
        if (socket == null || !socket.Connected)
        {
            return;
        }
        if (isConnecting)
        {
            return;
        }
		if (isClosing)
		{
			return;
		}
		byte[] nameBytes = MsgBase.EncodeName(msg);
		byte[] bodyBytes = MsgBase.Encode(msg);
		int len = nameBytes.Length + bodyBytes.Length;
		byte[] sendBytes = new byte[2 + len];
		sendBytes[0] = (byte)(len % 256);
		sendBytes[1] = (byte)(len / 256);
		Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
		Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);
		ByteArray ba = new ByteArray(sendBytes);
		int count = 0;
		lock(writeQueue)
		{
			writeQueue.Enqueue(ba);
			count = writeQueue.Count;
		}
		if(count == 1)
		{
			socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallBack, socket);
		}
    }

    private static void SendCallBack(IAsyncResult ar)
    {
        Socket socket = (Socket)ar.AsyncState;
        if (socket == null || !socket.Connected)
        {
            return;
        }

        try
        {
            int count = socket.EndSend(ar);
            ByteArray currentBa = null;

            // 处理当前发送的数据块
            lock (writeQueue)
            {
                if (writeQueue.Count == 0)
                {
                    // 队列意外为空，可能是并发操作导致
                    throw new Exception("警告：发送回调时队列已为空");

                }

                // 取出当前正在发送的数据块
                currentBa = writeQueue.Peek(); // 先查看但不移除
                currentBa.readIdx += count;

                // 检查当前数据块是否已全部发送完成
                if (currentBa.length == 0)
                {
                    writeQueue.Dequeue(); // 确认发送完成后才移除
                }
            }

            // 如果还有剩余数据需要发送
            lock (writeQueue)
            {
                if (writeQueue.Count > 0)
                {
                    ByteArray nextBa = writeQueue.Peek();
                    socket.BeginSend(
                        nextBa.bytes,
                        nextBa.readIdx,
                        nextBa.length,
                        0,
                        SendCallBack,
                        socket
                    );
                }
                else if (isClosing) // 队列空且正在关闭
                {
                    socket.Close();
                    FireEvent(NetEvent.Close, "发送完成，安全关闭");
                }
            }
        }
        catch (ObjectDisposedException ex)
        {
            throw new Exception($"发送回调异常（连接已关闭）: {ex.Message}");
        }
        catch (SocketException ex)
        {
            Close();
            throw new Exception($"发送失败: {ex.SocketErrorCode}");
            
        }
        catch (InvalidOperationException ex)
        {
            throw new Exception($"队列操作异常: {ex.Message}");
        }
    }
    private static void PingUpdate()
    {
        if (!isUsePing)
        {
            return;
        }

        // 计算自上次Ping以来的时间
        float timeSinceLastPing = (float)(DateTime.Now - lastPingTime).TotalSeconds;

        // 判断是否超过间隔
        if (timeSinceLastPing >= pingInterval)
        {
            MsgPing msgPing = new MsgPing();
            Send(msgPing);
            lastPingTime = DateTime.Now; // 更新时间戳
            Console.WriteLine("Ping");
        }

        // 保持Pong超时检测逻辑不变
        if ((DateTime.Now - lastPongTime).TotalSeconds > pingInterval * 4)
        {
            Close();
        }
    }
    private static void OnMsgPong(MsgBase msgBase)
	{
		lastPongTime = DateTime.Now;
	}
    private static void InitState()
    {
		socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		readBuff = new ByteArray();
		writeQueue = new Queue<ByteArray>();
		isConnecting = false;
		isClosing = false;
		msgList = new List<MsgBase>();
		msgCount = 0;
		lastPongTime = DateTime.Now;
        lastPingTime = DateTime.Now;
        if (!msgListener.ContainsKey("MsgPong"))
		{
			AddMsgListener("MsgPong", OnMsgPong);
		}
    }
}
	

public enum NetEvent
{
	ConnectSucc = 1,
	ConnectFail = 2,
	Close = 3
}