using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.tag);
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerController>().Respawn(false);
        }
    }
}
