using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Cards
{
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler,
        IEndDragHandler
    {
        public bool isFromSide => _frontCard.activeSelf;

        public CardStateType State { get; set; }
        public int CurrentHealth { get; set; }
        public CardPaymentType Payment { get; set; }
        public int CurrentDamage { get; set; }
        public PlayerTypeCard PlayerTypeCard { get; set; }

        [SerializeField] private GameObject _frontCard;
        [SerializeField] private MeshRenderer _picture;
        [SerializeField] private TextMeshPro _cost;
        [SerializeField] private TextMeshPro _name;
        [SerializeField] private TextMeshPro _description;
        [SerializeField] private TextMeshPro _damage;
        [SerializeField] private TextMeshPro _type;
        [SerializeField] private TextMeshPro _health;
        [SerializeField] private float _localScaleCard = 3f;
        [SerializeField] private int _defaultHealth = 1;
        [SerializeField] private int _defaultDamage = 0;
        [SerializeField] private Vector3 _animationPositionCard = new Vector3(0f, 3f, 0f);
        [SerializeField] private Player _onePlayer;
        [SerializeField] private Player _twoPlayer;
        private Card[] _cards = new Card[2];

        private TableManager _tableManager;

        private Camera _camera;
        private int _costCard;
        private LinkedList<BaseEffect> _effects = new LinkedList<BaseEffect>();
        private Vector3 _offset;
        private bool _isDraggable = true;
        private PlayerSwitcher _playerSwitcher;

        private void Awake()
        {
            _camera = Camera.allCameras[0];
            CurrentHealth = _defaultHealth;
            CurrentDamage = _defaultDamage;
            _playerSwitcher = _camera.GetComponent<PlayerSwitcher>();
        }

        public void AddEffect(BaseEffect effect)
        {
            _effects.AddLast(effect);
            effect.SetEffect(this);
        }

        public bool TryToRemoveEffect(BaseEffect effect)
        {
            if (!_effects.Contains(effect)) return false;
            _effects.Remove(effect);

            return effect.TryToRemoveEffect(this);
        }

        public void Configuration(Material picture, CardPropertiesData data, string description)
        {
            CurrentHealth = data.Health;
            CurrentDamage = data.Attack;
            _defaultHealth = data.Health;
            _defaultDamage = data.Attack;
            _picture.sharedMaterial = picture;
            _cost.text = data.Cost.ToString();
            _costCard = data.Cost;
            _name.text = data.Name;
            _description.text = description;
            _damage.text = data.Attack.ToString();
            _type.text = data.Type == CardUnitType.None ? string.Empty : data.Type.ToString();
            _health.text = data.Health.ToString();

            PlayerTypeCard = gameObject.transform.parent.gameObject.layer switch
            {
                6 => PlayerTypeCard.OnePlayerCard,
                7 => PlayerTypeCard.TwoPlayerCard,
                _ => PlayerTypeCard
            };
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if ((_playerSwitcher.firstPlayerMove && PlayerTypeCard == PlayerTypeCard.OnePlayerCard) ||
                (!_playerSwitcher.firstPlayerMove && PlayerTypeCard == PlayerTypeCard.TwoPlayerCard))
            {
                switch (State)
                {
                    case CardStateType.InHand:
                        transform.localScale *= _localScaleCard;
                        transform.position += _animationPositionCard;
                        break;
                    case CardStateType.OnTable:
                        break;
                }
            }
            else
            {
                Debug.Log("ЭТО НЕ ВАШИ КАРТЫ");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if ((_playerSwitcher.firstPlayerMove && PlayerTypeCard == PlayerTypeCard.OnePlayerCard) ||
                (!_playerSwitcher.firstPlayerMove && PlayerTypeCard == PlayerTypeCard.TwoPlayerCard))
            {
                switch (State)
                {
                    case CardStateType.InHand:
                        transform.localScale /= _localScaleCard;
                        transform.position -= _animationPositionCard;
                        Debug.Log("МЕНЯ ПОДНЯЛИ");
                        break;
                    case CardStateType.OnTable:
                        break;
                }
            }
            else
            {
                Debug.Log("ЭТО НЕ ВАШИ КАРТЫ");
            }
        }

        [ContextMenu("Switch Visual")]
        public void SwitchVisual()
        {
            _frontCard.SetActive(!isFromSide);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _offset = transform.position - _camera.ScreenToWorldPoint(eventData.position);

            if ((_playerSwitcher.firstPlayerMove && PlayerTypeCard == PlayerTypeCard.OnePlayerCard) ||
                (!_playerSwitcher.firstPlayerMove && PlayerTypeCard == PlayerTypeCard.TwoPlayerCard))
            {
                switch (State)
                {
                    case CardStateType.InHand:
                        //ивент передающий стойомсть карты в метод DecreaseMana
                        EventManager.CallInitMaxCountObstacle(PlayerTypeCard == PlayerTypeCard.OnePlayerCard,
                            _costCard, gameObject.GetComponent<Card>());
                        var parent = gameObject.transform.parent.GetComponent<ParentController>();
                        parent.SetNewParent(gameObject.GetComponent<Card>());

                        // есть ли мана на карту, мой ли ход, есть ли место на столе
                        break;
                    case CardStateType.OnTable:

                        if (gameObject.transform.parent.gameObject.layer == 6 ||
                            gameObject.transform.parent.gameObject.layer == 7)
                        {
                            _cards[0] = gameObject.GetComponent<Card>();
                            Debug.Log($"Я ЗАПИСАЛ КАРТУ: {gameObject.name} В СПИСОК ПОД ИНДЕКСОМ: {_cards[0]}");
                        }

                        break;
                }
            }
            else
            {
                Debug.Log("ЭТО НЕ ВАШИ КАРТЫ");
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if ((_playerSwitcher.firstPlayerMove && PlayerTypeCard == PlayerTypeCard.OnePlayerCard) ||
                (!_playerSwitcher.firstPlayerMove && PlayerTypeCard == PlayerTypeCard.TwoPlayerCard))
            {
                if (State == CardStateType.OnTable)
                {
                    Debug.Log("ЛОГИКА НАНЕСЕНИЯ УРОНА");
                }
                else
                {
                }
            }
            else
            {
                Debug.Log("ЭТО НЕ ВАШИ КАРТЫ");
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //State = Payment == CardPaymentType.Cheaply ? CardStateType.OnTable : CardStateType.InHand;

            switch (State)
            {
                case CardStateType.InHand:

                    if (Payment == CardPaymentType.Cheaply)
                    {
                        _tableManager = gameObject.transform.parent.GetComponent<TableManager>();
                        _tableManager.SetCardOnTable(gameObject.GetComponent<Card>());

                        Debug.Log("ПЕРЕДВИНУЛИСЬ");
                    }
                    else
                    {
                        Debug.Log("Не хватает маны");
                    }

                    // если есть клич (класс с логикой клича, то используем его)
                    break;
                case CardStateType.OnTable:
                    // if (_isDraggable)
                    // {
                    //     transform.position -= _animationPositionCard;
                    //     _isDraggable = false;
                    // }
                    var test = eventData.selectedObject;
                    var targetCard = eventData.hovered;
                    var card = targetCard.FirstOrDefault(x => x.GetComponent<Card>() != null).GetComponent<Card>();


                    // if (gameObject.transform.parent.gameObject.layer != targetCard.gameObject.transform.parent.gameObject.layer)
                    // {
                    if (card != null)
                    {
                        TakeDamage(card);
                    }
                    //}

                    // нужно ли атаковать, 
                    break;
            }
        }

        private void TakeDamage(Card target)
        {
            Debug.Log($"HP ПЕРВОГО: {CurrentHealth}, HP ВТОРОГО : {target.CurrentHealth} ");
            Debug.Log($"ДАмаг ПЕРВОГО: {CurrentDamage}, ДАмаг ВТОРОГО : {target.CurrentDamage} ");
            CurrentHealth -= target.CurrentDamage;
            target.CurrentHealth -= CurrentDamage;
            Debug.Log($"HP ПЕРВОГО: {CurrentHealth}, HP ВТОРОГО : {target.CurrentHealth} ");

            if (CurrentHealth <= 0)
            {
                gameObject.SetActive(false);
            }

            if (target.CurrentHealth <= 0)
            {
                target.gameObject.SetActive(false);
            }
        }
    }
}