using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Game
{
    public class GameManager : MonoBehaviour
    {
        private GameState _gameState;

        [SerializeField] private Player _firstPlayer;
        [SerializeField] private Player _secondPlayer;
        [SerializeField] private CardManager _cardManager;
        [SerializeField] private Button _endTurnButton; 
        
        private void Awake()
        {
            _cardManager.InitDeck(_firstPlayer);
            _cardManager.InitDeck(_secondPlayer);
            
            _gameState = GameState.FirstPlayerPreparation;
            _endTurnButton.onClick.AddListener(EndTurn);            
        }

        private void Start()
        {
            PrepareGame(_firstPlayer);            
        }

        private void PrepareGame(Player player)
        {
            _cardManager.DealCardsForSelect(player);
        }

        private void EndTurn()
        {
            switch (_gameState)
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
            Debug.Log(_gameState.ToString());
        }

        private IEnumerator OnFirstPlayerPreparationFinish()
        {
            yield return _cardManager.ChangeCards(_firstPlayer);
            _cardManager.HideCards(_gameState, _firstPlayer);
            Debug.Log("2nd player prep state");
            _gameState = GameState.SecondPlayerPreparation;
        }

        private IEnumerator OnSecondPlayerPreparationFinish()
        {
            yield return _cardManager.ChangeCards(_secondPlayer);
            
            Debug.Log("Flip all cards, wait game start in 2 sec");
            _cardManager.HideCards(_gameState, _secondPlayer);
            yield return new WaitForSeconds(2);
            
            Debug.Log("Starting game");
            Debug.Log("Flip p1 cards");
            
            _gameState = GameState.FirstPlayerTurn;
            _cardManager.StartGame(true);
                        
            yield return null;
        }

        private void ProcessPlayerTurn()
        {            
            var isFirstPlayerTurn = _gameState == GameState.FirstPlayerTurn;
            // Current turn section
            _cardManager.HideCards(_gameState, _firstPlayer);
            
            // Trigger that turn is switching
            EventManager.CallSwitchTurn(isFirstPlayerTurn);

            // End of the turn
            // Pass turn to another player
            _gameState = _gameState == GameState.FirstPlayerTurn
                ? GameState.SecondPlayerTurn
                : GameState.FirstPlayerTurn;
        }
    }
}