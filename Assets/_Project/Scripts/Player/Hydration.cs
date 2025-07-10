using UnityEngine;

public class Hydration : MonoBehaviour
{
    private const float MaxValue = 100;

    [SerializeField] private BarView _view;
    [SerializeField, Range(0, MaxValue)] private float _value = 100;
    [SerializeField] private float _secondsToEmpty;

    public float Value => _value;

    private void Awake() =>
        _view.SetValue(_value, MaxValue);

    private void Update()
    {
        if (_value == 0 && _view.TargetValue == _value)
            return;

        _value -= MaxValue / _secondsToEmpty * Time.deltaTime;
        _value = Mathf.Max(_value, 0);
        _view.SetTargetValue(_value, MaxValue);
    }

    public void Increase(float value) =>
        _value = Mathf.Clamp(_value + value, 0, MaxValue);
}