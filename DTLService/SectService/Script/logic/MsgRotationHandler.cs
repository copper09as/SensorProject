using Game.Script.Net;
public partial class MsgHandler
{
    public static void MsgRotation(ClientState c, MsgBase msgBase)
    {
        MsgRotation msg = (MsgRotation)msgBase;
        foreach (var client in NetManager.clients.Values)
        {
            NetManager.Send(client, msg);
        }
        Console.WriteLine(msg.rotation);
    }
}
