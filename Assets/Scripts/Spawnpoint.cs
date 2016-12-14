using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour
{
    public float debugSize = 2.0f;
    public bool showDirection = false;
    public enum Shape
    {
        BOX,
        SPHERE
    }
    public Shape shape;

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
        Gizmos.color = Color.green;

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
    }
}
