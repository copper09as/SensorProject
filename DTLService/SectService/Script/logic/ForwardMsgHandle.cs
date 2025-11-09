using Game.Script.Net;
public partial class MsgHandler
{
    public static void MsgForward(ClientState c, MsgBase msgBase)
    {
        MsgForward msg = (MsgForward)msgBase;
        foreach(var client in NetManager.clients.Values)
        {
            NetManager.Send(client, msg);
        }
    }
}
