using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Cards
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] private Transform[] _cardsInHandPositions;
        [SerializeField] private Transform[] _cardSelectorPositions;

        private List<Card> _cardsInHand;
        private Card[] _selectorContainer;

        private void Awake()
        {
            _cardsInHand = new();            
            _selectorContainer = new Card[CardManager.INIT_CARDS_COUNT];
            Debug.Log($"{_selectorContainer.Length}");
        }
        
        public List<int> GetSelectorIndexToChange()
        {
            var output = new List<int>();
            for (int i = 0; i < CardManager.INIT_CARDS_COUNT; i++)
            {
                var item = _selectorContainer[i];
                if (item.State == CardStateType.ToChange)
                    output.Add(i);
            }

            return output;
        }

        public Card GetCardFromSelectorByIndex(int index) => _selectorContainer[index];

        public void TakeCardForSelect(Card card, int index)
        {
            Debug.Log("Take card for select start");
            if (card == null)
            {
                Debug.LogWarning("null card!");
                return;
            }

            var go = gameObject;

            if (_selectorContainer == null)
            {
                Debug.Log("_selectorContainer is null");
            }

            // if (_cardSelectorPositions[index] == null)
            // {
            //     // TODO: destroy?
            //     Destroy(card.gameObject);
            //     return;
            // }
            
            Debug.Log($"Using selector container is null:{_selectorContainer == null}");
            _selectorContainer[index] = card;
            StartCoroutine(MoveToSelector(card, _cardSelectorPositions[index], CardStateType.InSelector));
        }

        //Перемещение карты
        private IEnumerator MoveToSelector(Card card, Transform target, CardStateType cardState,
            bool switchVisual = true, bool wait = false)
        {
            if (switchVisual)
                card.SwitchVisual();

            if (wait)
                yield return new WaitForSeconds(1);

            var time = 0f;
            var startPos = card.transform.position;
            var endPos = target.position;

            while (time < 1f)
            {
                card.transform.position = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime;
                yield return null;
            }

            card.State = cardState;
        }

        public IEnumerator MoveCardsToHand()
        {
            for (var i = 0; i < _selectorContainer.Length; i++)
            {
                var card = _selectorContainer[i];
                _cardsInHand.Insert(i, card);
                StartCoroutine(MoveToSelector(card,
                    _cardsInHandPositions[i],
                    CardStateType.InHand,
                    false,
                    true));
            }

            yield return new WaitForSeconds(2);
            
            yield return null;
        }

        public void FlipCards(bool myTurn)
        {
            foreach (var card in _cardsInHand)
            {
                card.MyTurn(myTurn);
            }
        }
        
        
        
        
    }
}