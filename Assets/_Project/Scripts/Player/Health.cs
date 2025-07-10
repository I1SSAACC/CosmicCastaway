using UnityEngine;

public class Health : MonoBehaviour
{
    private const float MaxValue = 100;

    [SerializeField] private Satiety _satiety;
    [SerializeField] private Hydration _hydration;
    [SerializeField] private BarView _view;
    [SerializeField, Range(0, MaxValue)] private float _value = 100;
    [SerializeField] private float _secondsToEmpty;

    private void Awake() =>
        _view.SetValue(_value, MaxValue);

    private void Update()
    {
        if (_satiety.Value == 0)
            Decrease();

        if(_hydration.Value == 0)
            Decrease();

        if (_value != _view.TargetValue)
            _view.SetTargetValue(_value, MaxValue);
    }

    private void Decrease()
    {
        _value -= MaxValue / _secondsToEmpty * Time.deltaTime;
        _value = Mathf.Max(_value, 0);
    }

    public void Increase(float value) =>
        _value = Mathf.Clamp(_value + value, 0, MaxValue);
}