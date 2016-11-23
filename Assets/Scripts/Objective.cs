using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Objective : MonoBehaviour
{
    private Image percentage_bar;
    public Transform bar_object;
    public bool active = false;
    public bool locked = false;

    // Use this for initialization
    void Start()
    {
        percentage_bar = bar_object.GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            StartCoroutine(SmoothBetweenValues(percentage_bar.fillAmount, 1.0f, 5.0f));
        }
        else
        {
            // Save the progress of the bar in segments so that the player has a 'save point'
            if (!locked)
            {
                if (percentage_bar.fillAmount > 0.75f)
                {
                    StartCoroutine(SmoothBetweenValues(percentage_bar.fillAmount, 0.75f, 2.5f));
                }
                else if (percentage_bar.fillAmount > 0.5f)
                {
                    StartCoroutine(SmoothBetweenValues(percentage_bar.fillAmount, 0.5f, 2.5f));
                }
                else if (percentage_bar.fillAmount > 0.25f)
                {
                    StartCoroutine(SmoothBetweenValues(percentage_bar.fillAmount, 0.25f, 2.5f));
                }
            }
        }
    }

    public IEnumerator SmoothBetweenValues(float start, float end, float time)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            percentage_bar.fillAmount = Mathf.Lerp(start, end, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            if (percentage_bar.fillAmount >= 0.99f)
            {
                locked = true;
                percentage_bar.color = Color.green;
            }
            yield return null;
        }
    }

    public void Reset()
    {
        StopAllCoroutines();
    }

    void OnTriggerStay(Collider col)
    {
    }

    public void TakeDamage()
    {
        
        if(percentage_bar.fillAmount > 0.25f)
        {
            percentage_bar.fillAmount -= 0.25f;
        }
        else
        {
            percentage_bar.fillAmount = 0f;
        }
        locked = false;
        percentage_bar.color = Color.white;
    }
}
