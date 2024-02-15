using System.Collections;
using UnityEngine;

namespace Cards
{
    public class PlayerHand : MonoBehaviour
    {
        public FieldType type;
        
        [SerializeField] private Transform[] _positions;
        public Player player;
        private Card[] _cardsInHand;

        private void Start()
        {
            _cardsInHand = new Card[_positions.Length];
        }


        public bool SetNewCard(Card card)
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
            
            _cardsInHand[index] = card;
            StartCoroutine(MoveInHand(card, _positions[index]));
            
            return true;
        }


        private int GetLastPositiom()
        {
            for (int i = 0; i < _cardsInHand.Length; i++)
            {
                if (_cardsInHand[i] == null) return i;
            }

            return -1;
        }

        //Перемещение карты
        private IEnumerator MoveInHand(Card card, Transform target)
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

            card.State = CardStateType.InHand;
        }
    }
}