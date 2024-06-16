using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySettingsManager : MonoBehaviour
{
    [SerializeField] private Text _enemyName;
    [SerializeField] private Button _applyButton;
    [SerializeField] private Slider _speed;
    [SerializeField] private Slider _health;
    [SerializeField] private Slider _probabilityOfMiss;
    [SerializeField] private Slider _probabilityOfDoubleDamage;
    [SerializeField] private Slider _probabilityOfWeakAndStrongAttacks;
    [SerializeField] private TMP_InputField _weakAttack;
    [SerializeField] private TMP_InputField _strongAttack;
    private Unit _unit;

    private void Start()
    {
        _applyButton.onClick.AddListener(OnApplyClick);
        EventManager.GetUnit.AddListener(SetSettings);
    }
    private void SetSettings(Unit unit)
    {
        _unit = unit;
        _enemyName.text = _unit.Name;
        _speed.value = _unit.MoveSpeed;
        _health.value = _unit.Health;
        _probabilityOfMiss.value = _unit.ProbabilityOfMiss;
        _probabilityOfDoubleDamage.value = _unit.ProbabilityOfDoubleDamage;
        _weakAttack.text = _unit.WeakAttack.ToString();
        _strongAttack.text = _unit.StrongAttack.ToString();
        //TODO
        _probabilityOfWeakAndStrongAttacks.value = _unit.ProbabilityOfWeakAttack;
    }
    private void OnApplyClick()
    {
        _unit.Health = (int)_health.value;
        _unit.MoveSpeed = (int)_speed.value;
        _unit.ProbabilityOfMiss = _probabilityOfMiss.value;
        _unit.ProbabilityOfDoubleDamage = _probabilityOfDoubleDamage.value;
        _unit.ProbabilityOfWeakAttack = _probabilityOfWeakAndStrongAttacks.value;
        _unit.WeakAttack = Convert.ToInt32(_weakAttack.text);
        _unit.StrongAttack = Convert.ToInt32(_strongAttack.text);
        _unit.ProbabilityOfWeakAttack = _probabilityOfWeakAndStrongAttacks.value;
        _unit.ProbabilityOfStrongAttack = 100 - _unit.ProbabilityOfWeakAttack;
        Debug.Log($"Характеристики успешно заменены для: {_unit.Name}");
    }
}
