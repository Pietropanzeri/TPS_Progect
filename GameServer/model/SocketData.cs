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
    public String UserName { get; set; }
    public String Data { get; set; }

    public String[] SplitData()
    {
        return Data.Split(":");
    }
}

public enum DataType
{
    BroadCast,
    Connect,
    Disconnect,
    Join,
    Quit
}