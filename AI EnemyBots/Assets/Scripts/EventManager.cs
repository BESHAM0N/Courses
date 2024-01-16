using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager 
{
    public static readonly UnityEvent ActivePanel = new();
    public static readonly UnityEvent<Unit> GetUnit = new();
    public static readonly UnityEvent<Transform> ChangeRotation = new();
    public static readonly UnityEvent<BaseUnit> CheckTarget = new();
    public static readonly UnityEvent<List<BaseUnit>> SpawnBoots = new();
    
    public static void CallActivePanel()
    {
        if (ActivePanel != null)
            ActivePanel.Invoke();
    }
    
    public static void CallGetUnit(Unit unit)
    {
        if (GetUnit != null)
            GetUnit.Invoke(unit);
    }
    
    public static void CallChangeRotation(Transform rotation)
    {
        if (ChangeRotation != null)
            ChangeRotation.Invoke(rotation);
    }
    
    public static void CallCheckTarget(BaseUnit unit)
    {
        if (CheckTarget != null)
            CheckTarget.Invoke(unit);
    }
    
    public static void CallSpawnBoots(List<BaseUnit> units)
    {
        if (SpawnBoots != null)
            SpawnBoots.Invoke(units);
    }
    
}
