using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject _prefabEnemy;
    [SerializeField, Range(1, 120)] private float _timer = 55;
    [SerializeField] private Transform _point;
    private List<GameObject> _poolEnemy = new();
    private float _startTime = 0;

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
        if (_startTime >= _timer && _poolEnemy.Count <= 3)
        {
            Spawn();
            _startTime = 0;
        }
        else
        {
            _startTime += Time.deltaTime;
        }
    }

    private void InitialSpawn()
    {
        foreach (var item in UnitsStorage.Units)
        {
            
        }    
    }
    
    private void Spawn()
    {
        
        var newEnemy = Instantiate(_prefabEnemy, _point.position, Quaternion.identity);
        _poolEnemy.Add(newEnemy);
        EnemyMove.pool.Add(newEnemy);
        EventManager.CallChangeRotation(newEnemy.transform);
    }
    
    // [SerializeField] private GameObject _prefabEnemy;
    // [SerializeField, Range(1, 120)] private float _timer = 55;
    // [SerializeField] private Transform _point;
    // private List<GameObject> _poolEnemy = new(30);
    // private float _startTime = 0;
    //
    // private void Start()
    // {
    //     Spawn(UnitsStorage.Units);
    // }
    //
    // private void FixedUpdate()
    // {
    //     SpawnValid();
    // }
    //
    // private void SpawnValid()
    // {
    //     if (_startTime >= _timer)
    //     {
    //         _startTime = 0;
    //     }
    //     else
    //     {
    //         _startTime += Time.deltaTime;
    //     }
    // }
    //
    // private void Spawn(List<Unit> units)
    // {
    //     if (units == null) return;
    //
    //     foreach (var unit in units)
    //     {
    //         if(unit.Prefab == null) return;
    //         
    //         for (int i = 0; i < 10; i++)
    //         {
    //             var newEnemy = Instantiate(unit.Prefab, unit.SpawnPoint, Quaternion.identity);
    //             _poolEnemy.Add(newEnemy);
    //             newEnemy.gameObject.SetActive(false);
    //             EventManager.CallChangeRotation(newEnemy.transform);
    //         }
    //     }
    //
    //     Debug.Log("Заполнено на " + _poolEnemy.Count);
    // }
}