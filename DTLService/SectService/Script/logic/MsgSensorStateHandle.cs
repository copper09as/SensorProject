using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Script.Net;

public partial class MsgHandler
{
    public static void MsgSensorState(ClientState c, MsgBase msgBase)
    {
        MsgSensorState msg = (MsgSensorState)msgBase;
        foreach (var client in NetManager.clients.Values)
        {
            NetManager.Send(client, msg);
        }

    }
}