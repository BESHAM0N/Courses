using System;
using Cards;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static readonly UnityEvent<Card> OnCardPlayed = new();
    public static readonly UnityEvent TurnSwitch = new();
    public static readonly UnityEvent<Player> OnPlayerDied = new();

    public static void CallCardPlayed(Card card) =>
        OnCardPlayed?.Invoke(card);

    public static void CallSwitchTurn() =>
        TurnSwitch?.Invoke();

    public static void CallPlayerDied(Player player) =>
        OnPlayerDied?.Invoke(player);
    
}
