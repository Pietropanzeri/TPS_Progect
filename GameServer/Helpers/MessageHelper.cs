using System.Drawing;

namespace GameServer.utils;

public class MessageHelper
{
    public static void Send(String message, ConsoleColor color)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = oldColor;
    }
}