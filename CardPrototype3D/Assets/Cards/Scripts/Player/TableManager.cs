using System.Collections;
using Cards;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public FieldType type;
    public Transform[] _cardSlots;

    public Card[] _cardsOnTable;

    private void Start()
    {
        _cardsOnTable = new Card[_cardSlots.Length];
    }
    
    public bool SetCardOnTable(Card card)
    {
        //TODO получение урона как в игре 
        if (card == null) return true;

        //Находим пустое место в руке
        var index = GetLastPosition();

        if (index == -1)
        {
            Destroy(card.gameObject);
            return false;
        }

        _cardsOnTable[index] = card;
        StartCoroutine(MoveOnTable(card, _cardSlots[index]));
        return true;
    }

    private int GetLastPosition()
    {
        for (int i = 0; i < _cardsOnTable.Length; i++)
        {
            if (_cardsOnTable[i] == null)
            {
                return i;
            }
        }

        return -1;
    }

    private IEnumerator MoveOnTable(Card card, Transform target, float timing = 1f)
    {
        var time = 0f;
        var startPos = card.transform.position;
        var endPos = target.position;

        while (time < timing)
        {
            card.transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }

        card.State = CardStateType.OnTable;
    }

    public IEnumerator MoveCard(Card from, Vector3 to)
    {
        var initialPos = from.transform.position;

        var time = 0f;

        Debug.Log("Go attack");
        while (time <= 1f)
        {
            from.transform.position = Vector3.Lerp(initialPos, to, time);
            time += Time.deltaTime;
            yield return null;
        }            
    }
}