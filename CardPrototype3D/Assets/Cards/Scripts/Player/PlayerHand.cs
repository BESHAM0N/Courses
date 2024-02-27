using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards.Game;
using UnityEngine;

namespace Cards
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] private Transform[] _cardsInHandPositions;
        [SerializeField] private Transform[] _cardSelectorPositions;

        private List<Card> _cardsInHand;
        private List<Card> _cardsOnTable;
        private Card[] _selectorContainer;

        public List<Card> CardsOnTable => _cardsOnTable;

        private void Awake()
        {
            _cardsInHand = new();
            _cardsOnTable = new();
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
            StartCoroutine(MoveCard(card, _cardSelectorPositions[index], CardStateType.InSelector));
        }

        //Перемещение карты
        private IEnumerator MoveCard(Card card, Transform target, CardStateType cardState,
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
                StartCoroutine(MoveCard(card,
                    _cardsInHandPositions[i],
                    CardStateType.InHand,
                    false,
                    true));
            }

            yield return new WaitForSeconds(2);
            yield return null;
        }

        public IEnumerator TakeCard(Card card)
        {
            Debug.Log("Taking card for player: " + GameManager.ActivePlayer.name);

            int index = _cardsInHand.Count;
            _cardsInHand.Add(card);
            Debug.Log("Index:" + index);
            var target = _cardsInHandPositions[index];

            yield return MoveCard(card,
                    target,
                    CardStateType.InHand,
                    false,
                    true);

            card.SetMyTurn(true);
        }

        public void SetTurn(bool myTurn)
        {
            foreach (var card in _cardsInHand)
            {
                card.SetMyTurn(myTurn);
                card.SetCanAttack(false);
            }

            foreach (var card in _cardsOnTable)
            {
                card.SetMyTurn(myTurn);
                card.SetCanAttack(myTurn);
            }
        }

        public void CalculateMana(int currentMana)
        {
            foreach (var card in _cardsInHand)
            {
                card.SetEnoughMana(currentMana);
            }
        }

        public void RemoveCardFromHand(Card card)
        {
            var index = _cardsInHand.IndexOf(card);
            _cardsOnTable.Add(card);

            for (int i = index; i < _cardsInHand.Count - 1; i++)
            {
                if (i == _cardsInHand.Count - 1)
                {
                    index = i;
                    return;
                }
                var current = _cardsInHand[i];
                var next = _cardsInHand[i + 1];

              //  Debug.Log($"Moving card from {i + 1} to {i}");
                StartCoroutine(MoveCard(next, _cardsInHandPositions[i], CardStateType.InHand, false));

                current = next;
            }
            _cardsInHand.RemoveAt(index);
            //Debug.Log($"Removed card from hand: {index}. {_cardsInHand.Count}");
        }

        public void SetCanTakeDamage()
        {
            var taunts = _cardsOnTable.Where(x => x.Mechanics == Mechanics.Taunt).ToList();
            var other = _cardsOnTable.Except(taunts).ToList();

            //TODO play with hasTaunts to check
            bool hasTaunts = taunts.Any();
            if (hasTaunts)
            {
                taunts.ForEach(x => x.SetCanTakeDamage(true));
                other.ForEach(x => x.SetCanTakeDamage(false));
                Debug.Log($"Has taunts: {taunts.Count}. Other: {other.Count}");
            }
            else
            {
                Debug.Log($"No taunts. All can be attacked: {other.Count}");
                other.ForEach(x => x.SetCanTakeDamage(true));
            }
        }

        public void Spawn(Card card)
        {
            _cardsOnTable.Add(card);
            GameManager.TableManager.SetCardOnTable(card);
        }
    }
}