using System;

public interface ISerializable 
{
    public event Action ObjectsMoved;
    public event Action StepFinished; 
    public event Action<ColorType> GameEnded;
    public event Action<BaseClickComponent> CheckerDestroyed;
}
