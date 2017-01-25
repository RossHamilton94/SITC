using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class TransformOverTime
{
    public Transform waypoint;

    [Range(0.0f, 5.0f)]
    public float timer = 1.0f;
}

public class SceneTransition : MonoBehaviour
{
    public TransformOverTime[] waypoints;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator MoveOverSeconds(Transform actor, Vector3 end, float seconds, int index)
    {
        float elapsedTime = 0;
        Vector3 startingPos = actor.transform.position;
        while (elapsedTime < seconds)
        {
            actor.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        actor.position = end;

        if (waypoints.Length > index + 1)
        {
            StartCoroutine(MoveOverSeconds(actor, waypoints[index + 1].waypoint.position, waypoints[index + 1].timer, index + 1));
        }
        else
        {
            CutsceneManager.LetterboxEnd();
        }
    }

    internal void Play()
    {
        int index = 1;
        StartCoroutine(MoveOverSeconds(GameObject.FindGameObjectWithTag("Boss").transform, waypoints[index].waypoint.position, waypoints[index].timer, index));
    }
}