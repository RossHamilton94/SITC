using UnityEngine;
using System.Collections;

public class Battery : MonoBehaviour {

    public float chargeAmount = 0.25f;
    public Transform audioPool;
    private AudioSource[] audioSources; 

	// Use this for initialization
    void Start()
    { 
        audioSources = audioPool.gameObject.GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Use(PlayerController player)
    {
        Debug.Log("BATTERY USED");
        player.AddCharge(chargeAmount);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        PlayAudio("mb_coin", "Effects");
        Destroy(gameObject, 0.2f);
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
        Debug.Log("Playing track: " + track + " on channel: " + group);
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
