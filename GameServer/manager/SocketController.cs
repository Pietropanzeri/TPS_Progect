using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using GameServer.model;
using GameServer.utils;
using WebSocketSharp;
using WebSocketSharp.Server;
using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

namespace GameServer.manager;

public class SocketController
{
    public GameController GameController { get; }
    public List<IWebSocketSession> connectedClients { get; } = new ();
    
    private WebSocketServer _server;
    private WebSocketServiceHost _serviceHost;

    public SocketController(GameController gameController)
    {
        GameController = gameController;

        //TODO: Carino forse da rivedere :)
        string ip = GetIpAddress().ToString();
        //_server = new WebSocketServer("ws://" + ip + ":7880");
        _server = new WebSocketServer("ws://172.17.4.249:7880");
        
        MessageUtils.Send("Connesso con ip: " + _server.Address, ConsoleColor.Magenta);
    }

    public void Start()
    {
        _server.AddWebSocketService("/", () => new SocketResponseBehavior(this));
        _server.Start();

        _serviceHost = _server.WebSocketServices["/"];
    }
    

    public void Stop()
    {
        _server.Stop();
    }

    private IPAddress GetIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        return host.AddressList.First(ip => ip.AddressFamily.Equals(AddressFamily.InterNetwork));
    }

    //Potrei passare anche player
    public void ReplyTo(string id, SocketData data)
    {
        _serviceHost.Sessions.SendTo(JsonSerializer.Serialize(data), id);
    }
    
    public class SocketResponseBehavior : WebSocketBehavior
    {
        private SocketController _socketController;

        public SocketResponseBehavior(SocketController socketController)
        {
            _socketController = socketController;
        }
        
        protected override void OnOpen()
        {
            MessageUtils.Send("Client connesso con successo! Id:" + ID, ConsoleColor.Green);
            _socketController.connectedClients.Add(this);
        }

        //Ricontrollare sta logica
        protected override void OnMessage(MessageEventArgs e)
        {
            //Get socketdata and check if is null
            SocketData? data = JsonSerializer.Deserialize<SocketData>(e.Data);
            if (data == null) return;
            
            //Call message handler and catch a response, if the response is null return
            SocketData? responseData = _socketController.GameController.SocketMessageHandler(ID, data);
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
            MessageUtils.Send("Server disconnesso con successo! Id: " + ID, ConsoleColor.Red);
            _socketController.connectedClients.Remove(this);
            _socketController.GameController.PlayerController.OnlinePlayers.Remove(ID);
        }
    }
}