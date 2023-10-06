using UnityEngine;
public class PlayerFall 
{
    public void CheckPosition(Transform transform)
    {
        if (transform.position.y < 0)
        {
            Debug.Log("Вы упали, нажмите на кнопку restart, чтобы начать игру заново");
            UnityEditor.EditorApplication.isPaused = true;
        }
    }
}
