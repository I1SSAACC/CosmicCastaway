using UnityEngine;
using UnityEngine.UI;

public class Indicators : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _foodBar;
    [SerializeField] private Slider _waterBar;

    [Range(0, 100)]
    [SerializeField] private float _healthAmount = 100;
    private float _uiHealthAmount = 100;

    [Range(0, 100)]
    [SerializeField] private float _foodAmount = 100;
    private float _uiFoodAmount = 100;

    [Range(0, 100)]
    [SerializeField] private float _waterAmount = 100;
    private float _uiWaterAmount = 100;

    [SerializeField] private float _secondsToEmptyFood = 60f;
    [SerializeField] private float _secondsToEmptyWater = 30f;
    [SerializeField] private float _secondsToEmptyHealth = 60f;

    [SerializeField] private float _changeFactor = 6f;

    void Start()
    {
        _healthBar.minValue = 0;
        _healthBar.maxValue = 100;
        _foodBar.minValue = 0;
        _foodBar.maxValue = 100;
        _waterBar.minValue = 0;
        _waterBar.maxValue = 100;

        _healthBar.value = _healthAmount;
        _foodBar.value = _foodAmount;
        _waterBar.value = _waterAmount;
    }

    void Update()
    {
        if (_foodAmount > 0)
        {
            _foodAmount -= 100f / _secondsToEmptyFood * Time.deltaTime;
            _uiFoodAmount = Mathf.Lerp(_uiFoodAmount, _foodAmount, Time.deltaTime * _changeFactor);
            _foodBar.value = _uiFoodAmount;
        }
        else
        {
            _uiFoodAmount = 0;
            _foodBar.value = 0;
        }

        if (_waterAmount > 0)
        {
            _waterAmount -= 100f / _secondsToEmptyWater * Time.deltaTime;
            _uiWaterAmount = Mathf.Lerp(_uiWaterAmount, _waterAmount, Time.deltaTime * _changeFactor);
            _waterBar.value = _uiWaterAmount;
        }
        else
        {
            _uiWaterAmount = 0;
            _waterBar.value = 0;
        }

        if (_foodAmount <= 0)
            _healthAmount -= 100f / _secondsToEmptyHealth * Time.deltaTime;
        if (_waterAmount <= 0)
            _healthAmount -= 100f / _secondsToEmptyHealth * Time.deltaTime;

        _uiHealthAmount = Mathf.Lerp(_uiHealthAmount, _healthAmount, Time.deltaTime * _changeFactor);
        _healthBar.value = _uiHealthAmount;
    }

    public void ChangeFoodAmount(float changeValue) =>
        _foodAmount = Mathf.Clamp(_foodAmount + changeValue, 0, 100);

    public void ChangeWaterAmount(float changeValue) =>
        _waterAmount = Mathf.Clamp(_waterAmount + changeValue, 0, 100);

    public void ChangeHealthAmount(float changeValue) =>
        _healthAmount = Mathf.Clamp(_healthAmount + changeValue, 0, 100);
}