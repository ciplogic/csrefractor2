namespace TargetApp;

public class MinimalSwitchSample
{
    public static void Main(string[] args)
    {
        int x = 2;
        switch (x)
        {
            case 0:
                Console.WriteLine("x");
                break;
            case 1:
                Console.WriteLine("abc");
                break;
            case 2:
                Console.WriteLine("maybe it is true");
                break;
        }
    }
    public static void Main3(string[] args)
    {
        int x = 2;
        if (x == 0)
        {
            Console.WriteLine("x");
        }
        else if (x == 1)
        {
            Console.WriteLine("abc");
        }
        else if (x == 2)
        {
            Console.WriteLine("maybe it is true");
        }
    }
}