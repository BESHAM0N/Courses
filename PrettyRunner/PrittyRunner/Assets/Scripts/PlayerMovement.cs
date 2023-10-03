using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private const float _reduceSpeed = 0.5f;

    [SerializeField] [Range(0, 100)] private float _speed = 15f;
    [SerializeField] private float _jumpForce = 1f;
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveForward();
    }

    public void MoveForward()
    {
        _rigidbody.velocity += _speed * Vector3.forward * Time.fixedDeltaTime;
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