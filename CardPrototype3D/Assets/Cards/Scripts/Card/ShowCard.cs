using UnityEngine;

public class ShowCard : MonoBehaviour
{
    [SerializeField] Color _color = Color.magenta;
    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawCube(transform.position, new Vector3(1f, 0.02f, 1.2f));
    }
}
