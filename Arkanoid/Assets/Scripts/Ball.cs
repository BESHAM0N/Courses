using UnityEngine;

public class Ball : MonoBehaviour, IMoveable
{
    public float Speed { get; }

    [SerializeField, Tooltip("Скорость шара"), Range(0, 10)] private float _speed = 5;
    [SerializeField, Tooltip("Максимальная скорость шара"), Range(1, 10)] private float _maxSpeed = 10;
    [SerializeField, Tooltip("Средняя скорость шара"), Range(1, 5)] private float _averageSpeed = 3;
    [SerializeField, Tooltip("Значение, на которое увеличивается скорость шара при рефлекте от поверхностей"), Range(0, 1)]
    private float _speedIncrease = 0.2f;
    private Vector3 _direction;

    public Ball()
    {
        Speed = _speed;
    }

    private void Start()
    {
        _direction = Vector3.right;
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.Translate(_direction * Time.deltaTime * _speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool obstacleCollision = collision.collider.TryGetComponent(out Obstacle obstacle);

        if (collision.collider.TryGetComponent(out Gates gates))
        {
            _direction = Vector3.right;
            BallStorage.Ball.SetActive(false);
            BallStorage.IsLaunched = false;
            EventManager.CallShortenLife();
            EventManager.CallChangeBallPosition();
            _speed = _averageSpeed;
        }
        else
        {
            Bounce(collision.GetContact(0).normal);

            if (obstacleCollision)
            {
                collision.gameObject.SetActive(false);
                EventManager.CallCheckLevelCompetition();
            }
        }
    }

    private void Bounce(Vector3 collisionNormal)
    {
        _direction = Vector3.Reflect(_direction.normalized, collisionNormal);
        _speed += _speedIncrease;
        SpeedCheck();
    }

    private void SpeedCheck()
    {
        if (_speed >= _maxSpeed || _speed <= 0)
        {
            _speed = _averageSpeed;
        }
    }
}