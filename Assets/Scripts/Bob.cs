using UnityEngine;
using System.Collections;

public class Bob : MonoBehaviour
{

    float originalY;
    public float floatStrength = 1;

    public bool startOffset = true;
    private float floatDelay = 1.0f;

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
            Debug.Log("I am bobbing " + this.transform.name);
            transform.position = new Vector3(transform.position.x,
                originalY + ((float)Mathf.Sin(Time.time) * floatStrength),
                transform.position.z);
        }
    }
}
