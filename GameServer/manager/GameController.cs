using GameServer.model;
using GameServer.utils;

namespace GameServer.manager;

public class GameController
{
    private SocketController _socketController;
    private Dictionary<int, Game> currentGame = new();

    public GameController()
    {
        _socketController = new SocketController(this);
    }

    public void Initialize()
    {
        _socketController.Start();
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