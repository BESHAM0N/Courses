using Cards;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static readonly UnityEvent<bool, int, Card> DecreasePlayerMana = new();
    
    public static void CallInitMaxCountObstacle(bool playerOne, int manaCost, Card card)
    {
        if (DecreasePlayerMana != null)
            DecreasePlayerMana.Invoke(playerOne, manaCost, card);
    }
}
