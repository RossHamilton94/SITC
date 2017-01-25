using UnityEngine;
using System.Collections;

public class Bob : MonoBehaviour {

    float originalY;
    public float floatStrength = 1; 

    void Start()
    {
        this.originalY = this.transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x,
            originalY + ((float)Mathf.Sin(Time.time) * floatStrength),
            transform.position.z);
    }
}
