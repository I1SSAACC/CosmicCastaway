using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpaceObjectSpawner : MonoBehaviour 
{
    private const float MinSpeedThreshold = 0.001f;
    private const float TimeDespanDelay = 5f;

    [SerializeField, RequireInterface(typeof(ISpawnWeighted))] private List<MonoBehaviour> _prefabs;
    [SerializeField] private BoxCollider _spawnArea;
    [SerializeField] private Vector2 _spawnInterval;
    [SerializeField] private float _despawnDistance;
    [SerializeField] private int _maxPoolSize;

    private readonly List<ISpawnWeighted> _active = new();
    private Pool[] _pools;
    private WeightedPicker<ISpawnWeighted> _picker;
    private WaitForSeconds _despawnWait;
    private Coroutine _spawnRoutine;
    private Coroutine _despawnRoutine;
    private float _speedSpaceship = 1f;
    private List<ISpawnWeighted> _filterPrefabs;

    private void Awake()
    {
        _filterPrefabs = new(_prefabs.Count);

        foreach (MonoBehaviour prefab in _prefabs)
            if (prefab is ISpawnWeighted filterPrefab)
                _filterPrefabs.Add(filterPrefab);

        _pools = _filterPrefabs.Select(p => new Pool(p, null, _maxPoolSize)).ToArray();
        _picker = new(_filterPrefabs, d => d.SpawnWeight);
        _despawnWait = new(TimeDespanDelay);
    }

    private void Start() =>
        Enable();

    public void UpdateSpeedSpaceship(float speed) =>
        _speedSpaceship = Mathf.Max(speed, MinSpeedThreshold);

    public void Enable()
    {
        Disable();
        _spawnRoutine = StartCoroutine(SpawnLoop());
        _despawnRoutine ??= StartCoroutine(DespawnLoop());
    }

    public void Disable()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_spawnInterval.x, _spawnInterval.y) / _speedSpaceship);

            if (_speedSpaceship > MinSpeedThreshold)
                Spawn();
        }
    }

    private IEnumerator DespawnLoop()
    {
        while (_active.Count < 1)
            yield return null;

        while (_active.Count > 0)
        {
            yield return _despawnWait;

            for (int i = _active.Count - 1; i >= 0; i--)
            {
                if (_active[i] == null)
                {
                    _active.RemoveAt(i);
                    Debug.LogWarning("Объект, принадлежащий пулу, вероятно был уничтожен");

                    continue;
                }

                if ((((MonoBehaviour)_active[i]).transform.position - transform.position).sqrMagnitude > _despawnDistance * _despawnDistance)
                    _active[i].ReturnToPool();
            }
        }

        _despawnRoutine = null;
    }

    private void Spawn()
    {
        ISpawnWeighted prefab = _picker.Pick();
        int indexPrefab = _filterPrefabs.IndexOf(prefab);

        if (indexPrefab < 0)
            return;

        Pool pool = _pools[indexPrefab];

        if (pool.TryGet(out IPoolable element) == false)
            return;

        ISpawnWeighted formatElement = (ISpawnWeighted)element;

        (element as Component).transform.SetPositionAndRotation(GetRandomWorldPosition(), Random.rotation);
        element.Released += OnElementReleazed;
        formatElement.Throw(transform.forward);
        _active.Add(formatElement);
    }

    private Vector3 GetRandomWorldPosition()
    {
        Bounds bounds = _spawnArea.bounds;

        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private void OnElementReleazed(IPoolable element)
    {
        element.Released -= OnElementReleazed;
        _active.Remove((ISpawnWeighted)element);
    }
}