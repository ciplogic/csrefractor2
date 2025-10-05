﻿namespace NativeSharp.Common;

public class TwoWayDictionary<T> where T : notnull
{
    public Dictionary<T, T?> Straight { get; } = [];
    public Dictionary<T, T> Reverse { get; } = [];

    public void Add(T key, T value)
    {
        Straight[key] = value;
        Reverse[value] = key;
    }

    public bool ContainsKey(T key) => Straight.ContainsKey(key);
    public bool ContainsValue(T value) => Reverse.ContainsValue(value);

    public T this[T key]
    {
        get => Straight[key]!;
        set => Add(key, value!);
    }

    public bool TryGetValue(T keyType, out T? valueOut)
        => Straight.TryGetValue(keyType, out valueOut);
}