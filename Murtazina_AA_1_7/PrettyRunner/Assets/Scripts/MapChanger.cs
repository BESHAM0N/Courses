using UnityEngine;

public class MapChanger : MonoBehaviour
{
    [SerializeField] private Transform[] _maps;

    private int _currentMapIndex;
    private Vector3 _lastposition = Vector3.zero;

    public void ChangeMap()
    {
        _maps[_currentMapIndex].position = _lastposition + new Vector3(0, 0, 16);
            _lastposition = _maps[_currentMapIndex].transform.position;
            _currentMapIndex = (_currentMapIndex + 1) % _maps.Length;
    }

    private void Awake()
    {
        _lastposition = _maps[_maps.Length - 1].position;
    }
}