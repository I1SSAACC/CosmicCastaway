using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Satiety _satiety;
    [SerializeField] private Hydration _hydration;

    public void TakeHill(float value) =>
        _health.Increase(value);

    public void TakeWater(float value) =>
        _hydration.Increase(value);

    public void TakeFood(float value) =>
        _satiety.Increase(value);
}