using System;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Health { get; set; }
    public Vector3 SpawnPoint { get; set; }
    
    public GameObject Prefab { get; set; }
    public int MoveSpeed { get; set; }
    public float WeakAttack { get; set; }
    public float StrongAttack { get; set; }
    public float ProbabilityOfMiss { get; set; }
    public float ProbabilityOfDoubleDamage { get; set; }
    public float ProbabilityOfWeakAttack { get; set; }
    public float ProbabilityOfStrongAttack { get; set; }    
    
    public EnemyType EnemyType { get; set; }
    
    
}

public enum EnemyType
{
    Slime,
    Mushroom,
    Cactus
}
