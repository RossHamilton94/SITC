using UnityEngine;
using System.Collections;

public class SlamController : MonoBehaviour
{
    public bool isActive = false;
    public Transform audioPool;
    private AudioSource[] audioSources;

    public delegate void SlamAttack();
    public static event SlamAttack OnSlam;

    void Start()
    {
        audioSources = audioPool.gameObject.GetComponents<AudioSource>();
    }

    void OnCollisionEnter(Collision col)
    {
        string tag = col.transform.tag;
        if (isActive)
        {
            // Trigger the slam attack shake
            if (OnSlam != null)
            {
                // Emit the fact we've just slammed so our systems can respond
                OnSlam();

                // Play the audio of the slam (welcome to the jam)
                PlayAudio("mb_touch", "Effects");

            }

            // Check what we just hit and act accordingly
            switch (tag)
            {
                case "Player":
                    col.transform.GetComponent<PlayerController>().Respawn(true);
                    isActive = false;
                    break;
                //case "CapturePoint":
                //    col.transform.GetComponent<Objective>().TakeDamage();
                //    isActive = false;
                //    break;
                case "Wall":
                    Debug.Log("I hit a wall");
                    break;
                case "Bridge":
                    // Get the bridge rigidbodies and send them flying (hopefully)
                    Debug.Log("I hit a bridge");
                    Rigidbody[] rbarr = col.transform.GetComponentsInChildren<Rigidbody>();
                    Instantiate(Resources.Load("Prefabs/Explosion"), transform.position, Quaternion.identity);
                    col.transform.GetComponent<BoxCollider>().enabled = false;
                    Debug.Log("We just hit " + rbarr.Length + " rigidbodies");
                    foreach (Rigidbody rb in rbarr)
                    {
                        rb.isKinematic = false;
                        rb.AddForce(new Vector3(Random.Range(-5.0f, 0.0f), rb.transform.position.y + 1, Random.Range(-5.0f, 0.0f)), ForceMode.Impulse);
                    }
                    isActive = false;
                    break;
                default:
                    break;
            }
        }
    }

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
