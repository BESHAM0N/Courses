using Cards;

public class StatsEffect : BaseEffect
{
    public int Damage { get; }
    public int Health { get; }
    
    public StatsEffect(int damage, int health, Card parent, bool permanent, string name = "") : base(parent, permanent, name)
    {
        Damage = damage; Health = health;
    }

    public override void SetEffect(Card target)
    {
        target.IncreaseHealth(Health);
        target.CurrentDamage += Damage;
        target.UpdateText();
    }

    public override bool TryToRemoveEffect(Card target)
    {
        if (Permanent) return false;

        target.IncreaseHealth(-Health);
        target.CurrentDamage -= Damage;

        return true;
    }
}
