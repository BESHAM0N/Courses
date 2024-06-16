using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject _prefabEnemy;
    [SerializeField, Range(1, 120)] private float _timer = 60;
    [SerializeField] private Transform _point;
    [SerializeField] private EnemyType _enemyType;
    private List<GameObject> _poolEnemy = new();
    private float _startTime;

    private void Start()
    {
        Spawn();
    }
    private void FixedUpdate()
    {
        SpawnValid();
    }
    private void SpawnValid()
    {
        if (_startTime >= _timer)
        {
            Spawn();
            _startTime = 0;
        }
        else
        {
            _startTime += Time.deltaTime;
        }
    }
    private void Spawn()
    {
        var newEnemy = Instantiate(_prefabEnemy, _point.position, Quaternion.identity);
        var unitStats = UnitsStorage.Units.FirstOrDefault(x => x.EnemyType == _enemyType);
        var unit = newEnemy.GetComponent<Unit>();
        unit.SetStats(unitStats);
        _poolEnemy.Add(newEnemy);
        EventManager.CallChangeRotation(newEnemy.transform);
    }
}