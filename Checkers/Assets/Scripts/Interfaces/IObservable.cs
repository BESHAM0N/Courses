using System;
using System.Threading.Tasks;

public interface IObservable
{
    public Task Serialize(string input);
    public event Action<Coordinate> NextStepReady;
}
