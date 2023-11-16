using UnityEngine.Events;

public class EventManager
{
    public static readonly UnityEvent ShortenLife = new();
    public static readonly UnityEvent ChangeBallPosition = new();
    public static readonly UnityEvent<int> InitMaxCountObstacle = new();
    public static readonly UnityEvent CheckLevelCompetition = new();
    public static readonly UnityEvent ChangeLevel = new();

    public static void CallShortenLife()
    {
        if (ShortenLife != null)
            ShortenLife.Invoke();
    }

    public static void CallChangeBallPosition()
    {
        if (ChangeBallPosition != null)
            ChangeBallPosition.Invoke();
    }

    public static void CallInitMaxCountObstacle(int countObstacles)
    {
        if (InitMaxCountObstacle != null)
            InitMaxCountObstacle.Invoke(countObstacles);
    }

    public static void CallCheckLevelCompetition()
    {
        if (CheckLevelCompetition != null) 
            CheckLevelCompetition.Invoke();
    }
    
    public static void CallChangeLevel()
    {
        if (ChangeLevel != null) 
            ChangeLevel.Invoke();
    }
}