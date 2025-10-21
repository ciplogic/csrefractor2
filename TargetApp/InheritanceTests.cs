using System.Text;

namespace TargetApp;

public interface IFoo
{
    int GetFoo();
}

public abstract class BaseFoo
{
    public abstract int GetBasicFoo();
}

internal class Foo : BaseFoo, IFoo
{
    public override int GetBasicFoo()
    {
        return 42;
    }

    public int GetFoo()
    {
        return 43;
    }
}

internal class Bar : IFoo
{
    public int GetFoo()
    {
        return 242;
    }
}

public class InheritanceTests
{
    public static void MainInherited()
    {
        Foo foo = new Foo();
        Console.WriteLine(foo.GetBasicFoo());
        Console.WriteLine(foo.GetFoo());
        Bar bar = new Bar();
        Console.WriteLine(bar.GetFoo());
        
    }
}