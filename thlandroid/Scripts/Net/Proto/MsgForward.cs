using System.Collections;
using System.Collections.Generic;
using Models;


public class MsgForward : MsgBase
{
    public MsgForward()
    {
        protoName = "MsgForward";
    }
    public THLData ThlData;
}
