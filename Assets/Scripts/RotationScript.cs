using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {
    GameObject target;
    public float speed;

	// Use this for initialization
	void Start () {
        target = GameObject.Find("Cursor");

	}
	
	// Update is called once per frame
	void Update ()
    {
        target.transform.Rotate(Vector3.up * speed * Time.deltaTime);
	}
}
