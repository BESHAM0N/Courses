using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
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
                        //var playerMana = transform.parent.GetComponent<PlayerHand>().player.currentManaCount;

                        //ивент передающий стойомсть карты в метод DecreaseMana
                        EventManager.CallInitMaxCountObstacle(PlayerTypeCard == PlayerTypeCard.OnePlayerCard,
                            _costCard);

                        // есть ли мана на карту, мой ли ход, есть ли место на столе
                        break;
                    case CardStateType.OnTable:
                        // может ли атаковать.

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
                    Vector3 newPosition = _camera.ScreenToWorldPoint(eventData.position);
                    newPosition.z = 0;
                    transform.position = newPosition + _offset;
                    Debug.Log("ПЕРЕДВИНУЛИСЬ");
                }
            }
            else
            {
                Debug.Log("ЭТО НЕ ВАШИ КАРТЫ");
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            State = CardStateType.OnTable;


            switch (State)
            {
                case CardStateType.InHand:
                    // если есть клич (класс с логикой клича, то используем его)
                    break;
                case CardStateType.OnTable:
                    if (_isDraggable)
                    {
                        transform.position -= _animationPositionCard;
                        _isDraggable = false;
                    }

                    // нужно ли атаковать, 
                    break;
            }
        }
    }
}