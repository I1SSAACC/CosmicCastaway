using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private const int MaximumSize = 100;

    private readonly IPoolable _prefab;
    private readonly Transform _parent;
    private readonly Stack<IPoolable> _elements = new();

    private readonly int _size;
    private int _count;

    public Pool(IPoolable prefab, Transform parent, int size = MaximumSize)
    {
        _prefab = prefab;
        _parent = parent;
        _size = size;
    }

    public bool TryGet(out IPoolable element)
    {
        element = null;

        if (_elements.Count == 0 && _count >= _size)
            return false;

        if (_elements.Count > 0)
            element = _elements.Pop();
        else
            element = Create();

        element.Released += Return;
        MonoBehaviour mono = element as MonoBehaviour;
        ((MonoBehaviour)element).gameObject.SetActive(true);

        return true;
    }

    private void Return(IPoolable element)
    {
        element.Released -= Return;
        ((MonoBehaviour)element).gameObject.SetActive(false);
        _elements.Push(element);
    }

    private IPoolable Create()
    {
        _count++;

        return Object.Instantiate((MonoBehaviour)_prefab, _parent) as IPoolable;
    }
}