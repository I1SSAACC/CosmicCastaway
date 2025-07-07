using UnityEngine;

public class TurnController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _slowMouseX;
    void Start() =>
        _animator = GetComponent<Animator>();

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        _slowMouseX = Mathf.Lerp(_slowMouseX, mouseX, 10 * Time.deltaTime);
        _animator.SetFloat("MouseX", _slowMouseX);

        Debug.Log(_slowMouseX);

    }
}