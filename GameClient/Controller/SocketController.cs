using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using GameServer.model;
using WebSocketSharp;

namespace GameClient.Controller;

public partial class SocketController : ObservableObject
{
    private Timer _timer;
    private int _retry;
    
    public WebSocket SocketClient { get; }
    private Stack<Action<SocketData>> actionStack = new();

    [ObservableProperty]
    private bool isConnected;

    public SocketController()
    {
        SocketClient = new WebSocket("ws://192.168.1.53:7880/");
        SocketClient.OnOpen += OnOpen;
        SocketClient.OnClose += OnClose;
        SocketClient.OnMessage += OnMessage;
    }
    
    public async void Start()
    {
        Task.Run(() =>
        {
            try
            {
                SocketClient.Connect();
            }
            catch (Exception e)
            {
            }
        });
    }

    public void Send(SocketData data, Action<SocketData> resultAction)
    {
        SocketClient.Send(JsonSerializer.Serialize(data));
        actionStack.Push(resultAction);
    }
    
    private void OnMessage(object sender, MessageEventArgs e)
    {
        SocketData data = JsonSerializer.Deserialize<SocketData>(e.Data);
        if (data == null) return;

        if(!data.DataType.Equals(DataType.Move)) 
        {
            Action<SocketData> action = actionStack.Pop();
            if (action == null) return;
            action.Invoke(data);
        }
    }
    private void OnOpen(object sender, EventArgs e)
    {
        IsConnected = true;
        _retry = 0;
    }
    
    
    private void OnClose(object sender, CloseEventArgs e)
    {
        IsConnected = false;

        if (e.WasClean) return;
        if (SocketClient.IsAlive) return;
        
        if (_retry < 5)
        {
            _retry++;
            Thread.Sleep(5000);
            SocketClient.Connect();
        }
    }

}