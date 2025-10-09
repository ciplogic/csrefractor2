namespace NativeSharp.Operations.BranchOperations;

internal static class CompactNumberWriter
{
    private static char[] _chars = BuildCharsTable();

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
        return offset.ToString();
        if (offset == 0)
        {
            return "0";
        }

        var chars = new List<char>();
        var len = _chars.Length;
        while (offset > 0)
        {
            var reminder =  offset % len;
            chars.Add(_chars[reminder]);
            offset /= len;
        }
        var charArray = chars.ToArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}