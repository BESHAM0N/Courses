using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static readonly UnityEvent<bool, int> DecreasePlayerMana = new();
    
    public static void CallInitMaxCountObstacle(bool playerOne, int manaCost)
    {
        if (DecreasePlayerMana != null)
            DecreasePlayerMana.Invoke(playerOne, manaCost);
    }
}
