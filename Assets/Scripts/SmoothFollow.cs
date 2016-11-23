
using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{

    public float smooth = 0.15F;
    private Vector3 velocity = Vector3.zero;
    public float clampxmin = -1.0f;
    public float clampxmax = -1.0f;
    public float clampymin = -1.0f;
    public float clampymax = -1.0f;

    // The target we are following
    public Transform target = null;
     

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

        Vector3 point = Camera.main.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        Vector3 clampedPosition = Vector3.SmoothDamp(transform.position, destination, ref velocity, smooth);
        if (clampxmax != -1.0f)
        {
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, clampxmin, clampxmax);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, clampymin, clampymax);
        }
        transform.position = clampedPosition;
    }
}