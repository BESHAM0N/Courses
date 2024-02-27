using System;
using System.Collections;
using Assets.Cards.Scripts.Mechanics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameState GameState { get; private set; }
        public static Player ActivePlayer { get; private set; }
        public static TableManager TableManager { get; private set; }
        public static bool IsFirstPlayerTurn => GameState == GameState.FirstPlayerTurn;

        [SerializeField] private Player _firstPlayer;
        [SerializeField] private Player _secondPlayer;

        [SerializeField] private TableManager _firstTable;
        [SerializeField] private TableManager _secondTable;
        [SerializeField] private CardManager _cardManager;
        [SerializeField] private Button _endTurnButton;

        private void Awake()
        {
            GameState = GameState.FirstPlayerPreparation;
            _endTurnButton.onClick.AddListener(EndTurn);
            EventManager.OnCardPlayed.AddListener(OnCardPlayed);
            EventManager.OnPlayerDied.AddListener(OnPlayerDied);
        }

        private void OnPlayerDied(Player deadPlayer)
        {
            var alivePlayer = deadPlayer == _firstPlayer 
                ? _secondPlayer 
                : _firstPlayer;
            GameState = GameState.Ending;
            Debug.Log($"Player {alivePlayer.name} has won. Ending game...");
        }

        private void Start()
        {
            PrepareGame();
            ActivePlayer = _firstPlayer;
            TableManager = _firstTable;
        }

        private void OnCardPlayed(Card card)
        {
            ActivePlayer.DecreaseMana(card.ManaCost);
            _cardManager.CalculateMana();
            _cardManager.RemoveCardFromHand(card);
            _cardManager.SetCanTakeDamage();
            var canTakeDamage = _cardManager.HasTaunts(IsFirstPlayerTurn) == false;
            ActivePlayer.SetCanTakeDamage(canTakeDamage);
            TableManager.SetCardOnTable(card);
            _cardManager.DoEffect(card);
        }

        private void PrepareGame()
        {
            _cardManager.DealCardsForSelect();
        }

        private void EndTurn()
        {
            switch (GameState)
            {
                case GameState.FirstPlayerPreparation:
                    StartCoroutine(OnFirstPlayerPreparationFinish());
                    break;
                case GameState.SecondPlayerPreparation:
                    StartCoroutine(OnSecondPlayerPreparationFinish());
                    break;
                case GameState.FirstPlayerTurn:
                    ProcessPlayerTurn();
                    break;
                case GameState.SecondPlayerTurn:
                    ProcessPlayerTurn();
                    break;
                case GameState.Ending:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Debug.Log(GameState.ToString());
        }

        private IEnumerator OnFirstPlayerPreparationFinish()
        {
            yield return _cardManager.ChangeCards();
            _cardManager.HideCards(GameState);

            Debug.Log("2nd player prep state");
            GameState = GameState.SecondPlayerPreparation;
            ActivePlayer = _secondPlayer;
            _cardManager.DealCardsForSelect();
        }

        private IEnumerator OnSecondPlayerPreparationFinish()
        {
            yield return _cardManager.ChangeCards();

            Debug.Log("Flip all cards, wait game start in 2 sec");
            _cardManager.HideCards(GameState);
            yield return new WaitForSeconds(2);

            GameState = GameState.FirstPlayerTurn;
            Debug.Log("Starting game: GameState " + GameState);
            ActivePlayer = _firstPlayer;
            _cardManager.StartGame();

            yield return null;
        }

        private void ProcessPlayerTurn()
        {
            var isFirstPlayerTurn = GameState == GameState.FirstPlayerTurn;
            // CURRENT player turn
            _cardManager.HideCards(GameState);

            // INIT next player turn
            GameState = isFirstPlayerTurn
                ? GameState.SecondPlayerTurn
                : GameState.FirstPlayerTurn;

            ActivePlayer = isFirstPlayerTurn
                ? _secondPlayer
                : _firstPlayer;

            TableManager = isFirstPlayerTurn
                ? _secondTable
                : _firstTable;

            ActivePlayer.IncreaseManaPool();
            _cardManager.DealCard();
            _cardManager.CalculateMana();

            Debug.Log($"Active player:{ActivePlayer.name}. IsFirstPlayer: {IsFirstPlayerTurn}");

            EventManager.CallSwitchTurn();
        }
    }
}