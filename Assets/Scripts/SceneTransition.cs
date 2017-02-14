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
    private GameObject objectToMove;
    public string tagToSearchFor;
    public TransformOverTime[] waypoints;
    ParticleEmitter emitter;

    GameObject splash;

    // Use this for initialization
    void Start()
    {
        splash = GameObject.Find("WaterSplash");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator MoveOverSeconds(Transform actor, Vector3 end, float seconds, int index)
    {
        float elapsedTime = 0;
        Vector3 startingPos = actor.transform.position;

        //Perform the particle effect during the starting animation.
        /**if (elapsedTime == 0 && startingPos == actor.transform.position)
        {
            //Make the splash happen at the starting position ONCE.
            splash.transform.position = actor.transform.position;
            splash.transform.parent = actor.gameObject.transform;
            //Make it rise up at the same time as the boss.
            splash.GetComponent<ParticleSystem>().startSpeed = elapsedTime / seconds;
        }*/


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
        objectToMove = GameObject.FindGameObjectWithTag(tagToSearchFor);
        StartCoroutine(MoveOverSeconds(objectToMove.transform, waypoints[0].waypoint.position, waypoints[0].timer, 0));
    }
}