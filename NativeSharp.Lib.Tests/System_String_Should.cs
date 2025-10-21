using NativeSharp.Common;
using NativeSharp.Lib.System;
using Shouldly;

namespace NativeSharp.Lib.Tests;

public class System_String_Should
{
    
    [Fact]
    public void Concat()
    {
        System_String text1 = "Hello ".ToSystemString();
        System_String text2 = "World".ToSystemString();
        System_String text3 = ResolvedMethods.System_String_Concat(text1, text2);
        string decoded = text3.ToClrString();
        decoded.ShouldBe("Hello World");
        
    }
}