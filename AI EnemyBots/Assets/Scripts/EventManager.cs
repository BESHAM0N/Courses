using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager 
{
    public static readonly UnityEvent ActivePanel = new();
    public static readonly UnityEvent<Unit> GetUnit = new();
    public static readonly UnityEvent<Unit> GetUnitForAttack = new();
    public static readonly UnityEvent<Transform> ChangeRotation = new();
    
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
    
    public static void CallGetUnitForAttack(Unit unit)
    {
        if (GetUnitForAttack != null)
            GetUnitForAttack.Invoke(unit);
    }
    
    public static void CallChangeRotation(Transform rotation)
    {
        if (ChangeRotation != null)
            ChangeRotation.Invoke(rotation);
    }
}
