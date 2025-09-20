using System.Text;
using NativeSharp.Operations.Vars;

namespace NativeSharp.CodeGen.Methods;

public class VariablesBulkWriter
{
    private readonly Dictionary<string, List<string>> _variables = [];

    public void Populate(IEnumerable<IndexedVariable> lines)
    {
        foreach (var line in lines)
        {
            var variableType = line.ExpressionType.Mangle();
            if (!_variables.TryGetValue(variableType, out List<string>? variableList))
            {
                variableList = new List<string>();
                _variables[variableType] = variableList;
            }

            variableList.Add(line.GenCodeImpl());
        }
    }

    public void Clear() => _variables.Clear();

    public string Write()
    {
        StringBuilder builder = new (200);
        foreach (var variable in _variables)
        {
            builder
                .Append("  ")
                .Append(variable.Key)
                .Append(' ')
                .Append(string.Join(", ", variable.Value))
                .Append(';')
                .AppendLine();
        }

        return builder.ToString();
    }
}