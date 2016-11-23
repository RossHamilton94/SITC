using UnityEngine;
using System.Collections;

public class SlamController : MonoBehaviour
{

    public bool isActive = false;

    void OnCollisionEnter(Collision col)
    {
        if(col.transform.tag == "Player" && isActive)
        {
            col.transform.GetComponent<PlayerController>().Respawn();
        }
        if(col.transform.tag == "CapturePoint" && isActive)
        {
            col.transform.GetComponent<Objective>().TakeDamage();
        }
    }
}
