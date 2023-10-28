using System.Text.Json.Serialization;
using WebSocketSharp;

namespace GameServer.model;

public class Game
{
    public String Id { get; set; } = Guid.NewGuid().ToString();
    public Player[] Players { get; set; }
    public List<Cell> GameField { get; set; } = new();
    
    [JsonIgnore]
    public Player CurrentUser { get; set; }
    
    public bool Side { get; set; }
    
    [JsonIgnore]
    public List<int[]> WinPossibilities { get; set; } = new List<int[]>();

    public Game(Player[] players)
    {
        Players = players;
        
        if (Side)
        {
            CurrentUser = Players[0];
            Players[0].Symbol = "X";
            Players[1].Symbol = "O";
        }
        else
        {
            CurrentUser = Players[1];
            Players[0].Symbol = "O";
            Players[1].Symbol = "X";
        }
        
        WinPossibilities.Add(new[] { 0, 1, 2 });
        WinPossibilities.Add(new[] { 3, 4, 5 });
        WinPossibilities.Add(new[] { 6, 7, 8 });
        WinPossibilities.Add(new[] { 0, 3, 6 });
        WinPossibilities.Add(new[] { 1, 4, 7 });
        WinPossibilities.Add(new[] { 2, 5, 8 });
        WinPossibilities.Add(new[] { 0, 4, 8 });
        WinPossibilities.Add(new[] { 2, 4, 6 });
        
        GameField.Add(new Cell() { Position = 0 });
        GameField.Add(new Cell() { Position = 1 });
        GameField.Add(new Cell() { Position = 2 });
        GameField.Add(new Cell() { Position = 3 });
        GameField.Add(new Cell() { Position = 4 });
        GameField.Add(new Cell() { Position = 5 });
        GameField.Add(new Cell() { Position = 6 });
        GameField.Add(new Cell() { Position = 7 });
        GameField.Add(new Cell() { Position = 8 });
    }

    public Cell? MakeMove(string socketId, Cell cell)
    {
        if (CurrentUser.SocketId != socketId) return null;
        if (!GameField[cell.Position].Content.IsNullOrEmpty()) return null;
        GameField[cell.Position].Content = CurrentUser.Symbol;

        updatePhase();
        
        return GameField[cell.Position];
        //TODO: Check vittoria pareggio
    }
    
    private bool updatePhase()
    {
        Side = Side;
        CurrentUser = Side ? Players[0] : Players[1];

        return Side;
    }
    
    public bool CheckWin(string symbol)
    {
        var playerIndex = GameField.Where(c => c.Content == symbol).Select(c =>  c.Position).ToList();
        int n = 0;
        for (int i = 0; i < WinPossibilities.Count; i++)
        {
            foreach (var index in playerIndex)
            {
                if (WinPossibilities[i].Contains(index)) n++;
                if (n >= 3) return true;
            }
            n = 0;
        }

        return false;
    }
    public bool CheckDraw()
    {
        int n = 0;
        foreach (var item in GameField)
        {
            if (item.Content.IsNullOrEmpty()) n++;
        }
        return (n == 0);
    }
}