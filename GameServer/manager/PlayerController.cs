using GameServer.model;

namespace GameServer.manager;

public class PlayerController
{
    public Dictionary<string, Player> OnlinePlayers { get; set; } = new();

    public PlayerController()
    {
        
    }
}