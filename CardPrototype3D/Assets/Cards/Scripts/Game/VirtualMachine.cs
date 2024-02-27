using System;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace Assets.Cards.Scripts.Game
{
    public class VirtualMachine : MonoBehaviour
    {
        [SerializeField] private CardManager _cardManager;

        private const int _capacity = 128;
        private readonly Stack<int> _stack = new(_capacity);

        private AbilityController _abilityController;
        private void Awake()
        {
            _abilityController = new(_cardManager);
        }

        public void Interpet(int[] bytecode, int size)
        {
            _stack.Clear();
            for (int i = 0; i < bytecode.Length; i++)
            {
                var instruction = (Instruction)bytecode[i];
                switch (instruction)
                {
                    case Instruction.Damage:
                        var damage = _stack.Pop();
                        _abilityController.DealDamage(damage);
                        break;
                    case Instruction.Buff:
                        var damageAmount = _stack.Pop();
                        var healthAmount = _stack.Pop();
                        var permanent = _stack.Pop() == 1;
                        var targetType = (CardUnitType)_stack.Pop();
                        _abilityController.Buff(damageAmount, healthAmount, permanent, targetType);
                        break;
                    case Instruction.Heal:
                        var healTargetType = (CardUnitType)_stack.Pop();
                        var healAmount = _stack.Pop();
                        _abilityController.Heal(healAmount, healTargetType);
                        break;
                    case Instruction.Literal:
                        _stack.Push(bytecode[++i]);
                        break;
                    case Instruction.Summon:
                        var id = _stack.Pop();
                        _abilityController.Summon(id);
                        break;
                    case Instruction.DrawCard:
                        _abilityController.DrawCard();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public enum Instruction
    {
        Damage = 0x00,
        Buff = 0x01,
        Heal = 0x02,
        Literal = 0x03,
        DrawCard = 0x04,
        Summon = 0x05
    }

    public enum TargetAmount
    {
        All = 10,
        RandomAlly = 11,
        RandomEnemy = 12,
        AllAllies = 13,
        AllEnemies = 14,
        EnemyPlayer = 15,
        AllAliesByType = 16,
    }
}
