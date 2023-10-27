namespace GameClient.model;

public class Player : Utente
{
    public string SocketId { get; set; }

    public override string ToString()
    {
        return "UserName: " + UserName; ;
    }

    public static Player Create()
    {
        return new Player
        {
            UserName = "Ospite"
        };
    }
}