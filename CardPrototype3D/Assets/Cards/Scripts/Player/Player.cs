using System;
using System.Collections;
using Assets.Cards.Scripts.Mechanics;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour, ICanTakeDamage, ICanAttack
{
    [SerializeField] private Transform _deckParent;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private int _maxManaPool = 10;
    [SerializeField] private int _manaPool = 5;
    [SerializeField] private int _currentDamage = 0;
    [SerializeField] private int _defaultDamage = 0;

    [SerializeField] private TextMeshPro _currentManaText;
    [SerializeField] private TextMeshPro _currentHealthText;
    [SerializeField] private TextMeshPro _manaPoolText;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public int CurrentMana => _currentMana;
    public bool CanTakeDamage => _canTakeDamage;
    public int DefaultDamage => _defaultDamage;
    public int CurrentDamage => _currentDamage;
    public bool CanAttack => _canAttack;

    private int _currentMana = 5; 
    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _canAttack;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _currentMana = _manaPool;
        UpdateHealthText();
        UpdateManaText();
    }
    public void SetCanTakeDamage(bool canTakeDamage) => _canTakeDamage = canTakeDamage;

    public void SetCanAttack(bool canAttack) => _canAttack = canAttack;

    public void IncreaseManaPool()
    {
        // Increase mana pool if not max
        if (_manaPool != _maxManaPool)
        {
            _manaPool++;
        }

        RestoreMana(_manaPool);
        UpdateManaText();
    }

    private void RestoreMana(int amount)
    {
        _currentMana = amount;
    }

    public void DecreaseMana(int manaCost)
    {
        Debug.Log("Mana decrese for player: " + _currentMana + " - " + manaCost);
        _currentMana -= manaCost;
        if (_currentMana < 0) 
            _currentMana = 0;
        UpdateManaText();
    }

    public bool TakeDamage(int damage)
    {
        _currentHealth -= damage;
        UpdateHealthText();
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Death();
            return true;
        }
        return false;
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;

        if (_currentHealth >= MaxHealth)
            _currentHealth = MaxHealth;
        UpdateHealthText();
    }

    public void Death()
    {
        Debug.Log($"Player {name} died");
        EventManager.CallPlayerDied(this);
    }

    public IEnumerator AttackAnimation(GameObject target)
    {
        //TODO: должно быть, но не по заданию
        yield return null;
    }
    private void UpdateHealthText()
    {
        _currentHealthText.text = _currentHealth.ToString();
    }

    private void UpdateManaText()
    {
        _currentManaText.text = _currentMana.ToString();
        _manaPoolText.text = _manaPool.ToString();
    }
}