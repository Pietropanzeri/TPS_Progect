namespace GameServer.model;

public class SocketData
{
    public SocketData(DataType dataType, string userName, string data)
    {
        DataType = dataType;
        UserName = userName;
        Data = data;
    }

    public DataType DataType { get; set; }
    public string UserName { get; set; }
    public string Data { get; set; }

    public string[] SplitData()
    {
        return Data.Split(":");
    }
}

public enum DataType
{
    BroadCast,
    Connect,
    Disconnect,
    MatchMaking,
    Move,
    JoinGame,
    QuitGame
}