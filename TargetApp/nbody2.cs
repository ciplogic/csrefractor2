internal class Nbody2
{
    public static void Main3()
    {
        int n = 10000;

        Bodies bodies = new Bodies();
        for (int i = 0; i < n; i++)
        {
            bodies.Advance(0.01);
        }
        Console.WriteLine(bodies.Energy);
    }
}

internal class Bodies
{
    public double Energy;

    public void Advance(double d)
    {
        Energy += d;
    }
}