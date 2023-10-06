using UnityEngine;

public class Boost : MonoBehaviour
{
    private static PlayerHealth _playerHealth;
    
    private void OnTriggerEnter(Collider collider)
    {
        _playerHealth.IncreaseHealth();
    }

    private void Awake()
    {
        if (_playerHealth == null)
            _playerHealth = FindObjectOfType<PlayerHealth>();
    }
}
