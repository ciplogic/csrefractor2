using NativeSharp.Common;

namespace NativeSharp.CodeGen;

public class MainMethodCodeGenerator
{
    public void WriteMainMethodBody(CodeGenToFile Code, string mainMethod, TimingMainKind timingMainKind,
        string argumentsVar = "")
    {
        Code
            .AddLine("#include \"native_sharp.cpp\"");
        Code.AddLine("int main(int argc, char**argv) {");
        if (!string.IsNullOrEmpty(argumentsVar))
        {
            Code.AddLine("auto ARGS = argsToStrings(argc, argv);");
        }

        string lineOfCode = timingMainKind switch
        {
            TimingMainKind.None => $"{LambdaBodyMethod()}();",
            TimingMainKind.Millisecond => $"timeItMilliseconds({LambdaBodyMethod()});",
            TimingMainKind.Microsecond => $"timeItMicroseconds({LambdaBodyMethod()});",
            TimingMainKind.Nanosecond=> $"timeItNanoseconds({LambdaBodyMethod()});",
            _ => throw new ArgumentOutOfRangeException(nameof(timingMainKind), timingMainKind, null)
        };
        Code.AddLine(lineOfCode);
        Code.AddLine("return 0;")
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