using UnityEngine;
using UnityEngine.UI;

public class BarView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _smoothSpeed;

    private float _targetValue;

    public float TargetValue => _targetValue;

    private void Update()
    {
        if (_slider.value != _targetValue)
            _slider.value = Mathf.Lerp(_slider.value, _targetValue, Time.deltaTime * _smoothSpeed);
    }

    public void SetValue(float value, float maxValue)
    {
        _targetValue = Mathf.Clamp(_slider.value + value, 0, maxValue);
        _slider.value = _targetValue;
    }

    public void SetTargetValue(float value, float maxValue) =>
        _targetValue = Mathf.Clamp(value / maxValue, 0, 1);
}