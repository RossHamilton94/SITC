using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour {

    public float lifetime;

    void Awake()
    {
        Destroy(gameObject, lifetime);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
