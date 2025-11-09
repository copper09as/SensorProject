using Godot;
using System;

public partial class UserApp : Node
{

    [Export]
    private Label temText;
    [Export]
    private Label humText;
    [Export]
    private Label lightText;
    [Export]
    private Label sensorStateText;
    [Export]
    private TextureButton btnReConnect;
    [Export]
    private Label signalText;
    [Export]
    private Label analyText;
    public override void _Ready()
    {
        NetManager.AddEventListener(NetEvent.ConnectSucc, OnConnect);
        NetManager.AddEventListener(NetEvent.ConnectFail, OnConnectFail);
        NetManager.AddEventListener(NetEvent.Close, OnClose);
        NetManager.AddMsgListener("MsgForward", ReceiveTHL);
        NetManager.AddMsgListener("MsgSensorState", SensorStateChange);
        NetManager.AddMsgListener("MsgThlAnaly", DisplayAnalyResult);
        btnReConnect.Pressed += OnConnectPressed;
        NetManager.Connect("60.215.128.110", 43195);
    }

    private void DisplayAnalyResult(MsgBase msgBase)
    {
        MsgThlAnaly msg = (MsgThlAnaly)msgBase;
        analyText.Text = msg.AnalyResult;
    }


    private void SensorStateChange(MsgBase msgBase)
    {
        MsgSensorState msg = (MsgSensorState)msgBase;
        if (msg.SensorState == 1)
        {
            sensorStateText.Text = "传感器异常";
            temText.Text = "-- °C";
            humText.Text = "-- %";
            lightText.Text = "-- lux";
            analyText.Text = "";
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
        CallDeferred(nameof(UpdateUIAfterConnect));
    }
    private void UpdateUIAfterConnect()
    {
        signalText.Text = "服务器异常";
    }

    private void OnConnect(string err)
    {

        CallDeferred(nameof(UpdateUIAfterConnectSucc));
    }
    private void OnClose(string err)
    {

       CallDeferred(nameof(UpdateUIAfterConnect));
    }
    private void UpdateUIAfterConnectSucc()
    {

        signalText.Text = "连接服务器";
    }

    private void OnConnectPressed()
    {
        NetManager.Connect("60.215.128.110", 43195);
    }
}
