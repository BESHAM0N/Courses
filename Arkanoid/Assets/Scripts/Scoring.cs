using UnityEngine;

public class Scoring : MonoBehaviour
{
    private int _maxCountObstacle;
    private const int POINT = 1;

    private void Awake()
    {
        EventManager.CheckLevelCompetition.AddListener(CheckLevelCompetition);
        EventManager.InitMaxCountObstacle.AddListener(InitMaxCountObstacle);
    }

    private void InitMaxCountObstacle(int countObstacles)
    {
        _maxCountObstacle = countObstacles;
        Debug.Log($"сколько надо набрать очков: {countObstacles}");
    }

    private void CheckLevelCompetition()
    {
        _maxCountObstacle -= POINT;
        Debug.Log($"сколько осталось очков: {_maxCountObstacle}");
        if (_maxCountObstacle == 0)
        {
            Debug.Log("Поздравляю, ты прошел уровень!");
            EventManager.CallChangeLevel();
        }
    }

    private void OnDisable()
    {
        EventManager.CheckLevelCompetition.RemoveListener(CheckLevelCompetition);
        EventManager.InitMaxCountObstacle.RemoveListener(InitMaxCountObstacle);
    }
}