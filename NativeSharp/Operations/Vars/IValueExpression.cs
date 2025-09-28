namespace NativeSharp.Operations.Vars;

public interface IValueExpression
{
    public Type ExpressionType { get; set; }
    string Code();
}

