namespace GameServer.model;

public class Cell
{
    public int Column { get; set; }
    public int Row { get; set; }

    public static Cell Create(int col, int row)
    {
        return new Cell { Column = col, Row = row };
    }
}