using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject _levelOne;
    [SerializeField] private GameObject _levelTwo;

    private void Awake()
    {
        EventManager.ChangeLevel.AddListener(SwitchGameLevel);
    }

    private void SwitchGameLevel()
    {
        _levelOne.SetActive(false);
        _levelTwo.SetActive(true);
        BallStorage.Ball.SetActive(false);
        BallStorage.IsLaunched = false;
        EventManager.CallChangeBallPosition();
    }

    private void OnDisable()
    {
        EventManager.ChangeLevel.RemoveListener(SwitchGameLevel);
    }
}