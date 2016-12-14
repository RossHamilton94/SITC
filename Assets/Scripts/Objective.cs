using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Objective : MonoBehaviour
{
    private Image percentage_bar;
    public Transform bar_object;
    public bool active = false;
    public bool locked = false;

    ObjectiveSystem objSystem;
    LineRenderer lineRenderer;

    public Transform audioPool;
    private AudioSource[] audioSources;

    public float currentCharge = 0.0f;
    public Transform beamOrigin;

    // Use this for initialization
    void Start()
    {
        percentage_bar = bar_object.GetComponent<Image>();
        objSystem = GameObject.FindGameObjectWithTag("ObjectiveSystem").GetComponent<ObjectiveSystem>();

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.enabled = false;
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, transform.position);

        audioSources = audioPool.gameObject.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            //StartCoroutine(SmoothBetweenValues(currentCharge, 1.0f, 5.0f));
        }
        else
        {
            // Save the progress of the bar in segments so that the player has a 'save point'
            if (!locked)
            {
                if (currentCharge > 0.75f)
                {
                    StartCoroutine(SmoothBetweenValues(currentCharge, 0.75f, 2.5f));
                }
                else if (currentCharge > 0.5f)
                {
                    StartCoroutine(SmoothBetweenValues(currentCharge, 0.5f, 2.5f));
                }
                else if (currentCharge > 0.25f)
                {
                    StartCoroutine(SmoothBetweenValues(currentCharge, 0.25f, 2.5f));
                }
            }
        }

        percentage_bar.fillAmount = currentCharge;
    }

    public void AddCharge(float chargeToAdd)
    {
        currentCharge = currentCharge + chargeToAdd;
        PlayAudio("LoadingPad", "Effects");
    }

    public IEnumerator SmoothBetweenValues(float start, float end, float time)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            currentCharge = Mathf.Lerp(start, end, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            if (currentCharge >= 0.95f)
            {
                currentCharge = 1.0f;
                locked = true;
                percentage_bar.color = Color.green;
                objSystem.RegisterChargedObj(this);
            }
            yield return null;
        }
    }

    public void Reset()
    {
        StopAllCoroutines();
    }

    void OnTriggerEnter(Collider col)
    {
        // Debug.Log("Hello darkness, my old friend");
        if (col.transform.tag == "Player")
        {
            //PlayAudio("laser_charge", "Effects");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            // Stop playing the charging sound
        }
        // Debug.Log("Goodbye darkness");
    }

    void OnTriggerStay(Collider col)
    {

    }

    public void TakeDamage()
    {
        objSystem.RegisterDechargedObj(this);

        if (currentCharge > 0.25f)
        {
            currentCharge -= 0.25f;
        }
        else
        {
            currentCharge = 0f;
        }
        locked = false;
        percentage_bar.color = Color.white;
    }

    public void Fire(Vector3 bossPos)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, beamOrigin.position);
        lineRenderer.SetPosition(1, bossPos);

        currentCharge = 0.0f;
        percentage_bar.color = Color.blue;
        locked = true;

        Invoke("EndFire", 3.0f);
    }

    private void EndFire()
    {
        lineRenderer.enabled = false;
        locked = false;
    }

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
}
