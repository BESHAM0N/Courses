using System;
using System.Collections;
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
        IEndDragHandler, IPointerDownHandler
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
        [SerializeField] private float _localScaleCard = 1f;
        [SerializeField] private int _defaultHealth = 1;
        [SerializeField] private int _defaultDamage = 0;
        [SerializeField] private Vector3 _animationPositionCard = new Vector3(0f, 2f, 0f);

        [SerializeField] private Texture2D _cursor;

        private bool _myTurn;
        private Color _initialMatColor;
        
        private TableManager _tableManager;
        private Player _player;

        private int _costCard;
        private LinkedList<BaseEffect> _effects = new LinkedList<BaseEffect>();
        private bool _isDraggable = true;

        private void Awake()
        {
            CurrentHealth = _defaultHealth;
            CurrentDamage = _defaultDamage;

            _initialMatColor = _frontCard.GetComponent<MeshRenderer>().material.color;
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

        public void MyTurn(bool myTurn)
        {
            _myTurn = myTurn;

            if (State != CardStateType.OnTable)
                _frontCard.SetActive(myTurn);
        }

        [ContextMenu("Switch Visual")]
        public void SwitchVisual()
        {
            _frontCard.SetActive(!isFromSide);
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
            Debug.Log(_myTurn);
            if (_myTurn)
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
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_myTurn)
            {
                switch (State)
                {
                    case CardStateType.InHand:
                        transform.localScale /= _localScaleCard;
                        transform.position -= _animationPositionCard;

                        break;
                    case CardStateType.OnTable:
                        break;
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //_offset = transform.position - _camera.ScreenToWorldPoint(eventData.position);

            if (_myTurn)
            {
                switch (State)
                {
                    case CardStateType.InHand:
                        //ивент передающий стойомсть карты в метод DecreaseMana
                        
                        
                        var parent = gameObject.transform.parent.GetComponent<ParentController>();
                        parent.SetNewParent(gameObject.GetComponent<Card>());

                        // есть ли мана на карту, мой ли ход, есть ли место на столе
                        break;
                    case CardStateType.OnTable:

                        if (gameObject.transform.parent.gameObject.layer == 6 ||
                            gameObject.transform.parent.gameObject.layer == 7)
                        {
                            //_cards[0] = gameObject.GetComponent<Card>();
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
            if (_myTurn)
            {
                if (State == CardStateType.OnTable)
                {
                    Debug.Log("ЛОГИКА НАНЕСЕНИЯ УРОНА");
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
                    var targetCard = eventData.hovered.FirstOrDefault(x => x.GetComponent<Card>() != null)
                        ?.GetComponent<Card>();

                    if (targetCard != null)
                    {
                        StartCoroutine(Attack(targetCard));
                    }

                    break;
            }
        }

        /// <summary>
        /// Take damage logic.
        /// </summary>
        /// <param name="damage"></param>
        /// <returns>True if died. False if survived</returns>
        private bool TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                Death();
                return true;
            }

            _health.text = CurrentHealth.ToString();
            return false;
        }

        private void Death()
        {
            // More logic...
            gameObject.SetActive(false);
        }

        private IEnumerator Attack(Card target)
        {
            var initPosition = transform.position;
            var targetPosition = target.transform.position;
            
            // Move to the enemy
            yield return _tableManager.MoveCard(this, targetPosition);

            // Target takes damage
            target.TakeDamage(CurrentDamage);

            // Take damage from target
            if (!TakeDamage(target.CurrentDamage))
            {
                // if survived, go back
                yield return _tableManager.MoveCard(this, initPosition);
            }
        }

        private bool CanPlayCard(int currentPlayerMana)
        {
            return currentPlayerMana - _costCard >= 0;
        }

        public void ResetColor(Color? color = null)
        {
            color ??= _initialMatColor;
            _frontCard.GetComponent<MeshRenderer>().material.color = color.Value;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (State)
            {
                case CardStateType.InDeck:
                    break;
                case CardStateType.InSelector:
                    State = CardStateType.ToChange;
                    ResetColor(Color.red);
                    break;
                case CardStateType.ToChange:
                    State = CardStateType.InSelector;
                    ResetColor();
                    break;
                case CardStateType.InHand:
                    break;
                case CardStateType.OnTable:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.Log($"Card state after click: {State}");
        }
    }
}