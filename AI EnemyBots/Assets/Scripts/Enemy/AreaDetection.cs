using System.Linq;
using UnityEngine;

public class AreaDetection : MonoBehaviour
{
    
    private Unit _unit;

    private void Start()
    {
        var unitMarker = GetComponentInParent<IBaseBotsMarker>();
        _unit = UnitsStorage.Units.FirstOrDefault(x => x.Id == unitMarker.UnitId);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IBaseBotsMarker unit) &&
            other.gameObject.TryGetComponent(out BaseUnit baseUnit))
        {
            if (unit.UnitId != baseUnit.Id)
            {
                Debug.Log(baseUnit.Id + "айди кактуса");
                Debug.Log(unit.UnitId + "айди врага");
                EventManager.CallCheckTarget(baseUnit);
            }
        }
    }
}