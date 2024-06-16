using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float _pauseAttacks = 2f;
    private int _randomValue;
    private void Start()
    {
        //EventManager.GetUnitForAttack.AddListener(Attack);
    }

    // public void Attack(Unit unit)
    // {
    //     Debug.Log($"Атака началась после вызова по ивенту, передался юнит {unit}");
    //     if (unit.Target == null) return;
    //     unit.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //     _randomValue = Random.Range(0, 100);
    //
    //     if (unit.Target.Health <= 0)
    //     {
    //         unit.Target.gameObject.SetActive(false);
    //         Destroy(unit.Target.gameObject);
    //         unit.Target = null;
    //     }
    //     else
    //     {
    //         if (_pauseAttacks > 0)
    //         {
    //             _pauseAttacks -= Time.deltaTime;
    //         }
    //         else
    //         {
    //             if (_randomValue <= unit.ProbabilityOfMiss)
    //             {
    //                 unit.Target.Health -= 0;
    //                 _pauseAttacks = 2;
    //                 Debug.Log($"{unit.Name} промахнулся");
    //             }
    //             else
    //             {
    //                 _randomValue = Random.Range(0, 100);
    //                 //Если число попадает в диапазон слабой
    //                 if (_randomValue <= unit.ProbabilityOfWeakAttack)
    //                 {
    //                     Debug.Log($"{unit.Name} попал в диапазон слабой атаки");
    //                     if (_randomValue <= unit.ProbabilityOfDoubleDamage)
    //                     {
    //                         unit.Target.Health -= (int)unit.WeakAttack;
    //                     }
    //                     else
    //                     {
    //                         unit.Target.Health -= (int)unit.WeakAttack * 2;
    //                         Debug.Log($"{unit.Name} попал в диапазон двойной слабой атаки");
    //                     }
    //                     _pauseAttacks = 2;
    //                 }
    //                 //Если число попадает в диапазон сильной
    //                 else
    //                 {
    //                     Debug.Log($"{unit.Name} попал в диапазон сильной атаки");
    //                     if (_randomValue <= unit.ProbabilityOfDoubleDamage)
    //                     {
    //                         unit.Target.Health -= (int)unit.StrongAttack;
    //                     }
    //                     else
    //                     {
    //                         unit.Target.Health -= (int)unit.StrongAttack * 2;
    //                         Debug.Log($"{unit.Name} попал в диапазон двойной сильной атаки");
    //                     }
    //                     _pauseAttacks = 4;
    //                 }
    //             }
    //         }
    //     }
    // }
}