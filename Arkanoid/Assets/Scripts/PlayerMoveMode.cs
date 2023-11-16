using UnityEngine;

public class PlayerMoveMode : MonoBehaviour, IMoveable
{
    public float Speed { get; }
    
    [SerializeField, Tooltip("Задает режим управления для первого игрока")] private bool _moveMode;
    [SerializeField, Tooltip("Скорость управления игроком"), Range(0, 10)] private float _speed = 4;
    
    private float _z;
    private float _y;
    private string _zName;
    private string _yName;

    public PlayerMoveMode()
    {
        Speed = _speed;
    }

    private void Awake()
    {
        SelectMoveMode();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void SelectMoveMode()
    {
        _zName = _moveMode ? "HorizontalOne" : "Horizontal";
        _yName = _moveMode ? "VerticalOne" : "Vertical";
    }

    public void Move()
    {
        Vector3 left = Vector3.zero;
        Vector3 up = Vector3.zero;
        left.z = Input.GetAxis(_zName);
        up.y = Input.GetAxis(_yName);
        Vector3 right = new Vector3(0, 0, left.z) * Time.deltaTime * _speed;
        transform.Translate(right, Space.Self);
        Vector3 down = new Vector3(0, up.y, 0) * Time.deltaTime * _speed;
        transform.Translate(down, Space.Self);
    }
}