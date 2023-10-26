namespace GameClient.model;

public class Player : Utente
{
    public string SocketID { get; set; }
    public int Points { get; set; }

    public override string ToString()
    {
        return "UserName: " + UserName + "\nPoints: " + Points;
    }

    public static Player Create()
    {
        return new Player
        {
            UserName = "Ospite",
            Points = 0,
        };
    }
}