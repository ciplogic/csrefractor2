namespace NativeSharp.Cha;

public class OrderedList<T>
{
    public Dictionary<T, int> Index { get; } = [];
    public List<T> Items { get; } = [];

    public int Set(T item)
    {
        if (!Index.TryGetValue(item, out int index))
        {
            return index;
        }

        Index.Add(item, Items.Count);
        Items.Add(item);
        return Items.Count - 1;
    }

    public int Get(T item)
        => Index.GetValueOrDefault(item, -1);
}