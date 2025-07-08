using UnityEngine;
using System;

public class SpaceDebris : MonoBehaviour, ISpawnWeighted
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector2 _speedRange;
    [SerializeField] private Vector2 _rotationSpeedLimits;
    [SerializeField] private Vector2 _scaleLimits;
    [SerializeField] private Vector2 _massLimits;
    [SerializeField] private float _maxAngleDeviation;
    [SerializeField, Range(0.0001f, 1)] private float _spawnWeight;

    private float _moveSpeed;
    private float _factor;

    public event Action<IPoolable> Released;

    public float SpawnWeight => _spawnWeight;

    public void Throw(Vector3 direction)
    {
        direction = GetDeviatedDirection(direction);
        _moveSpeed = UnityEngine.Random.Range(_speedRange.x, _speedRange.y);
        _factor = Mathf.InverseLerp(_speedRange.x, _speedRange.y, _moveSpeed);

        _rigidbody.linearVelocity = direction * _moveSpeed;
        _rigidbody.angularVelocity = UnityEngine.Random.onUnitSphere * Mathf.Lerp(_rotationSpeedLimits.x, _rotationSpeedLimits.y, _factor);
        transform.localScale = Vector3.one * Mathf.Lerp(_scaleLimits.x, _scaleLimits.y, 1f - _factor);
        _rigidbody.mass = Mathf.Lerp(_massLimits.x, _massLimits.y, _factor);
    }

    public void ReturnToPool() =>
        Released?.Invoke(this);

    private Vector3 GetDeviatedDirection(Vector3 baseDirection)
    {
        Quaternion deviation = Quaternion.Euler(
            UnityEngine.Random.Range(-_maxAngleDeviation, _maxAngleDeviation),
            UnityEngine.Random.Range(-_maxAngleDeviation, _maxAngleDeviation),
            UnityEngine.Random.Range(-_maxAngleDeviation, _maxAngleDeviation)
        );

        return deviation * baseDirection;
    }
}