using UnityEngine;

public class ShowCard : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, new Vector3(1f, 0.02f, 1.1f));
    }
}
