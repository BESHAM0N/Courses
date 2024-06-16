using System;
using UnityEngine;

public class HealhController : MonoBehaviour
{
    public event Action<float> HealthChanged;
    private Unit _unit;
    private int _unitMaxHP;
    private int _currentHP;
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _unit = GetComponent<Unit>();
        _unitMaxHP = _unit.Health;
        _currentHP = _unitMaxHP;
    }

    public void ChangeHealth(int value)
    {
        _currentHP -= value;
        if (_currentHP <= 0)
        {
            Death();
        }
        else
        {
            float _currentHealthAsPercantage = (float)_currentHP / _unitMaxHP;
            HealthChanged?.Invoke(_currentHealthAsPercantage);
        }
    }

    private void Death()
    {
        HealthChanged?.Invoke(0);
        _animator.SetBool("IsAttacking", false);
        _animator.SetBool("IsDead", true);
    }
    
}
