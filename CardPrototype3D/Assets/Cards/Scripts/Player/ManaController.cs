using Cards;
using UnityEngine;

public class ManaController : MonoBehaviour
{
    [SerializeField] private Player _playerOne;
    [SerializeField] private Player _playerTwo;

    private void Start()
    {
        EventManager.TurnSwitch.AddListener(OnTurnSwitch);
    }

    private void OnTurnSwitch(bool isFirstPlayerTurn)
    {
        var player = isFirstPlayerTurn
            ? _playerOne
            : _playerTwo;

        player.IncreaseManaPool();
    }

    private Player SetPlayer(GameState gameState)
    {
        return gameState == GameState.FirstPlayerTurn ? _playerOne : _playerTwo;
    }    
}