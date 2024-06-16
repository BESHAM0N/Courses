using System.Linq;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Vector3 _center = new(3f, 1.1f, -5.3f);

    private GameObject _target;
    private Animator _animator;
    private Unit _unit;
    private Vector3 _direction;
    private float _dist;
    private float pause = 3f;
    private float _pauseAttacks = 2f;
    private int _randomValue;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _unit = GetComponent<Unit>();
    }

    void Update()
    {
        RotateEnemyToTarget();

        if (_unit.Target == null)
        {
            GoToCenter();
            GetTarget();
        }
        else
        {
            GoToTarget();
        }
    }

    private void RotateEnemyToTarget()
    {
        if (_unit.Target == null)
        {
            _direction = _center - transform.position;
            var rotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 2 * Time.deltaTime);
        }
        else
        {
            _direction = _unit.Target.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 2 * Time.deltaTime);
        }
    }

    private void GoToCenter()
    {
        if (_unit.Target != null) return;
        _animator.SetBool("IsRunninig", true);
        Debug.Log("Я двигаюсь к центру");
        var startPos = transform.position;
        transform.position = Vector3.MoveTowards(startPos,
            _center,
            Time.deltaTime * 1);
    }

    private void GetTarget()
    {
        Collider[] hit;
        hit = Physics.OverlapSphere(transform.position, 3f);
        var enemy = hit.FirstOrDefault(x => x.gameObject.layer != gameObject.layer).gameObject.GetComponent<Unit>();
        if (enemy != null)
        {
            _unit.Target = enemy;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, 3f);
    }

    private void GoToTarget()
    {
        _dist = Vector3.Distance(_unit.Target.transform.position, transform.position);

        if (_dist > 2f)
        {
            Debug.Log($"Я {_unit.Name} двигаюсь за врагом {_unit.Target}");
            var desireVelocity =
                (_unit.Target.transform.position - _unit.transform.position).normalized * _unit.maxVelocity;
            var steering = desireVelocity - _unit.GetVelocity();

            var velocity =
                Vector3.ClampMagnitude(_unit.GetVelocity(IgnoreAxisType.None) + steering, _unit.maxSpeed);
            _unit.SetVelocity(velocity, IgnoreAxisType.Y);
        }
        else
        {
            Debug.Log($"я {_unit.Name} Начинаю атаку на {_unit.Target.Name}");
            // EventManager.CallGetUnitForAttack(_unit);
            Attack(_unit);
        }
    }
    
     private void Attack(Unit unit)
    {
        Debug.Log($"Атака началась после вызова по ивенту, передался юнит {unit}");
        if (unit.Target == null) return;
        unit.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _animator.SetBool("IsRunninig", false);
        _randomValue = Random.Range(0, 100);

        if (unit.Target.Health <= 0)
        {
            _animator.SetBool("IsAttacking", false);
            _animator.SetBool("IsRunninig", true);
            unit.Target.gameObject.SetActive(false);
            Destroy(unit.Target.gameObject);
            unit.Target = null;
        }
        else
        {
            _animator.SetBool("IsAttacking", true);
            if (_pauseAttacks > 0)
            {
                _pauseAttacks -= Time.deltaTime;
            }
            else
            {
                if (_randomValue <= unit.ProbabilityOfMiss)
                {
                    unit.Target.Health -= 0;
                    _pauseAttacks = 2;
                    Debug.Log($"{unit.Name} промахнулся");
                }
                else
                {
                    
                    //Если число попадает в диапазон слабой
                    if (_randomValue <= unit.ProbabilityOfWeakAttack)
                    {
                        Debug.Log($"{unit.Name} попал в диапазон слабой атаки");
                        if (_randomValue <= unit.ProbabilityOfDoubleDamage)
                        {
                            unit.Target.Health -= (int)unit.WeakAttack;
                            unit.Target.GetComponent<HealhController>().ChangeHealth((int)unit.WeakAttack);
                        }
                        else
                        {
                            unit.Target.Health -= (int)unit.WeakAttack * 2;
                            unit.Target.GetComponent<HealhController>().ChangeHealth((int)unit.WeakAttack * 2);
                            Debug.Log($"{unit.Name} попал в диапазон двойной слабой атаки");
                        }
                        _pauseAttacks = 2;
                    }
                    //Если число попадает в диапазон сильной
                    else
                    {
                       
                        Debug.Log($"{unit.Name} попал в диапазон сильной атаки");
                        if (_randomValue <= unit.ProbabilityOfDoubleDamage)
                        {
                            unit.Target.Health -= (int)unit.StrongAttack;
                            unit.Target.GetComponent<HealhController>().ChangeHealth((int)unit.StrongAttack);
                        }
                        else
                        {
                            unit.Target.Health -= (int)unit.StrongAttack * 2;
                            unit.Target.GetComponent<HealhController>().ChangeHealth((int)unit.StrongAttack * 2);
                            Debug.Log($"{unit.Name} попал в диапазон двойной сильной атаки");
                        }
                        _pauseAttacks = 4;
                    }
                }
            }
        }
    }
}