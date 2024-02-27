using System.Collections;
using UnityEngine;

namespace Assets.Cards.Scripts.Mechanics
{
    public interface ICanAttack
    {
        public int DefaultDamage { get; }
        public int CurrentDamage { get; }
        public bool CanAttack { get; }
        public void SetCanAttack(bool canAttack);

        public IEnumerator AttackAnimation(GameObject target);

    }
}
