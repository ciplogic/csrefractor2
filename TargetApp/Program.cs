using System.Diagnostics;
using NativeSharp.Lib.Resolvers;

namespace TargetApp;

class Program
{
    private static bool IsPrime(int number)
    {
        if (number <= 4)
        {
            return true;
        }

        if ((number % 2) == 0)
        {
            return false;
        }

        for (int i = 3; i * i <= number; i += 2)
        {
            if (number % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    [PureMethod]
    private static int GetPrimeCount(int rangeValue)
    {
        var primeCount = 0;
        for (var i = 0; i < rangeValue; i++)
        {
            if (IsPrime(i))
            {
                primeCount++;
            }
        }

        return primeCount;
    }

    static void MainPri()
    {
        var primeCount = GetPrimeCount(10_000_000);

        Console.WriteLine(primeCount);
    }
    
    static void MainSimple()
    {
        bool isTrue = false;
        var trueText = isTrue.ToString();
        Console.WriteLine("Hello...");
        Console.WriteLine(trueText);
    }
}
