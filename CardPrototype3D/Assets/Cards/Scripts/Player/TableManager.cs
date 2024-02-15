using System.Collections;
using Cards;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public FieldType type;
    
    [SerializeField] private Transform[] _cardSlots;
    private Card[] _cardsOnTable;
    
    private void Start()
    {
        _cardsOnTable = new Card[_cardSlots.Length];
    }
    
    
    public bool SetCardOnTable(Card card)
    {
        //TODO получение урона как в игре 
        if (card == null) return true;

        //Находим пустое место в руке
        var index = GetLastPositiom();

        if (index == -1)
        {
            Destroy(card.gameObject);
            return false;
        }
            
        _cardsOnTable[index] = card;
        StartCoroutine(MoveOnTable(card, _cardSlots[index]));
        return true;
    }
    
    
    private int GetLastPositiom()
    {
        for (int i = 0; i < _cardsOnTable.Length; i++)
        {
            if (_cardsOnTable[i] == null) return i;
        }

        return -1;
    }
    
    private IEnumerator MoveOnTable(Card card, Transform target)
    {
        card.SwitchVisual();
        var time = 0f;
        var startPos = card.transform.position;
        var endPos = target.position;

        while (time < 1f)
        {
            card.transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }

        card.State = CardStateType.OnTable;
    }
    
}
