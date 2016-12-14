using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
      

    // Use this for initialization
    void Start()
    {
        transform.LookAt(Camera.main.transform, transform.up);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
