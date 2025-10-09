//
// ByteBuffer.cs
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

namespace NativeSharp.Common;

internal class ByteBuffer
{
    internal readonly byte[] Buffer;
    internal int Position;

    public ByteBuffer(byte[] buffer) => this.Buffer = buffer;

    public byte ReadByte()
    {
        CheckCanRead(1);
        return Buffer[Position++];
    }

    public byte[] ReadBytes(int length)
    {
        CheckCanRead(length);
        byte[] value = new byte [length];
        Array.Copy(Buffer, Position, value, 0, length);
        Position += length;
        return value;
    }

    public short ReadInt16()
    {
        CheckCanRead(2);
        short value = (short)(Buffer[Position]
                              | (Buffer[Position + 1] << 8));
        Position += 2;
        return value;
    }

    public int ReadInt32()
    {
        CheckCanRead(4);
        int value = Buffer[Position]
                    | (Buffer[Position + 1] << 8)
                    | (Buffer[Position + 2] << 16)
                    | (Buffer[Position + 3] << 24);
        Position += 4;
        return value;
    }

    public long ReadInt64()
    {
        CheckCanRead(8);
        uint low = (uint)(Buffer[Position]
                          | (Buffer[Position + 1] << 8)
                          | (Buffer[Position + 2] << 16)
                          | (Buffer[Position + 3] << 24));

        uint high = (uint)(Buffer[Position + 4]
                           | (Buffer[Position + 5] << 8)
                           | (Buffer[Position + 6] << 16)
                           | (Buffer[Position + 7] << 24));

        long value = ((long)high << 32) | low;
        Position += 8;
        return value;
    }

    public float ReadSingle()
    {
        if (!BitConverter.IsLittleEndian)
        {
            byte[] bytes = ReadBytes(4);
            Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }

        CheckCanRead(4);
        float value = BitConverter.ToSingle(Buffer, Position);
        Position += 4;
        return value;
    }

    public double ReadDouble()
    {
        if (!BitConverter.IsLittleEndian)
        {
            byte[] bytes = ReadBytes(8);
            Array.Reverse(bytes);
            return BitConverter.ToDouble(bytes, 0);
        }

        CheckCanRead(8);
        double value = BitConverter.ToDouble(Buffer, Position);
        Position += 8;
        return value;
    }

    private void CheckCanRead(int count)
    {
        if (Position + count > Buffer.Length)
        {
            throw new ArgumentOutOfRangeException();
        }
    }
}