using System;
using System.Collections.Generic;

public class WeightedPicker<T>
{
    private readonly List<T> _items;
    private readonly List<float> _cumulative;

    public WeightedPicker(List<T> items, Func<T, float> weight)
    {
        _items = items;
        _cumulative = new List<float>(items.Count);
        float sum = 0;
        foreach (var item in items)
        {
            sum += weight(item);
            _cumulative.Add(sum);
        }
    }

    public T Pick()
    {
        float r = UnityEngine.Random.value * _cumulative[^1];
        for (int i = 0; i < _cumulative.Count; i++)
            if (r <= _cumulative[i])
                return _items[i];
        return _items[0];
    }
}