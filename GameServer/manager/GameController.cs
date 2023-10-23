using System.Text.Json;
using GameServer.model;
using GameServer.utils;
using WebSocketSharp;

namespace GameServer.manager;

public class GameController
{
    private SocketController _socketController;
    private DatabaseController _databaseController;
    
    private Dictionary<int, Game> currentGame = new();

    public GameController()
    {
        _socketController = new SocketController(this);
        _databaseController = new DatabaseController(this);
    }

    public void Initialize()
    {
        _socketController.Start();
        Console.ReadKey();
        _socketController.Stop();
    }

    //Return null if you don't want to reply
    public SocketData? SocketMessageHandler(string id, SocketData data)
    {
        switch (data.DataType)
        {
            case DataType.Join:
                //TODO: Spostare in un metodo a parte e fare tutti i check se l'utente e' gia' connesso
                _databaseController.LoadPlayer(data.UserName).ContinueWith(task =>
                {
                    //TODO: Il player contiene anche socketId vedere come caricarlo
                    Player player = task.Result;
                    _socketController.ReplyTo(id, new SocketData(DataType.Connect,"Server", JsonSerializer.Serialize(player)));
                });
                break;
            case DataType.Quit:
                MessageUtils.Send("Messaggio ricevuto sul quit", ConsoleColor.Yellow);
                break;
        }

        return null;
    }
}