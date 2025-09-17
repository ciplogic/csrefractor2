class nbody2
{
    public static void Mai3()
    {
        int n = 10000;

        Bodies bodies = new Bodies();
        for (int i = 0; i < n; i++)
        {
            bodies.advance(0.01);
        }
        Console.WriteLine(bodies.energy);
    }
}

class Bodies
{
    public double energy;

    public void advance(double d)
    {
        energy += d;
    }
}