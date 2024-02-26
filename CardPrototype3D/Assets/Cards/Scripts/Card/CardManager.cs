using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards.Extensions;
using UnityEngine;
using Cards.ScriptableObjects;
using Random = UnityEngine.Random;

namespace Cards
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private CardPackConfiguration[] _packs;
        [SerializeField, Range(5, 100)] private int _cardInDeck = 30;
        [SerializeField] private Card _cardPrefab;

        [Space, SerializeField] private Transform _deckOneParent;
        [Space, SerializeField] private Transform _deckTwoParent;

        //[SerializeField] private PlayerHand _playerOneHand;
        //[SerializeField] private PlayerHand _playerTwoHand;
        
        [SerializeField] private float _offset = 0.01f;

        private Material _baseMat;
        private CardPropertiesData[] _allCards;
        public Card[] _playerOneDeck;
        public Card[] _playerTwoDeck;

        private GameState _gameState;

        public const int INIT_CARDS_COUNT = 4;

        private void Awake()
        {
            IEnumerable<CardPropertiesData> cards = new List<CardPropertiesData>();
            foreach (var pack in _packs) cards = pack.UnionProperties(cards);
            _allCards = cards.ToArray();
            _baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
            _baseMat.renderQueue = 2995;
            //EventManager.TurnSwitch.AddListener(OnTurnSwitch);            
        }

        public void InitDeck(Player player)
        {
            var deck = CreateDeck(player._deckParent);
            player.InitDeck(deck);
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
            var deck = new Card[_cardInDeck];
            var offset = new Vector3(0, 0, 0);

            for (int i = 0; i < _cardInDeck; i++)
            {
                deck[i] = Instantiate(_cardPrefab, parent);
                if (deck[i].isFromSide)
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
                
        public void DealCardsForSelect(Player player, int cardsToDealCount = INIT_CARDS_COUNT)
        {
            for (int i = 0; i < cardsToDealCount; i++)
            {
                var card = player.GetNextCardFromDeck();
                player.TakeCardForSelect(card, i);                
            }
        }

        public IEnumerator ChangeCards(Player player)
        {
            var toChange = player.GetSelectorIndexToChange();
            
            foreach (var index in toChange)
            {
                // draw card                
                var card = player.GetCardFromSelectorByIndex(index);
                card.SwitchVisual();
                card.transform.localPosition = new Vector3(0, 0, 0);
                card.ResetColor();

                // take new
                var newCard = player.GetNextCardFromDeck();
                player.TakeCardForSelect(newCard, index);
                
                // shuffle back
                player.ShuffleInCard(card);
            }
            yield return player.MoveCardsToHand();
            
            if (_gameState == GameState.FirstPlayerTurn)
            {
                yield return new WaitForSeconds(1);
                DealCardsForSelect(player);
                yield return new WaitForSeconds(1);
            }
            yield return null;            
        }
        
       

        private static Card GetNextCardFromDeck(Card[] cards)
        {
            Card card = null;
            for (int i = cards.Length - 1; i >= 0; i--)
            {
                if (cards[i] == null) continue;

                card = cards[i];
                cards[i] = null;
                break;
            }

            return card;
        }

        public void HideCards(GameState gameState, Player player)
        {
            switch (gameState)
            {
                case GameState.FirstPlayerPreparation:
                    //_playerOneHand.FlipCards(false);
                    break;
                case GameState.SecondPlayerPreparation:
                    // переворот карт второго игрока
                    //_playerTwoHand.FlipCards(false);
                    break;
                case GameState.FirstPlayerTurn:
                    Debug.Log("1 - да, 2 - нет");
                    
                    //_playerOneHand.FlipCards(false);
                    //_playerTwoHand.FlipCards(true);
                    break;
                case GameState.SecondPlayerTurn:
                    Debug.Log("1 - нет, 2 - да");
                    //_playerOneHand.FlipCards(true);
                    //_playerTwoHand.FlipCards(false);
                    break;
                case GameState.Ending:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
        }

        public void StartGame(bool isFirstPlayer)
        {
            var playerHand = new PlayerHand();
            
            playerHand.FlipCards(true);
        }
    }
}