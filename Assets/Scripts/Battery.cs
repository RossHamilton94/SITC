using UnityEngine;
using System.Collections;

public class Battery : MonoBehaviour {

    public float chargeAmount = 0.25f;
    public Transform audioPool;
    private AudioSource[] audioSources;
    public GameObject collectSound;
    private bool active = true;
    public float destroyTime = 15.0f;

	// Use this for initialization
    void Start()
    { 
        audioSources = audioPool.gameObject.GetComponents<AudioSource>();
        Destroy(gameObject, destroyTime);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Use(PlayerController player)
    {
        //Debug.Log("BATTERY USED");
        if(active)
        {
            active = false;
            player.AddCharge(chargeAmount);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            PlayAudio("mb_coin", "Effects");
            //GameObject tempCollectSound = Instantiate(collectSound, transform) as GameObject;
            //Destroy(tempCollectSound, 1.0f);
            transform.GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 0.0f);
        }
        
    }

    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.transform.tag == "Player")
    //    {
    //        col.GetComponent<PlayerController>().AddCharge(chargeAmount);
    //    }
    //}
     
    #region Audio Controls

    public void PlayAudio(string track, string group)
    {
        //Debug.Log("Playing track: " + track + " on channel: " + group);
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                AudioClip clip = Resources.Load("Sounds/" + track) as AudioClip;
                source.PlayOneShot(clip);
            }
        }
    }

    #endregion  
}
