using UnityEngine;

public class EndMapZone : MonoBehaviour
{
    private static MapChanger _mapChanger;
    private void OnTriggerEnter(Collider other)
    {
        _mapChanger.ChangeMap();
    }

    private void Awake()
    {
        if (_mapChanger == null)
            _mapChanger = FindObjectOfType<MapChanger>();
    }
}
