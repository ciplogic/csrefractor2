using NativeSharp.Common;

namespace NativeSharp.CodeGen;

public class MainMethodCodeGenerator
{
    public void WriteMainMethodBody(CodeGenToFile code, string mainMethod, TimingMainKind timingMainKind,
        string argumentsVar = "")
    {
        code
            .AddLine("#include \"native_sharp.cpp\"");
        code.AddLine("int main(int argc, char**argv) {");
        if (!string.IsNullOrEmpty(argumentsVar))
        {
            code.AddLine("auto ARGS = argsToStrings(argc, argv);");
        }

        string lineOfCode = timingMainKind switch
        {
            TimingMainKind.None => $"{LambdaBodyMethod()}();",
            TimingMainKind.Millisecond => $"timeItMilliseconds({LambdaBodyMethod()});",
            TimingMainKind.Microsecond => $"timeItMicroseconds({LambdaBodyMethod()});",
            TimingMainKind.Nanosecond=> $"timeItNanoseconds({LambdaBodyMethod()});",
            _ => throw new ArgumentOutOfRangeException(nameof(timingMainKind), timingMainKind, null)
        };
        code.AddLine(lineOfCode);
        code.AddLine("return 0;")
            .AddLine("}");

        string LambdaBodyMethod()
        {
            if (string.IsNullOrEmpty(argumentsVar))
            {
                return mainMethod;
            }
            return $"[&]{{ {mainMethod} (ARGS); }}";
        }
    }
    
}