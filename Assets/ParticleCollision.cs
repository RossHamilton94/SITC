using UnityEngine;
using System.Collections;

public class ParticleCollision : MonoBehaviour
{
    public string tagToCheck = "";
    public Transform particleSystem;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    { 
            Debug.Log("pew");
            particleSystem.GetComponent<EllipsoidParticleEmitter>().Emit();
      
    }
}
