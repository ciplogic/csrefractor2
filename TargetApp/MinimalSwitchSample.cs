namespace TargetApp;

public class MinimalSwitchSample
{
    public static void Main(string[] args)
    {
        int x = 2;
        var v2 = 3;
        if (x > v2)
        {
            Console.WriteLine(x);
        }

        if (x < 3)
        {
            Console.WriteLine(x);
        }
        else
        {
            Console.WriteLine(v2);
        }
    }
}