using Godot;
using System;

public partial class UserApp : Node
{
    [Export]
    private TextEdit addEdit;
    [Export]
    private TextEdit portEdit;
    [Export]
    private Button connectBtn;
    [Export]
    private Label temText;
    [Export]
    private Label humText;
    [Export]
    private Label lightText;
    [Export]
    private Label sensorStateText;
    public override void _Ready()
    {
        connectBtn.Pressed += OnConnectPressed;
        NetManager.AddEventListener(NetEvent.ConnectSucc, OnConnect);
        NetManager.AddEventListener(NetEvent.ConnectFail, OnConnectFail);
        NetManager.AddMsgListener("MsgForward", ReceiveTHL);
        NetManager.AddMsgListener("MsgSensorState",SensorStateChange);

    }

    private void SensorStateChange(MsgBase msgBase)
    {
            MsgSensorState msg = (MsgSensorState)msgBase;
            if(msg.SensorState==1)
            {
                sensorStateText.Text = "传感器异常";
            }
    }


    public override void _Process(double delta)
    {
        NetManager.Update();
    }

    private void ReceiveTHL(MsgBase msgBase)
    {
        MsgForward msg = (MsgForward)msgBase;
        sensorStateText.Text = "传感器正常";
        temText.Text = msg.ThlData.Temperature.ToString("F2") + " °C";
        humText.Text = msg.ThlData.Humidity.ToString("F2") + " %";
        lightText.Text = msg.ThlData.Light.ToString("F2") + " lux";
    }


    private void OnConnectFail(string err)
    {
        GD.Print(err);
    }


    private void OnConnect(string err)
    {
        
        GD.Print(err);
    }


    private void OnConnectPressed()
    {
       NetManager.Connect(addEdit.Text, Convert.ToInt32(portEdit.Text));
    }
}
