using UnityEngine;
using System.Collections;

public class Bob : MonoBehaviour
{

    float originalY;
    public float floatStrength = 1;

    public bool startOffset = true;
    private float floatDelay = 1.0f;

    //Generate a minimum value that it cannot "Bob" below.
    public float minValue;
    RaycastHit hit;


    void Start()
    {
        this.originalY = this.transform.position.y;
        if (startOffset)
        {
            floatDelay = Random.Range(0.5f, 2.0f);
            StartCoroutine(BobUpdate());
        }
    }
    IEnumerator BobUpdate()
    {
        yield return new WaitForSeconds(floatDelay);
        startOffset = false;
    }

    void Update()
    {
        if (!startOffset)
        {
            Physics.Raycast(new Ray(transform.position, Vector3.down), out hit);
            minValue = hit.point.y + 5.0f;

            Mathf.Clamp(originalY + ((float)Mathf.Sin(Time.time) * floatStrength), minValue, minValue * 2.0f);

            // Debug.Log("I am bobbing " + this.transform.name);
            transform.position = new Vector3(transform.position.x,
                originalY + ((float)Mathf.Sin(Time.time) * floatStrength),
                transform.position.z);
        }
    }
}
