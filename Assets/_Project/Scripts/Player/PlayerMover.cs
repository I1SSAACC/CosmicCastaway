using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _camera;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _runningSpeed;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _animationInterpolation;

    [SerializeField] private Transform _aimTarget;

    public Rigidbody RigidbodyComponent => _rigidbody;
    public Vector3 ForwardDirection => transform.forward;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Run()
    {
        _animationInterpolation = Mathf.Lerp(_animationInterpolation, 1.5f, Time.deltaTime * 3);
        _animator.SetFloat("x", Input.GetAxis("Horizontal") * _animationInterpolation);
        _animator.SetFloat("y", Input.GetAxis("Vertical") * _animationInterpolation);

        _currentSpeed = Mathf.Lerp(_currentSpeed, _runningSpeed, Time.deltaTime * 3);
    }
    void Walk()
    {
        _animationInterpolation = Mathf.Lerp(_animationInterpolation, 1f, Time.deltaTime * 3);
        _animator.SetFloat("x", Input.GetAxis("Horizontal") * _animationInterpolation);
        _animator.SetFloat("y", Input.GetAxis("Vertical") * _animationInterpolation);

        _currentSpeed = Mathf.Lerp(_currentSpeed, _walkingSpeed, Time.deltaTime * 3);
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _camera.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                Walk();

            else
                Run();
        }

        else
            Walk();

        if (Input.GetKeyDown(KeyCode.Space))
            _animator.SetTrigger("Jump");

        Ray desiredTargetRay = _camera.GetComponent<Camera>().ScreenPointToRay(new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f));
        Vector3 desiredTargetPosition = desiredTargetRay.origin + desiredTargetRay.direction * 0.5f;
        _aimTarget.position = desiredTargetPosition;
    }

    void FixedUpdate()
    {
        Vector3 camF = _camera.forward;
        Vector3 camR = _camera.right;
        camF.y = 0;
        camR.y = 0;
        Vector3 movingVector;
        movingVector = Vector3.ClampMagnitude(camF.normalized * Input.GetAxis("Vertical") * _currentSpeed + camR.normalized * Input.GetAxis("Horizontal") * _currentSpeed, _currentSpeed);
        _animator.SetFloat("magnitude", movingVector.magnitude / _currentSpeed);
        Debug.Log(movingVector.magnitude / _currentSpeed);
        _rigidbody.linearVelocity = new Vector3(movingVector.x, _rigidbody.linearVelocity.y, movingVector.z);
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void Jump() =>
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
}