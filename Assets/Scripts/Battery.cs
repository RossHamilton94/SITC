using UnityEngine;
using System.Collections;

public class Battery : MonoBehaviour {

    public float chargeAmount = 0.25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Use(PlayerController player)
    {
        Debug.Log("BATTERY USED");

        player.AddCharge(chargeAmount);
        Destroy(gameObject);
    }

    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.transform.tag == "Player")
    //    {
    //        col.GetComponent<PlayerController>().AddCharge(chargeAmount);
    //    }
    //}
}
