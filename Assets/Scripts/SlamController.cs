using UnityEngine;
using System.Collections;

public class SlamController : MonoBehaviour
{
    public bool isActive = false;

    void OnCollisionEnter(Collision col)
    {
        string tag = col.transform.tag;
        if (isActive)  {
        switch (tag)
        {
            case "Player":
                   col.transform.GetComponent<PlayerController>().Respawn(true);
            break;
            case "CapturePoint":
                    col.transform.GetComponent<Objective>().TakeDamage();
            break;
            case "Wall":
                Debug.Log("I hit a wall");
            break;
            case "Bridge":
                // Get the bridge rigidbodies and send them flying
                Debug.Log("I hit a bridge"); 
                Rigidbody[] rbarr = col.transform.GetComponents<Rigidbody>();
                Debug.Log("We just hit " rbarr.Length " rigidbodies");
                foreach(Rigidbody rb in rbarr) 
                {
                    rb.isKinematic = false;
                    rb.AddForce(new Vector3(Random.Range(-5.0f, 0.0f), rb.transform.position.y - 1, Random.Range(-5.0f, 0.0f)));
                }
            break; 
            default:
            break;
        }  
        }
    }
}
