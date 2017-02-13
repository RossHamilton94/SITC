using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LetterboxTransition : MonoBehaviour {

    public Vector3 start;
    public Vector3 end = Vector3.one;
    private bool opening = false, closing = false;

    [Range(0.0f, 5.0f)]
    public float timer = 1.0f;

	// Use this for initialization
	void Start () {        
        start = GetComponent<RectTransform>().anchoredPosition3D;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnEnable()
    {
        CutsceneManager.StartLetterbox += StartLetterbox;
        CutsceneManager.CloseLetterbox += CloseLetterbox;
    }

    private void OnDisable()
    {
        CutsceneManager.StartLetterbox -= StartLetterbox;
        CutsceneManager.CloseLetterbox -= CloseLetterbox;
    }

    public IEnumerator MoveOverSeconds(Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = GetComponent<RectTransform>().anchoredPosition3D;
        while (elapsedTime < seconds)
        {
            GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        GetComponent<RectTransform>().anchoredPosition3D = end;
     
        if (closing) {
            closing = false;
            GameManager.instance.em.ui.ToggleUI(0, true);
        }
    }

    public void StartLetterbox()
    {
        opening = true;
        StartCoroutine(MoveOverSeconds(end, timer));
    }

    public void CloseLetterbox()
    {
        closing = true;
        StartCoroutine(MoveOverSeconds(start, timer));
    }
}
