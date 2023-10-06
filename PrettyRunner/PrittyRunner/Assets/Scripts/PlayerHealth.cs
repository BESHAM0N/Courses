using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] [Min(1)] private int _health;
    public int Health => _health;

    public event Action HealthChanged;

    public void DecreaseHealth()
    {
        _health--;
        HealthChanged?.Invoke();
        
        if (_health == 0)
        {
            Debug.Log("Вы погибли, нажмите на кнопку restart, чтобы начать игру заново");
            UnityEditor.EditorApplication.isPaused = true;
        }
    }

    public void IncreaseHealth()
    {
        _health++;
        Debug.Log("Вы нашли что-то вкусненькое");
        HealthChanged?.Invoke();

    }
}
