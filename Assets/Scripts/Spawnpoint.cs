using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour
{
    public float debugSize = 2.0f;
    public bool showDirection = false;
    public Color drawColor = Color.green;
    public Color selectionColor = Color.red;
    public enum Shape 
    {
        BOX,
        SPHERE
    }
    public Shape shape;
    public Transform connection = null;

    // Use this for initialization
    void Start() 
    {

    }

    // Update is called once per frame
    void Update() 
    {

    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR 
        if (UnityEditor.Selection.Contains(gameObject))
        {
            Gizmos.color = selectionColor; 
        }
        else
        {
            Gizmos.color = drawColor;
        }
#endif
        switch (shape)
        {
            case Shape.BOX:
                Gizmos.DrawWireCube(this.transform.position, Vector3.one * debugSize);
                break;
            case Shape.SPHERE:
                Gizmos.DrawWireSphere(this.transform.position, debugSize);
                break;
            default:
                break;
        }

        if (showDirection)
        {
            Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
            Gizmos.DrawRay(transform.position, direction);
        }

        if (connection != null)
        {
            Gizmos.DrawLine(transform.position, connection.position);
        }
    }
}
