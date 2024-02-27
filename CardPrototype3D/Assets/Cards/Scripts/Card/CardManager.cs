using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards.Extensions;
using UnityEngine;
using Cards.ScriptableObjects;
using Random = UnityEngine.Random;
using Cards.Game;
using Assets.Cards.Scripts.Game;

namespace Cards
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private CardPackConfiguration[] _packs;
        [SerializeField, Range(5, 100)] private int _deckSize = 30;
        [SerializeField] private Card _cardPrefab;

        [Space, SerializeField] private Transform _deckOneParent;
        [Space, SerializeField] private Transform _deckTwoParent;
        [SerializeField] private float _offset = 0.01f;

        [SerializeField] private PlayerHand _playerOneHand;
        [SerializeField] private PlayerHand _playerTwoHand;
        [SerializeField] private VirtualMachine _vm;
        private Card[] _playerOneDeck;
        private Card[] _playerTwoDeck;

        private Material _baseMat;
        private CardPropertiesData[] _allCards;

        public const int INIT_CARDS_COUNT = 4;

        private void Awake()
        {
            IEnumerable<CardPropertiesData> cards = new List<CardPropertiesData>();
            foreach (var pack in _packs)
                cards = pack.UnionProperties(cards);
            _allCards = cards.ToArray();
            _baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
            _baseMat.renderQueue = 2995;

        }

        public void SpawnCard(int id)
        {
            var parent = GameManager.IsFirstPlayerTurn
                ? _deckOneParent
                : _deckTwoParent;

            var card = Instantiate(_cardPrefab, parent);
            var cardProp = _allCards.FirstOrDefault(x => x.Id == id);
            var picture = new Material(_baseMat);
            picture.mainTexture = cardProp.Texture;
            card.Configuration(picture, cardProp, CardUtility.GetDescriptionById(cardProp.Id));
            var hand = GetPlayerHand();
            hand.Spawn(card);
        }

        private void Start()
        {
            _playerOneDeck = CreateDeck(_deckOneParent);
            _playerTwoDeck = CreateDeck(_deckTwoParent);
        }

        /// <summary>
        /// Создание колоды карт.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public Card[] CreateDeck(Transform parent)
        {
            var deck = new Card[_deckSize];
            var offset = new Vector3(0, 0, 0);

            for (int i = 0; i < _deckSize; i++)
            {
                deck[i] = Instantiate(_cardPrefab, parent);
                if (deck[i].IsFromSide)
                {
                    deck[i].SwitchVisual();
                }

                deck[i].transform.localPosition = offset;
                offset.y += _offset;

                //TODO Заменить на список из 30 карт (список айдишников)
                var card = _allCards[Random.Range(0, _allCards.Length)];
                var picture = new Material(_baseMat);
                picture.mainTexture = card.Texture;
                deck[i].Configuration(picture, card, CardUtility.GetDescriptionById(card.Id));
            }

            return deck;
        }

        // TODO: переписать, чтобы использовался один метод
        public void DealCardsForSelect(int cardsToDealCount = INIT_CARDS_COUNT)
        {
            var hand = GetPlayerHand();
            for (int i = 0; i < cardsToDealCount; i++)
            {
                var card = GetNextCardFromDeck();
                hand.TakeCardForSelect(card, i);
            }
        }

        public void DealCard()
        {
            var hand = GetPlayerHand();
            var card = GetNextCardFromDeck();
            card.State = CardStateType.InHand;
            StartCoroutine(hand.TakeCard(card));
        }

        public IEnumerator ChangeCards()
        {
            var playerHand = GetPlayerHand();
            var toChange = playerHand.GetSelectorIndexToChange();

            foreach (var index in toChange)
            {
                // draw card
                var card = playerHand.GetCardFromSelectorByIndex(index);
                card.SwitchVisual();
                card.transform.localPosition = new Vector3(0, 0, 0);
                card.ResetColor();

                // take new
                var newCard = GetNextCardFromDeck();
                playerHand.TakeCardForSelect(newCard, index);

                // shuffle back
                ShuffleInCard(card);
            }
            yield return playerHand.MoveCardsToHand();

            if (GameManager.GameState == GameState.FirstPlayerTurn)
            {
                yield return new WaitForSeconds(1);
                DealCardsForSelect();
                yield return new WaitForSeconds(1);
            }
            yield return null;
        }

        public void ShuffleInCard(Card card)
        {
            var deck = GetPlayerDeck();
            for (int i = 0; i < deck.Length - 1; i++)
            {
                if (deck[i] == null)
                    deck[i] = card;
            }

            RandomExtensions.Shuffle(deck);
        }

        private Card GetNextCardFromDeck()
        {
            var cards = GetPlayerDeck();
            Card card = null;
            for (int i = cards.Length - 1; i >= 0; i--)
            {
                if (cards[i] == null)
                    continue;

                card = cards[i];
                cards[i] = null;
                break;
            }

            return card;
        }

        public void HideCards(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.FirstPlayerPreparation:
                    _playerOneHand.SetTurn(false);
                    break;
                case GameState.SecondPlayerPreparation:
                    _playerTwoHand.SetTurn(false);
                    break;
                case GameState.FirstPlayerTurn:
                    _playerOneHand.SetTurn(false);
                    _playerTwoHand.SetTurn(true);
                    break;
                case GameState.SecondPlayerTurn:
                    _playerOneHand.SetTurn(true);
                    _playerTwoHand.SetTurn(false);
                    break;
                case GameState.Ending:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
        }

        public void StartGame()
        {
            var hand = GetPlayerHand();
            hand.CalculateMana(5);
            hand.SetTurn(true);
        }

        private PlayerHand GetPlayerHand()
        {
            return GameManager.GameState == GameState.FirstPlayerPreparation || GameManager.GameState == GameState.FirstPlayerTurn
                ? _playerOneHand
                : _playerTwoHand;
        }

        private Card[] GetPlayerDeck()
        {
            return GameManager.GameState == GameState.FirstPlayerPreparation || GameManager.GameState == GameState.FirstPlayerTurn
                ? _playerOneDeck
                : _playerTwoDeck;
        }

        public void CalculateMana()
        {
            var hand = GetPlayerHand();
            Debug.Log($"Calculating mana for hand: {hand.name}");
            hand.CalculateMana(GameManager.ActivePlayer.CurrentMana);
        }

        public void RemoveCardFromHand(Card card)
        {
            var hand = GetPlayerHand();
            hand.RemoveCardFromHand(card);
        }

        public void DoEffect(Card card)
        {
            var mechanics = card.Mechanics;
            var hasEffect = card.Instructions.Length > 0;
            card.SetCanAttack(false);
            switch (mechanics)
            {
                case Mechanics.None:
                    break;
                case Mechanics.Charge:
                    card.SetCanAttack(true);
                    break;
                case Mechanics.BattleCry:
                    if (hasEffect)
                        _vm.Interpet(card.Instructions, card.Instructions.Length);
                    break;
                case Mechanics.Taunt:
                    break;
                case Mechanics.Buff:
                    if (hasEffect)
                        _vm.Interpet(card.Instructions, card.Instructions.Length);
                    break;
            }
        }

        /// <summary>
        /// Должен вызываться каждый раз когда:<br></br>
        /// 1. Кладется новая карта на стол (там может быть таунт)<br></br>
        /// </summary>
        public void SetCanTakeDamage()
        {
            GetPlayerHand().SetCanTakeDamage();
        }

        public bool HasTaunts(bool isFirstPlayer)
        {
            var table = GetTable(isFirstPlayer);
            return table.Any(x => x.Mechanics == Mechanics.Taunt);        
        }

        private bool GetPlayerHand(bool isFirstPlayer)
        {
            return isFirstPlayer
                ? _playerOneHand
                : _playerTwoHand;
        }

        public List<Card> GetTable(bool isFirstPlayerTurn)
        {
            var opponentHand = isFirstPlayerTurn
                ? _playerOneHand
                : _playerTwoHand;

            return opponentHand.CardsOnTable;
        }
    }
}