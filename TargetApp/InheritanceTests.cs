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

class Foo : BaseFoo, IFoo
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

class Bar : IFoo
{
    public int GetFoo()
    {
        return 242;
    }
}

public class InheritanceTests
{
    public static void Main()
    {
        var foo = new Foo();
        Console.WriteLine(foo.GetBasicFoo());
        Console.WriteLine(foo.GetFoo());
        var bar = new Bar();
        Console.WriteLine(bar.GetFoo());
        
    }
}