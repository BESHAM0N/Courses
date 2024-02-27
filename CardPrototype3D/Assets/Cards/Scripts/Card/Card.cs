using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Cards.Scripts.Mechanics;
using Cards.Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    [System.Diagnostics.DebuggerDisplay("NameCostHealth = {CardDebug}")]
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
        IEndDragHandler, IPointerDownHandler, IDragHandler, ICanTakeDamage, ICanAttack
    {
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
        [SerializeField] private Vector3 _animationPositionCard = new Vector3(0f, 2f, -0.6f);
        [SerializeField] private Texture2D _cursor;

        public int CurrentDamage { get; set; }
        public CardStateType State { get; set; }
        public CardPaymentType Payment { get; set; }
        public PlayerTypeCard PlayerTypeCard { get; set; }
        public Mechanics Mechanics { get; private set; }
        public CardUnitType UnitType { get; private set; }
        public int[] Instructions { get; private set; }
        public string Desciption { get; private set; }
        public string CardDebug { get; private set; }
        public bool IsFromSide => _frontCard.activeSelf;
        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        public int ManaCost => _manaCost;
        public bool CanTakeDamage => _canTakeDamage;
        public int DefaultDamage => _defaultDamage;
        public bool CanAttack => _canAttack;

        private Color _initialMatColor;
        private List<BaseEffect> _effects = new();

        private bool _myTurn;
        private bool _enoughMana;
        private bool _canTakeDamage;
        private bool _canAttack;

        private int _maxHealth;
        private int _currentHealth;
        private int _manaCost;

        private void Awake()
        {
            _maxHealth = _defaultHealth;
            CurrentDamage = _defaultDamage;

            _initialMatColor = _frontCard.GetComponent<MeshRenderer>().material.color;
        }

        public void Configuration(Material picture, CardPropertiesData data, string description)
        {
            _maxHealth = data.Health;
            _currentHealth = _maxHealth;
            CurrentDamage = data.Attack;
            Mechanics = data.Mechanics;
            Instructions = data.Instructions;
            Desciption = description.Trim();
            UnitType = data.Type;
            CardDebug = $"{data.Name} {data.Cost} {data.Health}";

            _defaultHealth = data.Health;
            _defaultDamage = data.Attack;
            _picture.sharedMaterial = picture;
            _cost.text = data.Cost.ToString();
            _manaCost = data.Cost;
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

            if (PlayerTypeCard == PlayerTypeCard.TwoPlayerCard)
            {
                _animationPositionCard.z = 1;
                _animationPositionCard.y = 1;
            }
        }

        public void AddEffect(BaseEffect effect)
        {
            _effects.Add(effect);
            effect.SetEffect(this);
        }

        public bool TryToRemoveEffect(BaseEffect effect)
        {
            if (!_effects.Contains(effect))
                return false;
            _effects.Remove(effect);

            return effect.TryToRemoveEffect(this);
        }

        [ContextMenu("Switch Visual")]
        public void SwitchVisual()
        {
            _frontCard.SetActive(!IsFromSide);
        }

        public void UpdateText()
        {
            _damage.text = CurrentDamage.ToString();
            _health.text = CurrentHealth.ToString();
        }

        #region Hover
        public void OnPointerEnter(PointerEventData eventData)
        {

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
        #endregion

        #region Events
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_myTurn)
                return;

            switch (State)
            {
                case CardStateType.InHand:
                    //ивент передающий стойомсть карты в метод DecreaseMana                       

                    Debug.Log("Can be played: " + _enoughMana + " Player: " + GameManager.ActivePlayer.name);
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
        public void OnEndDrag(PointerEventData eventData)
        {
            switch (State)
            {
                case CardStateType.InHand: // Выкладывается на стол
                    if (_enoughMana)
                    {
                        // send event to the game manager
                        EventManager.CallCardPlayed(this);
                    }
                    else
                    {
                        Debug.Log("У игрока недостаточно маны: " + _enoughMana);
                    }
                    break;
                case CardStateType.OnTable: // Атакует оппонента
                    if (_myTurn && _canAttack)
                    {
                        var target = eventData.hovered.FirstOrDefault(x => 
                        {
                            var iCanTakeDamage = x.GetComponent<ICanTakeDamage>();
                            return iCanTakeDamage != null && iCanTakeDamage.CanTakeDamage; 
                        });

                        if (target != null)
                        {                            
                            StartCoroutine(AttackAnimation(target));
                        }
                    }
                    else
                    {
                        Debug.Log($"Не могу атаковать: _myTurn: {_myTurn} _enoughMana:{_enoughMana} canattack: {_canAttack}");
                    }

                    break;
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            switch (State)
            {
                case CardStateType.InSelector:
                    State = CardStateType.ToChange;
                    ResetColor(Color.red);
                    break;
                case CardStateType.ToChange:
                    State = CardStateType.InSelector;
                    ResetColor();
                    break;
                default:
                    break;
            }

            Debug.Log($"CanBeAttacked: {_canTakeDamage} _enoughMana: {_enoughMana} can attack:{_canAttack} MyTurn: {_myTurn} {Mechanics} \n {Desciption}");
        }
        public void OnDrag(PointerEventData eventData)
        {
            //TODO: card follow cursor
        }
        #endregion

        public bool TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Death();
                return true;
            }

            _health.text = CurrentHealth.ToString();
            return false;
        }

        public void ResetColor(Color? color = null)
        {
            color ??= _initialMatColor;
            _frontCard.GetComponent<MeshRenderer>().material.color = color.Value;
        }

        public void Heal(int amount)
        {
            _currentHealth += amount;

            if (_currentHealth >= MaxHealth)
                _currentHealth = MaxHealth;
        }

        public void IncreaseHealth(int amount)
        {
            _currentHealth += amount;
        }

        public void Death()
        {
            Debug.Log($"Card: {CardDebug} is dead. Destroying GameObject");
            // More logic...
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        /// <summary>
        /// Устанавливает значение моего хода. Переворачивает карты, если они в руке и не мои.
        /// </summary>
        /// <param name="myTurn"></param>
        public void SetMyTurn(bool myTurn)
        {
            _myTurn = myTurn;

            if (State != CardStateType.OnTable)
                _frontCard.SetActive(myTurn);
        }

        public void SetEnoughMana(int currentMana)
        {
            _enoughMana = currentMana - _manaCost >= 0;
        }

        public void SetCanAttack(bool canAttack)
        {
            _canAttack = canAttack;
            Debug.Log($"Can attack: {canAttack} for {CardDebug} {State}");
        }

        public void SetCanTakeDamage(bool canTakeDamage) => _canTakeDamage = canTakeDamage;

        #region Coroutines
        public IEnumerator AttackAnimation(GameObject targetGameObject)
        {
            //TODO: move to another place
            var initPosition = transform.position;
            var targetPosition = targetGameObject.transform.position;

            var targetDamageble = targetGameObject.GetComponent<ICanTakeDamage>();
            var targetCanAttack = targetGameObject.GetComponent<ICanAttack>();

            // Move to the enemy
            yield return MoveCard(this, targetPosition);

            // Target takes damage
            targetDamageble.TakeDamage(CurrentDamage);

            // Take damage from target
            if (!TakeDamage(targetCanAttack.CurrentDamage))
            {
                // if survived, go back
                yield return MoveCard(this, initPosition);
            }
            // После хода выключаем возможность ходить
            SetCanAttack(false);
        }

        private IEnumerator MoveCard(Card from, Vector3 to)
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
        #endregion
    }
}