using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private PlayerHand _playerOne;
        [SerializeField] private PlayerHand _playerTwo;
        [SerializeField] private float _offset = 0.01f;

        [SerializeField] private Transform _camera;
        [SerializeField] private Transform _pointOne;
        [SerializeField] private Transform _pointTwo;

        [SerializeField] private TableManager _tableOne;
        [SerializeField] private TableManager _tableTwo;
        [SerializeField] private PlayerSwitcher _playerSwitcher;

        private Material _baseMat;
        private CardPropertiesData[] _allCards;
        private Card[] _playerOneCards;
        private Card[] _playerTwoCards;

        private void Awake()
        {
            IEnumerable<CardPropertiesData> cards = new List<CardPropertiesData>();

            foreach (var pack in _packs) cards = pack.UnionProperties(cards);

            _allCards = cards.ToArray();

            _baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
            _baseMat.renderQueue = 2995;
        }

        private void Start()
        {
            _playerOneCards = CreateDeck(_deckOneParent);
            _playerTwoCards = CreateDeck(_deckTwoParent);
        }

        //Берем карту по нажатию на пробел
        private void Update()
        {
            if(_playerSwitcher.firstPlayerMove)
            {
                GetCardsInHand(_playerOneCards, _playerOne);
            }
            else
            {
                GetCardsInHand(_playerTwoCards, _playerTwo);
            }
        }

        private void GetCardsInHand(Card[] cards, PlayerHand playerHand)
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Card index = null;

                for (int i = cards.Length - 1; i >= 0; i--)
                {
                    if (cards[i] != null)
                    {
                        index = cards[i];
                        cards[i] = null;
                        break;
                    }
                }

                playerHand.SetNewCard(index);
            }
        }

        //Создание карт для игроков
        private Card[] CreateDeck(Transform parent)
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
                var random = _allCards[Random.Range(0, _allCards.Length)];
                var picture = new Material(_baseMat);
                picture.mainTexture = random.Texture;
                deck[i].Configuration(picture, random, CardUtility.GetDescriptionById(random.Id));
            }

            return deck;
        }
    }
}