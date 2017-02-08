using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetRocket : MonoBehaviour
{

    public GameObject rocket;
    private GameObject spawnedRocket = null;
    public Transform fireLocation;
    public Transform target;
    public GameObject bezierFlightPath;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {

    }

    private void OnEnable()
    {
        ObjectiveSystem.OnFire += FireRocket;
    }

    private void OnDisable()
    {
        ObjectiveSystem.OnFire -= FireRocket;
    }

    void FireRocket(Vector3 targetPosition, float travelTime)
    { 
        StartCoroutine(MoveRocket((GameObject)GameObject.Instantiate(rocket, fireLocation.position, Quaternion.identity), travelTime));
    }

    IEnumerator MoveRocket(GameObject g, float travelTime)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < travelTime)
        {
            float reciprocalTime = elapsedTime / travelTime;
            g.transform.position =
                bezierFlightPath.GetComponent<BezierCurve>().GetPointAt(reciprocalTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
