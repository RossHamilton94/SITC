using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {
    public float spinSpeed = 5.0f;
	// Update is called once per frame
	void FixedUpdate () {
        transform.Rotate(0.0f,spinSpeed, 0.0f);
	}
}
