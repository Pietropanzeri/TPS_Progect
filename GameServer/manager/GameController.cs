using GameServer.model;
using GameServer.utils;

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

        _databaseController.LoadPlayer("Marcolino").ContinueWith(task =>
        {
            MessageUtils.Send("L'utente è stato creato", ConsoleColor.Green);
        });
        
        Console.ReadKey();
        _socketController.Stop();
    }

    //Return null if you don't want to reply
    public SocketData? SocketMessageHandler(SocketData data)
    {
        switch (data.DataType)
        {
            case DataType.Join:
                MessageUtils.Send("Messaggio arrivato con join", ConsoleColor.Magenta);
                return new SocketData(DataType.BroadCast, "UwU", data.Data);
            case DataType.Quit:
                MessageUtils.Send("Messaggio ricevuto sul quit", ConsoleColor.Yellow);
                break;
        }

        return null;
    }
}