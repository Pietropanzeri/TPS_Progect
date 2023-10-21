namespace GameServer.model;

public class Game
{
    public Player[] Players { get; set; }
    public char[][] Match { get; set; }


    public char GetSymbol(Cell cell)
    {
        return Match[cell.Row][cell.Column];
    }
}