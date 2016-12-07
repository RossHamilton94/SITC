using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour
{
    public delegate void PlayerDeath(string clip, string channel);
    public static event PlayerDeath OnDeath;

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.transform.tag);
        if (col.transform.tag == "Player")
        {
            if (OnDeath != null) {
                OnDeath("mb_die", "Effects");
            }
            col.transform.GetComponent<PlayerController>().Respawn(false);
        }
    }
}
