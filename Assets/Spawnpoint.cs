using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour
{
    public float debugSize = 2.0f;

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
        Gizmos.DrawWireCube(this.transform.position, Vector3.one * debugSize);
    }
}
