using UnityEngine;
using System;

public class SpaceDebris : MonoBehaviour, IDeactivatable<SpaceDebris>
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector2 _movingSpeedLimits;
    [SerializeField] private Vector2 _rotationSpeedLimits;
    [SerializeField] private Vector2 _scaleFactorLimits;
    [SerializeField] private Vector2 _massLimits;
    [SerializeField] private float _debrisAngleDeviation;
    [SerializeField, Range(0.0001f, 1)] private float _chance;

    private float _moveSpeed;
    private float _factor;

    public event Action<SpaceDebris> Deactivated;

    public float Chance => _chance;

    public void Init(Vector3 direction)
    {
        direction = GetDeviatedDirection(direction);
        _moveSpeed = UnityEngine.Random.Range(_movingSpeedLimits.x, _movingSpeedLimits.y);
        _factor = Mathf.InverseLerp(_movingSpeedLimits.x, _movingSpeedLimits.y, _moveSpeed);

        _rigidbody.linearVelocity = direction * _moveSpeed;
        _rigidbody.angularVelocity = UnityEngine.Random.onUnitSphere * Mathf.Lerp(_rotationSpeedLimits.x, _rotationSpeedLimits.y, _factor);
        transform.localScale = transform.localScale * Mathf.Lerp(_scaleFactorLimits.x, _scaleFactorLimits.y, 1f - _factor);
        _rigidbody.mass = Mathf.Lerp(_massLimits.x, _massLimits.y, _factor);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Deactivated?.Invoke(this);
    }

    private Vector3 GetDeviatedDirection(Vector3 baseDirection)
    {
        Quaternion deviation = Quaternion.Euler(
            UnityEngine.Random.Range(-_debrisAngleDeviation, _debrisAngleDeviation),
            UnityEngine.Random.Range(-_debrisAngleDeviation, _debrisAngleDeviation),
            UnityEngine.Random.Range(-_debrisAngleDeviation, _debrisAngleDeviation)
        );

        return deviation * baseDirection;
    }
}