using System;
using UnityEngine;

public interface IPoolable
{
    event Action<IPoolable> Released;

    void ReturnToPool();
}

public interface IThrowable
{
    void Throw(Vector3 spawnDirection);
}

public interface ISpawnWeighted : IThrowable, IPoolable
{
    float SpawnWeight { get; }
}