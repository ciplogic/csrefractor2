using System.Diagnostics;

namespace TargetApp;

class Program
{
    private static bool IsPrime(int number)
    {
        if (number <= 4)
        {
            return true;
        }

        if ((number & 1) == 0)
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

    static void Main()
    {
        var sw = Stopwatch.StartNew();
        var primeCount = GetPrimeCount(10_000_000);

        Console.WriteLine(primeCount);
        sw.Stop();
        Console.WriteLine(sw.Elapsed);
    }

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

    static void Main_()
    {
        bool isTrue = false;
        var trueText = isTrue.ToString();
        Console.WriteLine("Hello...");
        Console.WriteLine(trueText);
    }
}