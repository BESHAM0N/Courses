using Cards;
public abstract class BaseEffect 
{
    public string Name { get; }
    public Card Parent { get; }
    public bool Permanent { get; }

    public BaseEffect(Card parent, bool permanent, string name = "")
    {
        Parent = parent; Permanent = permanent; Name = name;
    }

    public abstract void SetEffect(Card target);
    public abstract bool TryToRemoveEffect(Card target);
}
