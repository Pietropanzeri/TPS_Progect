using System.Text.Json;
using GameServer.model;
using WebSocketSharp;

namespace GameClient.Controller;

public class SocketController
{
    public WebSocket SocketClient { get; }
    private Stack<Action<SocketData>> actionStack = new();

    public SocketController()
    {
        SocketClient = new WebSocket("ws://192.168.1.53:7880/");
        SocketClient.OnMessage += OnMessage;
    }

    public async void Start()
    {
        try
        {
            SocketClient.Connect();
        }
        catch (Exception e) { }
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

}