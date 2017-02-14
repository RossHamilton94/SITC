using UnityEngine;
using System.Collections;

public class ParticleTrigger : MonoBehaviour {

    GameObject trigger;

    void Start()
    {
        trigger = this.gameObject;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Boss")
        {
            Debug.Log("The Boss is Aligned with the receiver");
        }
        else
        {
            Debug.Log("There is no match");
        }
    }


}
