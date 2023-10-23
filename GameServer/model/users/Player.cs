namespace GameServer.model;

public class Player : Utente
{
    public string SocketId { get; set; }
    public string UserName { get; set; }
    public int Points { get; set; }

    public override string ToString()
    {
        return "UserName: " + UserName + "\nPoints: " + Points;
    }
}