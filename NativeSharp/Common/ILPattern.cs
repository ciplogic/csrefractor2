//
// ILPattern.cs
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

public abstract class IlPattern
{
    public static IlPattern Optional(OpCode opcode)
    {
        return Optional(OpCode(opcode));
    }

    public static IlPattern Optional(params OpCode[] opcodes)
    {
        return Optional(Sequence(opcodes.Select(opcode => OpCode(opcode)).ToArray()));
    }

    public static IlPattern Optional(IlPattern pattern)
    {
        return new OptionalPattern(pattern);
    }

    private class OptionalPattern : IlPattern
    {
        private IlPattern pattern;

        public OptionalPattern(IlPattern optional)
        {
            pattern = optional;
        }

        public override void Match(MatchContext context)
        {
            pattern.TryMatch(context);
        }
    }

    public static IlPattern Sequence(params IlPattern[] patterns)
    {
        return new SequencePattern(patterns);
    }

    private class SequencePattern : IlPattern
    {
        private IlPattern[] patterns;

        public SequencePattern(IlPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public override void Match(MatchContext context)
        {
            foreach (IlPattern pattern in patterns)
            {
                pattern.Match(context);

                if (!context.Success)
                {
                    break;
                }
            }
        }
    }

    public static IlPattern OpCode(OpCode opcode)
    {
        return new OpCodePattern(opcode);
    }

    private class OpCodePattern : IlPattern
    {
        private OpCode opcode;

        public OpCodePattern(OpCode opcode)
        {
            this.opcode = opcode;
        }

        public override void Match(MatchContext context)
        {
            if (context.Instruction == null)
            {
                context.Success = false;
                return;
            }

            context.Success = context.Instruction.OpCode == opcode;
            context.Advance();
        }
    }

    public static IlPattern Either(IlPattern a, IlPattern b)
    {
        return new EitherPattern(a, b);
    }

    private class EitherPattern : IlPattern
    {
        private IlPattern a;
        private IlPattern b;

        public EitherPattern(IlPattern a, IlPattern b)
        {
            this.a = a;
            this.b = b;
        }

        public override void Match(MatchContext context)
        {
            if (!a.TryMatch(context))
            {
                b.Match(context);
            }
        }
    }

    public abstract void Match(MatchContext context);

    protected static Instruction GetLastMatchingInstruction(MatchContext context)
    {
        if (context.Instruction == null)
        {
            return null;
        }

        return context.Instruction.Previous;
    }

    public bool TryMatch(MatchContext context)
    {
        Instruction instruction = context.Instruction;
        Match(context);

        if (context.Success)
        {
            return true;
        }

        context.Reset(instruction);
        return false;
    }

    public static MatchContext Match(MethodBase method, IlPattern pattern)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(pattern);

        IList<Instruction> instructions = method.GetInstructions();
        if (instructions.Count == 0)
        {
            throw new ArgumentException();
        }

        MatchContext context = new MatchContext(instructions[0]);
        pattern.Match(context);
        return context;
    }
}

public sealed class MatchContext
{
    internal Instruction Instruction;
    internal bool Success;

    private Dictionary<object, object> data = new();

    public bool IsMatch
    {
        get => Success;
        set => Success = true;
    }

    internal MatchContext(Instruction instruction)
    {
        Reset(instruction);
    }

    public bool TryGetData(object key, out object value)
    {
        return data.TryGetValue(key, out value);
    }

    public void AddData(object key, object value)
    {
        data.Add(key, value);
    }

    internal void Reset(Instruction instruction)
    {
        this.Instruction = instruction;
        Success = true;
    }

    internal void Advance()
    {
        Instruction = Instruction.Next;
    }
}