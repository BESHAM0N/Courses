namespace Assets.Cards.Scripts.Mechanics
{
    public interface ICanTakeDamage
    {
        public int MaxHealth { get; }
        public int CurrentHealth { get; }
        public bool CanTakeDamage { get; }
        public void SetCanTakeDamage(bool canTakeDamage);
        public bool TakeDamage(int damage);
        public void Heal(int amount);
        public void Death();
    }
}
