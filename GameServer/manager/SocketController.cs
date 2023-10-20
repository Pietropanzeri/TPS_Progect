using System.Text.Json;
using GameServer.model;
using GameServer.utils;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace GameServer.manager;

public class SocketController
{
    public GameController _gameController { get; }
    private WebSocketServer _server;

    public SocketController(GameController gameController)
    {
        _gameController = gameController;
        _server = new WebSocketServer("ws://127.0.0.1:7880");
    }

    public void Start()
    {
        _server.AddWebSocketService<SocketResponse>("/", () => new SocketResponse(_gameController));
        _server.Start();
    }

    public void Stop()
    {
        _server.Stop();
    }
    
    private class SocketResponse : WebSocketBehavior
    {
        private GameController _gameController;

        public SocketResponse(GameController gameController)
        {
            _gameController = gameController;
        }
        
        protected override void OnOpen()
        {
            MessageUtils.Send("Server acceso con successo!", ConsoleColor.Green);
        }

        //Ricontrollare sta logica
        protected override void OnMessage(MessageEventArgs e)
        {
            //Get socketdata and check if is null
            SocketData? data = JsonSerializer.Deserialize<SocketData>(e.Data);
            if (data == null) return;
            
            //Call message handler and catch a response, if the response is null return
            SocketData? responseData = _gameController.SocketMessageHandler(data);
            if (responseData == null) return;

            //Check the response type if is broadcast send a broadcast message
            if (responseData.DataType.Equals(DataType.BroadCast))
            {
                Sessions.Broadcast(JsonSerializer.Serialize(responseData));
                return;
            }
            Send(JsonSerializer.Serialize(responseData));
        }

        protected override void OnClose(CloseEventArgs e)
        {
            MessageUtils.Send("Server spento con successo!", ConsoleColor.Red);
        }
    }
}