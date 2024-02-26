using Cards;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static readonly UnityEvent<bool, int, Card> DecreasePlayerMana = new();
    public static readonly UnityEvent<bool> TurnSwitch = new();
    public static readonly UnityEvent Test = new(); 

    public static void CallInitMaxCountObstacle(bool playerOne, int manaCost, Card card) =>    
        DecreasePlayerMana?.Invoke(playerOne, manaCost, card);
    
    

    public static void CallSwitchTurn(bool isFirstPlayerTurn) =>    
        TurnSwitch?.Invoke(isFirstPlayerTurn);
}
