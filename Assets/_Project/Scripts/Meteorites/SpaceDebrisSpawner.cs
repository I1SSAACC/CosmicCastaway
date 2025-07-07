using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SpaceDebrisSpawner : MonoBehaviour
{
    private const float MinSpeedThreshold = 0.001f;
    private const float TimeDespanDelay = 5f;

    [SerializeField] private List<SpaceDebris> _prefabs;
    [SerializeField] private int _maximumObjects;
    [SerializeField] private Vector2 _spawnInterval;
    [SerializeField] private BoxCollider _spawnArea;
    [SerializeField] private float _despawnDistance;

    private readonly Dictionary<SpaceDebris, Pool<SpaceDebris>> _poolDictionary = new();
    private readonly List<SpaceDebris> _debrisList = new();
    private WaitForSeconds _despawnWait;
    private Coroutine _coroutine;
    private float _speedSpaceship = 1f;
    private float _totalChance;

    private void Awake()
    {
        _totalChance = 0f;

        foreach (var prefab in _prefabs)
        {
            if (prefab == null)
                continue;

            Pool<SpaceDebris> pool = new(prefab, null, _maximumObjects);
            _poolDictionary.Add(prefab, pool);
            _totalChance += prefab.Chance;
        }

        _despawnWait = new(TimeDespanDelay);
    }

    private void Start()
    {
        Enable();
        StartCoroutine(DespawnCoroutine());
    }

    public void UpdateSpeedSpaceship(float speed) =>
        _speedSpaceship = Mathf.Max(speed, MinSpeedThreshold);

    public void Enable()
    {
        Disable();
        _coroutine = StartCoroutine(SpawnCoroutine());
    }

    public void Disable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private SpaceDebris GetRandomPrefab()
    {
        float randomValue = Random.Range(0f, _totalChance);
        float currentChance = 0f;

        foreach (var prefab in _prefabs)
        {
            if (prefab == null) continue;

            currentChance += prefab.Chance;

            if (randomValue <= currentChance)
                return prefab;
        }

        return _prefabs.Count > 0 ? _prefabs[0] : null;
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(GetSpawnInterval());

            if (_speedSpaceship > MinSpeedThreshold)
                Spawn();
        }
    }

    private IEnumerator DespawnCoroutine()
    {
        while (true)
        {
            yield return _despawnWait;

            foreach (SpaceDebris debris in _debrisList)
                if (debris != null && debris.transform.position.sqrMagnitude > _despawnDistance * _despawnDistance)
                    debris.Deactivate();
        }
    }

    private float GetSpawnInterval() =>
        Random.Range(_spawnInterval.x, _spawnInterval.y) / _speedSpaceship;

    private void Spawn()
    {
        SpaceDebris debrisPrefab = GetRandomPrefab();

        if (debrisPrefab == null || _poolDictionary.TryGetValue(debrisPrefab, out Pool<SpaceDebris> pool) == false)
            return;

        if (pool.TryGet(out SpaceDebris debris) == false)
            return;

        debris.transform.SetPositionAndRotation(GetRandomWorldPosition(), Random.rotation);
        debris.Deactivated += OnDebrisDeactivated;
        debris.Init(transform.forward);
        _debrisList.Add(debris);
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

    private void OnDebrisDeactivated(SpaceDebris debris)
    {
        debris.Deactivated -= OnDebrisDeactivated;
        _debrisList.Remove(debris);
    }
}