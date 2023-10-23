namespace GameServer.model;

public class Game
{
    public Player[] Players { get; set; }
    public List<Cell> GameField { get; set; } = new();
}