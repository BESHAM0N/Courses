using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    //[SerializeField] private CardManager _cardManager;
    [Space, SerializeField] public Transform _deckParent;

    [SerializeField] private int _health = 30;
    [SerializeField] private int _maxManaPool = 10;

    [SerializeField] private TextMeshPro _currentManaText;
    [SerializeField] private TextMeshPro _manaPoolText;

    private int _currentMana = 1;
    private int _manaPool = 1;

    private Card[] _cardsDeck;
    private PlayerHand _cardsInHand = new();

    private void Awake()
    {
        //_cardsDeck = _cardManager.CreateDeck(_deckParent);
        Debug.Log("_cardsDeck ЗАПОЛНЕН для" + gameObject.name);
        UpdateManaText();
    }

    public void InitDeck(Card[] cards)
    {
        _cardsDeck = cards;
    }

    public Card GetNextCardFromDeck()
    {
        Card card = null;        
        for (int i = _cardsDeck.Length - 1; i >= 0; i--)
        {
            if (_cardsDeck[i] == null) continue;

            card = _cardsDeck[i];
            _cardsDeck[i] = null;
            break;
        }

        return card;
    }

    private void UpdateManaText()
    {
        _currentManaText.text = _currentMana.ToString();
        _manaPoolText.text = _manaPool.ToString();
    }

    public void IncreaseManaPool()
    {
        // Increase mana pool if not max
        if (_manaPool != _maxManaPool)
        {
            _manaPool++;
        }

        RestoreMana(_manaPool);
        UpdateManaText();
    }

    private void RestoreMana(int amount)
    {
        _currentMana = amount;
    }

    public void ShuffleInCard(Card card)
    {
        for (int i = 0; i < _cardsDeck.Length - 1; i++)
        {
            if (_cardsDeck[i] == null)
                _cardsDeck[i] = card;
        }

        RandomExtensions.Shuffle(_cardsDeck);
    }


    public IEnumerator MoveCardsToHand()
    {
        yield return _cardsInHand.MoveCardsToHand();
    }

    public void TakeCardForSelect(Card card, int i)
    {
        _cardsInHand.TakeCardForSelect(card, i);
    }

    public IEnumerable<int> GetSelectorIndexToChange()
    {
        return _cardsInHand.GetSelectorIndexToChange();
    }

    public Card GetCardFromSelectorByIndex(int index)
    {
        return _cardsInHand.GetCardFromSelectorByIndex(index);
    }
}