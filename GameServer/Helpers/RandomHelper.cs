namespace GameClient.Helpers;

public class RandomHelper
{
    private static Random _random = new Random();

    public static bool RandomBool()
    {
        return _random.Next(0, 2) == 0;
    }
}