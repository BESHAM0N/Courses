using UnityEngine;
public class PlayerFall 
{
    public void CheckPosition(Transform transform)
    {
        if (transform.position.y < 0)
        {
            Debug.Log("Вы упали");
            UnityEditor.EditorApplication.isPaused = true;
        }
    }
}
