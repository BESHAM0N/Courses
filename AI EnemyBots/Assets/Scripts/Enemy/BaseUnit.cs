using UnityEngine;


public abstract class BaseUnit: Unit 
{
    public BaseUnit Target { get; set; }
    
    private Rigidbody _rigidbody;

    public float maxVelocity = 1;
    public float maxSpeed = 1;
    //public EnemyType EType;
    
    public virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        EventManager.CheckTarget.AddListener(SetTarget);
    }

    private void SetTarget(BaseUnit unit)
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
    
}
