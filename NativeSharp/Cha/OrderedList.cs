namespace NativeSharp.Cha;

public class OrderedList<T>
{
    public Dictionary<T, int> Index { get; } = [];
    public List<T> Items { get; } = [];

    public int Set(T item)
    {
        if (Index.TryGetValue(item, out var index))
        {
            Index.Add(item, Items.Count);
            Items.Add(item);
            return Items.Count-1;
        }

        return index;
    }

    public int Get(T item)
    {
        if (Index.TryGetValue(item, out var index))
        {
            return index;
        }
        return -1;
    }
}