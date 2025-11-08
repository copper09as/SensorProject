using System.Collections;
using System.Collections.Generic;
using Models;


public class MsgSensorState : MsgBase
{
    public MsgSensorState()
    {
        protoName = "MsgSensorState";
    }
    public int SensorState;
}
