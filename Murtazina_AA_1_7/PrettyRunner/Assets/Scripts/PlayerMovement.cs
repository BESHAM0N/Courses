using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private const float _reduceSpeed = 0.5f;

    [SerializeField] [Range(0, 100)] private float _speed = 10f;
    [SerializeField] private float _jumpForce = 2f;
    private PlayerFall _playerFall;
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _playerFall = new PlayerFall();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveForward();
        _playerFall.CheckPosition(transform);
    }

    public void MoveForward()
    {
        _rigidbody.velocity += _speed * Vector3.forward * Time.fixedDeltaTime;
    }

    public void MoveBack()
    {
        if (_speed == 25)
        {
            _rigidbody.velocity += _speed * _reduceSpeed * Vector3.back * Time.fixedDeltaTime;
        }
    }

    public void MoveRight()
    {
        _rigidbody.velocity += _speed * _reduceSpeed * Vector3.right * Time.fixedDeltaTime;
    }

    public void MoveLeft()
    {
        _rigidbody.velocity += _speed * _reduceSpeed * Vector3.left * Time.fixedDeltaTime;
    }

    public void Jump()
    {
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
}