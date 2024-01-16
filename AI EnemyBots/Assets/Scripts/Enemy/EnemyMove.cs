using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public static List<GameObject> pool = new();
    private BaseUnit _unit;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody _rigidbody;
    private Vector3 _lastVelocity;
    private float _speed = 5;
    private Vector3 direction;

    private void Start()
    {
        _unit = GetComponent<BaseUnit>();
    }

    private void Update()
    {
        RotateEnemyToTarget();
        OnSeek();
    }

    private void RotateEnemyToTarget()
    {
        if (_unit.Target == null)
        {
            direction = new Vector3(3f, 1.1f, -5.3f) - transform.position;
            var rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 2 * Time.deltaTime);
        }
        else
        {
            direction = _unit.Target.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 2 * Time.deltaTime);
        }
    }

    private void OnSeek()
    {
        if (_unit.Target == null)
        {
            var startPos = transform.position;
            transform.position = Vector3.MoveTowards(startPos,
                new Vector3(3f, 1.1f, -5.3f),
                Time.deltaTime * 1);
            return;
        }

        Debug.Log($"Я {_unit.Name} иду за {_unit.Target.Name}");

        var desireVelocity =
            (_unit.Target.transform.position - _unit.transform.position).normalized * _unit.maxVelocity;
        var steering = desireVelocity - _unit.GetVelocity();

        var velocity = Vector3.ClampMagnitude(_unit.GetVelocity(IgnoreAxisType.None) + steering, _unit.maxSpeed);
        _unit.SetVelocity(velocity, IgnoreAxisType.Y);
    }

    private void EnemyAttack()
    {
        //TODO 1) Определение минимальной дистанции, для приостановления движения 2) Запуск атаки
    }

}