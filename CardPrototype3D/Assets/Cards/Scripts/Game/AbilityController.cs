using System.Collections.Generic;
using System.Linq;
using Assets.Cards.Scripts.Mechanics;
using Cards;
using Cards.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Cards.Scripts.Game
{
    public class AbilityController
    {
        private readonly CardManager _cardManager;

        public AbilityController(CardManager cardManager)
        {
            _cardManager = cardManager;
        }

        public void DrawCard()
        {
            _cardManager.DealCard();
            Debug.Log("Draw card from bytecode");
        }

        public void Summon(int id)
        {
            _cardManager.SpawnCard(id);
            Debug.Log("Summoning card with Id: " + id);
        }

        public void DealDamage(int damage)
        {
            List<Card> targets = _cardManager.GetTable(!GameManager.IsFirstPlayerTurn);
            if (!targets.Any())
            {
                Debug.Log("No target to deal damage");
                return;
            }
            var index = Random.Range(0, targets.Count);
            var target = targets[index];

            target.TakeDamage(damage);
            Debug.Log("Deal damage: " + damage + " on: " + target.name);
        }

        public void Heal(int amount, CardUnitType targetType)
        {
            ICanTakeDamage target = GetTargetByUnitType(targetType);
            if (target == null)
            {
                Debug.LogWarning("Target for heal null.");
                return;
            }
            target.Heal(amount);
            Debug.Log("Healing for " + amount + " Target: " + targetType);
        }

        public void Buff(int damage, int health, bool permanent, CardUnitType targetType)
        {
            var currentTable = _cardManager.GetTable(GameManager.IsFirstPlayerTurn);
            //TODO: if None - random unit
            if (targetType != CardUnitType.None)
            {
                currentTable = currentTable.Where(x => x.UnitType == targetType).ToList();
            }

            if (currentTable.Any())
            {
                var card = currentTable[Random.Range(0, currentTable.Count)];
                card.AddEffect(new StatsEffect(damage, health, card, permanent, $"Buff {targetType}"));
                Debug.Log($"Perm: {permanent} BUFF to {targetType} for dmg {damage} hp {health} to {card.CardDebug}");
            }

        }

        private ICanTakeDamage GetTargetByUnitType(CardUnitType targetType)
        {
            var currentTable = _cardManager.GetTable(GameManager.IsFirstPlayerTurn);
            return currentTable.Where(x => x.UnitType == targetType).FirstOrDefault();
        }
    }
}
