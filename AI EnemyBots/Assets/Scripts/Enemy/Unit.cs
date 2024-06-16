using UnityEngine;

public class Unit: MonoBehaviour
{
    public int Id;
    public string Name;
    public int Health;
    public Vector3 SpawnPoint;
    public GameObject Prefab;
    public int MoveSpeed;
    public float WeakAttack;
    public float StrongAttack;
    public float ProbabilityOfMiss;
    public float ProbabilityOfDoubleDamage;
    public float ProbabilityOfWeakAttack;
    public float ProbabilityOfStrongAttack;
    public EnemyType EnemyType;
    public Unit Target;
    public float maxVelocity = 1;
    public float maxSpeed = 1;
    
    private Rigidbody _rigidbody;
    public virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //EventManager.CheckTarget.AddListener(SetTarget);
    }
    private void SetTarget(Unit unit)
    {
        Target = unit;
    }
    public Vector3 GetVelocity(IgnoreAxisType ignore = IgnoreAxisType.Y)
    {
        return IgnoreAxisUpdate(ignore, _rigidbody.velocity);
    }
    public void SetVelocity(Vector3 velocity, IgnoreAxisType ignore = IgnoreAxisType.None)
    {
        _rigidbody.velocity = IgnoreAxisUpdate(ignore, velocity);
    }
    private Vector3 IgnoreAxisUpdate(IgnoreAxisType ignore, Vector3 velocity)
    {
        if (ignore == IgnoreAxisType.None) return velocity;
        if ((ignore & IgnoreAxisType.X) == IgnoreAxisType.X) velocity.x = 0f;
        if ((ignore & IgnoreAxisType.Y) == IgnoreAxisType.Y) velocity.y = 0f;
        if ((ignore & IgnoreAxisType.Z) == IgnoreAxisType.Z) velocity.z = 0f;

        return velocity;
    }

    public void SetStats(Unit unit)
    {
        Name = unit.Name;
        Health = unit.Health;
        MoveSpeed = unit.MoveSpeed;
        WeakAttack = unit.WeakAttack;
        StrongAttack = unit.StrongAttack;
        ProbabilityOfMiss = unit.ProbabilityOfMiss;
        ProbabilityOfDoubleDamage = unit.ProbabilityOfDoubleDamage;
        ProbabilityOfWeakAttack = unit.ProbabilityOfWeakAttack;
        ProbabilityOfStrongAttack = unit.ProbabilityOfStrongAttack;
        EnemyType = unit.EnemyType;
    }
}

public enum EnemyType
{
    Slime,
    Mushroom,
    Cactus
}

