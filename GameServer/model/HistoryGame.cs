using GameServer.model;

namespace GameClient.Model;

public class HistoryGame
{
    public String Player1 { get; set; }
    public String Player2 {get; set;}
    public String Winner {get; set;}
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}