//
// BackingFieldResolver.cs
//
// Author:
//   Jb Evain (jbevain@novell.com)
//
// (C) 2009 - 2010 Novell, Inc. (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Reflection;
using System.Reflection.Emit;

namespace NativeSharp.Common;

public static class BackingFieldResolver
{
    private class FieldPattern : IlPattern
    {
        public static object FieldKey = new();

        private IlPattern pattern;

        public FieldPattern(IlPattern pattern)
        {
            this.pattern = pattern;
        }

        public override void Match(MatchContext context)
        {
            pattern.Match(context);
            if (!context.Success)
            {
                return;
            }

            Instruction match = GetLastMatchingInstruction(context);
            FieldInfo field = (FieldInfo)match.Operand;
            context.AddData(FieldKey, field);
        }
    }

    private static IlPattern Field(OpCode opcode)
    {
        return new FieldPattern(IlPattern.OpCode(opcode));
    }

    private static IlPattern _getterPattern =
        IlPattern.Sequence(
            IlPattern.Optional(OpCodes.Nop),
            IlPattern.Either(
                Field(OpCodes.Ldsfld),
                IlPattern.Sequence(
                    IlPattern.OpCode(OpCodes.Ldarg_0),
                    Field(OpCodes.Ldfld))),
            IlPattern.Optional(
                IlPattern.Sequence(
                    IlPattern.OpCode(OpCodes.Stloc_0),
                    IlPattern.OpCode(OpCodes.Br_S),
                    IlPattern.OpCode(OpCodes.Ldloc_0))),
            IlPattern.OpCode(OpCodes.Ret));

    private static IlPattern _setterPattern =
        IlPattern.Sequence(
            IlPattern.Optional(OpCodes.Nop),
            IlPattern.OpCode(OpCodes.Ldarg_0),
            IlPattern.Either(
                Field(OpCodes.Stsfld),
                IlPattern.Sequence(
                    IlPattern.OpCode(OpCodes.Ldarg_1),
                    Field(OpCodes.Stfld))),
            IlPattern.OpCode(OpCodes.Ret));

    private static FieldInfo GetBackingField(MethodInfo method, IlPattern pattern)
    {
        MatchContext result = IlPattern.Match(method, pattern);
        if (!result.Success)
        {
            throw new ArgumentException();
        }

        object value;
        if (!result.TryGetData(FieldPattern.FieldKey, out value))
        {
            throw new InvalidOperationException();
        }

        return (FieldInfo)value;
    }

    public static FieldInfo GetBackingField(this PropertyInfo self)
    {
        ArgumentNullException.ThrowIfNull(self);

        MethodInfo? getter = self.GetGetMethod(true);
        if (getter != null)
        {
            return GetBackingField(getter, _getterPattern);
        }

        MethodInfo? setter = self.GetSetMethod(true);
        if (setter != null)
        {
            return GetBackingField(setter, _setterPattern);
        }

        throw new ArgumentException();
    }
}