using System.Text.Json;
using GameServer.model;
using GameServer.utils;
using WebSocketSharp;

namespace GameServer.manager;

public class GameController
{
    private SocketController _socketController;
    private DatabaseController _databaseController;
    public PlayerController PlayerController { get; }
    
    private Dictionary<string, Game> currentGame = new();
    public List<string> MatchMaking { get; set; } = new();

    public GameController()
    {
        _socketController = new SocketController(this);
        _databaseController = new DatabaseController(this);
        PlayerController = new PlayerController();
    }

    public void Initialize()
    {
        _socketController.Start();
        Console.ReadKey();
        _socketController.Stop();
    }

    private void StartGame(Game game)
    {
        currentGame.Add(game.Id, game);
        
        foreach (var gamePlayer in game.Players)
        {
            Player player = gamePlayer;
            
            _socketController.ReplyTo(
                player.SocketId, 
                new SocketData(DataType.JoinGame, "Server", JsonSerializer.Serialize(game))
            );
        }
        
        //Gestire mosse
    }

    //Return null if you don't want to reply
    public SocketData? SocketMessageHandler(string id, SocketData data)
    {
        switch (data.DataType)
        {
            case DataType.Connect:
                if (PlayerController.OnlinePlayers.Values.Any(player => player.UserName == data.UserName))
                    return new SocketData(DataType.Error, "Server", "Esiste gia' un utente online con quel nome");
                
                //TODO: Spostare in un metodo a parte e fare tutti i check se l'utente e' gia' connesso
                _databaseController.LoadPlayer(id, data.UserName).ContinueWith(task =>
                {
                    //TODO: Il player contiene anche socketId vedere come caricarlo
                    Player player = task.Result;
                    _socketController.ReplyTo(id, new SocketData(DataType.Connect,"Server", JsonSerializer.Serialize(player)));
                    
                    PlayerController.OnlinePlayers.Add(id, player);
                });
                break;
            case DataType.MatchMaking:
                MatchMakingHandler(id, data);
                break;
            case DataType.Move:
                MoveHandler(id, data);
                break;
            case DataType.Disconnect:
                //TODO: Togliere dal matchmaking ecc (preferibilmente nel disconnect)
                MessageHelper.Send("Messaggio ricevuto sul disconnect", ConsoleColor.Yellow);
                break;
            case DataType.Top:
                _databaseController.GetPlayerTop().ContinueWith(result =>
                {
                    _socketController.ReplyTo(id,
                            new SocketData(
                                DataType.Top,
                                "Server",
                                JsonSerializer.Serialize(result.Result)
                        )
                    );
                });
                break;
        }

        return null;
    }

    private void MoveHandler(string id, SocketData data)
    {
        if (data.Data.IsNullOrEmpty()) return; //Rispondere con errore e quindi non fa la mossa
        
        string[] splitted = data.SplitData();
        
        string gameId = splitted[0];
        Cell cell = JsonSerializer.Deserialize<Cell>(splitted[1]);

        Game game = currentGame[gameId];
        
        //TODO: Per piu' sicurezza magari controlla anche l'id della socket
        Cell updatedCell = game.MakeMove(id, cell);
        if (updatedCell == null)
        {
            //Se ritorna falso dare errore etc..
            return;
        }

        Player otherPlayer = game.Players[0].SocketId == id ? game.Players[1] : game.Players[0];
        
        _socketController.ReplyTo(
            otherPlayer.SocketId,
            new SocketData(DataType.Move, "Server", JsonSerializer.Serialize(updatedCell))
        );
        
        bool hasWon = game.CheckWin(game.CurrentUser.Symbol);
        
        if (hasWon)
        {
            game.CurrentUser.Points++;
            if (game.CurrentUser.Id == game.Players[0].Id) game.GamePoints[0]++;
            else game.GamePoints[1]++;
            
            _databaseController.UpdatePoints(game.CurrentUser.Id, game.CurrentUser.Points);
        }

        if (hasWon || game.CheckDraw())
        {
            var newGame = game.ResetGame();
            currentGame[game.Id] = newGame;
            
            foreach (var player in game.Players)
            {
                _socketController.ReplyTo(
                    player.SocketId,
                    new SocketData(DataType.Restart, "Server", JsonSerializer.Serialize(newGame))
                );
            }
        }
        
        game.updatePhase();
    }
    
    private void MatchMakingHandler(string id, SocketData data)
    {
        if (MatchMaking.Any(socket => socket == id)) return;
        MatchMaking.Add(id);
        if (MatchMaking.Count < 2) return;

        StartGame(
            new Game(new[]
            {
                PlayerController.OnlinePlayers[MatchMaking[0]],
                PlayerController.OnlinePlayers[MatchMaking[1]]
            })
        );
        
        MatchMaking.RemoveRange(0, 2);
    }
}