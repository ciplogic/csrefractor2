namespace NativeSharp.Operations.BranchOperations;

static class CompactNumberWriter
{
    private static char[] Chars = BuildCharsTable();

    private static char[] BuildCharsTable()
    {
        List<char> list = [];
        for (char i = '0'; i <= '9'; i++)
        {
            list.Add(i);
        }
        for (char i = 'a'; i <= 'z'; i++)
        {
            list.Add(i);
        }
        for (char i = 'A'; i <= 'Z'; i++)
        {
            list.Add(i);
        }

        return list.ToArray();
    }

    public static string Str(int offset)
    {
        if (offset == 0)
        {
            return "0";
        }

        var chars = new List<char>();
        var len = Chars.Length;
        while (offset > 0)
        {
            var reminder =  offset % len;
            chars.Add(Chars[reminder]);
            offset /= len;
        }
        var charArray = chars.ToArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}