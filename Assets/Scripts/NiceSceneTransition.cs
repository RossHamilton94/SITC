using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NiceSceneTransition : MonoBehaviour
{
    public static NiceSceneTransition instance;
    public float transitionTime = 1.0f;
    public bool fadeIn;
    public bool fadeOut;
    public Image fadeImg;
    float time = 0f;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
            if (fadeIn)
            {
                fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 1.0f);
            }
        } else
        {
            Destroy(transform.gameObject);
        }
    }

    void OnEnable()
    {
        if (fadeIn)
        {
            StartCoroutine(StartScene());
        }
    }

    //This gets called from anywhere you need to load a new scene
    public void LoadScene(string level)
    {
        StartCoroutine(EndScene(level));
    }

    IEnumerator StartScene()
    {
        time = 1.0f;
        yield return null;
        while (time >= 0.0f)
        {
            fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, time);
            time -= Time.deltaTime * (1.0f / transitionTime);
            yield return null;
        }
        fadeImg.gameObject.SetActive(false);
    }

    IEnumerator EndScene(string nextScene)
    {
        fadeImg.gameObject.SetActive(true);
        time = 0.0f;
        yield return null;
        while (time <= 1.0f)
        {
            fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, time);
            time += Time.deltaTime * (1.0f / transitionTime);
            yield return null;
        }
#if UNITY_5_1
        Application.LoadLevel(nextScene);
#else
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
#endif
        StartCoroutine(StartScene());
    }

}