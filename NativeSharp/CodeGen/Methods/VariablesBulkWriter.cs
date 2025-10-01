using System.Text;
using NativeSharp.EscapeAnalysis;
using NativeSharp.Operations.Vars;

namespace NativeSharp.CodeGen.Methods;

public class VariablesBulkWriter
{
    private readonly Dictionary<string, List<string>> variables = [];

    public void Populate(IEnumerable<IndexedVariable> enumerateVariables)
    {
        foreach (var variable in enumerateVariables)
        {
            var variableType = variable.ExpressionType.Mangle(variable.EscapeResult);
            if (!this.variables.TryGetValue(variableType, out List<string>? variableList))
            {
                variableList = [];
                this.variables[variableType] = variableList;
            }

            var prefix = variable.EscapeResult == EscapeKind.Local && !variable.ExpressionType.IsValueType
                ? "*"
                : "";
            if (variableList.Count > 0)
            {
                variableList.Add(prefix + variable.GenCodeImpl());
            }
            else
            {
                variableList.Add(variable.GenCodeImpl());
            }
        }
    }

    public void Clear() => variables.Clear();

    public string Write()
    {
        StringBuilder builder = new(200);
        foreach (var variable in variables)
        {
            builder
                .Append("  ")
                .Append(variable.Key)
                .Append(' ')
                .Append(string.Join(',', variable.Value))
                .Append(';')
                .AppendLine();
        }

        return builder.ToString();
    }
}