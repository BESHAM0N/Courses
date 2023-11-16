using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefabBall;
    [SerializeField] private GameObject _prefabObstacles;

    [SerializeField, Tooltip("Позиция появления шара")]
    private Transform _point;

    [SerializeField, Tooltip("Максимальное количество препятствий на уровне"), Range(1, 20)]
    private int _maxCountObstacles = 8;

    [SerializeField, Tooltip("Ширина препятствий"), Range(0, 2)]
    private float _xScale = 0.6f;

    [SerializeField, Tooltip("Высота препятствий"), Range(0, 2)]
    private float _yScale = 0.5f;

    [SerializeField, Tooltip("Максимальная длина препятствий"), Range(0, 20)]
    private float _zMaxScale = 10f;

    [SerializeField, Tooltip("Минимальная длина препятствий"), Range(0, 20)]
    private float _zMinScale = 1.5f;

    [SerializeField, Tooltip("Максимальный уровень наклона препятствий на уровне"), Range(0, 360)]
    private float _xMaxAngle = 30f;

    [SerializeField, Tooltip("Минимальный уровень наклона препятствий на уровне"), Range(0, -360)]
    private float _xMinAngle = -40f;

    private List<GameObject> _obstacles = new();
    private GameObject _ball;

    private const float X_MAX = 4.5f;
    private const float X_MIN = -3f;
    private const float Y_MAX = 5.2f;
    private const float Y_MIN = 0.3f;
    private const float Z_MAX = 3.6f;
    private const float Z_MIN = -3.6f;

    private void Start()
    {
        EventManager.ChangeBallPosition.AddListener(ChangePosition);
        SpawnBall();
        SpawnObstacles();
        EventManager.CallInitMaxCountObstacle(_maxCountObstacles);
    }

    private void Update()
    {
        ResetBall();
    }

    private void SpawnObstacles()
    {
        for (int i = 0; i < _maxCountObstacles; i++)
        {
            Quaternion newAngle = Quaternion.Euler(Random.Range(_xMaxAngle, _xMinAngle), 0, 0);
            Vector3 randomSpawnPoint = new Vector3(Random.Range(X_MAX, X_MIN), Random.Range(Y_MAX, Y_MIN),
                Random.Range(0, 2) == 1 ? Z_MAX : Z_MIN);
            _prefabObstacles.gameObject.transform.localScale =
                new Vector3(_xScale, _yScale, Random.Range(_zMaxScale, _zMinScale));
            GameObject newObstacle = Instantiate(_prefabObstacles, randomSpawnPoint, newAngle);
            _obstacles.Add(newObstacle);
        }
    }

    private void SpawnBall()
    {
        if (BallStorage.Ball == null)
        {
            BallStorage.Ball = Instantiate(_prefabBall, _point.position, Quaternion.identity);
            BallStorage.Ball.SetActive(false);
        }
    } 

    private void ResetBall()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (BallStorage.IsLaunched)
                return;

            BallStorage.Ball.transform.position = _point.position;
            BallStorage.Ball.SetActive(true);
            BallStorage.IsLaunched = true;
        }
    }
    private void ChangePosition()
    {
        BallStorage.Ball.transform.position = _point.position;
    }
}