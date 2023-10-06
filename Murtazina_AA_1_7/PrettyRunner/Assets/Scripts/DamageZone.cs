using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private static PlayerHealth _playerHealth;
    
    private void OnTriggerEnter(Collider collider)
    {
        _playerHealth.DecreaseHealth();
    }

    private void Awake()
    {
        if (_playerHealth == null)
            _playerHealth = FindObjectOfType<PlayerHealth>();
    }
}
