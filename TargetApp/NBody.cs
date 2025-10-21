internal class nbody
{
    public static void Main()
    {
        int n = 100_000_000;

        NBodySystem bodies = new NBodySystem();
        Console.WriteLine(bodies.energy());
        for (int i = 0; i < n; ++i)
            bodies.advance(0.01);
        Console.WriteLine(bodies.energy());
    }
}

internal class NBodySystem
{
    private Body[] bodies;

    public NBodySystem()
    {
        bodies = new Body[]
        {
            Body.sun(),
            Body.jupiter(),
            Body.saturn(),
            Body.uranus(),
            Body.neptune()
        };

        double px = 0.0;
        double py = 0.0;
        double pz = 0.0;
        for (int i = 0; i < bodies.Length; ++i)
        {
            px += bodies[i].Vx * bodies[i].Mass;
            py += bodies[i].Vy * bodies[i].Mass;
            pz += bodies[i].Vz * bodies[i].Mass;
        }

        bodies[0].offsetMomentum(px, py, pz);
    }

    public void advance(double dt)
    {
        AdvanceTwoLoops(dt);

        AdvanceBodiesEnergy(dt);
    }

    private void AdvanceBodiesEnergy(double dt)
    {
        foreach (Body body in bodies)
        {
            body.X += dt * body.Vx;
            body.Y += dt * body.Vy;
            body.Z += dt * body.Vz;
        }
    }

    private void AdvanceTwoLoops(double dt)
    {
        for (int i = 0; i < bodies.Length; ++i)
        {
            Body iBody = bodies[i];
            for (int j = i + 1; j < bodies.Length; ++j)
            {
                advanceInnerLoop(dt, iBody, j);
            }
        }
    }

    private void advanceInnerLoop(double dt, Body iBody, int j)
    {
        double dx = iBody.X - bodies[j].X;
        double dy = iBody.Y - bodies[j].Y;
        double dz = iBody.Z - bodies[j].Z;

        double dSquared = dx * dx + dy * dy + dz * dz;
        double distance = Math.Sqrt(dSquared);
        double mag = dt / (dSquared * distance);

        iBody.Vx -= dx * bodies[j].Mass * mag;
        iBody.Vy -= dy * bodies[j].Mass * mag;
        iBody.Vz -= dz * bodies[j].Mass * mag;

        bodies[j].Vx += dx * iBody.Mass * mag;
        bodies[j].Vy += dy * iBody.Mass * mag;
        bodies[j].Vz += dz * iBody.Mass * mag;
    }

    public double energy()
    {
        double dx, dy, dz, distance;
        double e = 0.0;

        for (int i = 0; i < bodies.Length; ++i)
        {
            Body iBody = bodies[i];
            e += 0.5 * iBody.Mass *
                 (iBody.Vx * iBody.Vx
                  + iBody.Vy * iBody.Vy
                  + iBody.Vz * iBody.Vz);

            for (int j = i + 1; j < bodies.Length; ++j)
            {
                Body jBody = bodies[j];
                dx = iBody.X - jBody.X;
                dy = iBody.Y - jBody.Y;
                dz = iBody.Z - jBody.Z;

                distance = Math.Sqrt(dx * dx + dy * dy + dz * dz);
                e -= (iBody.Mass * jBody.Mass) / distance;
            }
        }

        return e;
    }
}


internal class Body
{
    private const double PI = 3.141592653589793;
    private const double SOLAR_MASS = 4 * PI * PI;
    private const double DAYS_PER_YEAR = 365.24;

    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    
    public double Vx, Vy, Vz, Mass;

    public Body()
    {
    }

    internal static Body jupiter()
    {
        Body p = new Body();
        p.X = 4.84143144246472090e+00;
        p.Y = -1.16032004402742839e+00;
        p.Z = -1.03622044471123109e-01;
        p.Vx = 1.66007664274403694e-03 * DAYS_PER_YEAR;
        p.Vy = 7.69901118419740425e-03 * DAYS_PER_YEAR;
        p.Vz = -6.90460016972063023e-05 * DAYS_PER_YEAR;
        p.Mass = 9.54791938424326609e-04 * SOLAR_MASS;
        return p;
    }

    internal static Body saturn()
    {
        Body p = new Body();
        p.X = 8.34336671824457987e+00;
        p.Y = 4.12479856412430479e+00;
        p.Z = -4.03523417114321381e-01;
        p.Vx = -2.76742510726862411e-03 * DAYS_PER_YEAR;
        p.Vy = 4.99852801234917238e-03 * DAYS_PER_YEAR;
        p.Vz = 2.30417297573763929e-05 * DAYS_PER_YEAR;
        p.Mass = 2.85885980666130812e-04 * SOLAR_MASS;
        return p;
    }

    internal static Body uranus()
    {
        Body p = new Body();
        p.X = 1.28943695621391310e+01;
        p.Y = -1.51111514016986312e+01;
        p.Z = -2.23307578892655734e-01;
        p.Vx = 2.96460137564761618e-03 * DAYS_PER_YEAR;
        p.Vy = 2.37847173959480950e-03 * DAYS_PER_YEAR;
        p.Vz = -2.96589568540237556e-05 * DAYS_PER_YEAR;
        p.Mass = 4.36624404335156298e-05 * SOLAR_MASS;
        return p;
    }

    internal static Body neptune()
    {
        Body p = new Body();
        p.X = 1.53796971148509165e+01;
        p.Y = -2.59193146099879641e+01;
        p.Z = 1.79258772950371181e-01;
        p.Vx = 2.68067772490389322e-03 * DAYS_PER_YEAR;
        p.Vy = 1.62824170038242295e-03 * DAYS_PER_YEAR;
        p.Vz = -9.51592254519715870e-05 * DAYS_PER_YEAR;
        p.Mass = 5.15138902046611451e-05 * SOLAR_MASS;
        return p;
    }

    internal static Body sun()
    {
        Body p = new Body();
        p.Mass = SOLAR_MASS;
        return p;
    }

    internal Body offsetMomentum(double px, double py, double pz)
    {
        Vx = -px / SOLAR_MASS;
        Vy = -py / SOLAR_MASS;
        Vz = -pz / SOLAR_MASS;
        return this;
    }
}