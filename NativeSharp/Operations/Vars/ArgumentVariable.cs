namespace NativeSharp.Operations.Vars;

public class ArgumentVariable : IndexedVariable
{
    public string Name { get; set; } = string.Empty;

    public override string GenCodeImpl()
    {
        return string.IsNullOrEmpty(Name)
            ? $"arg_{Index}"
            : Name;
    }
}