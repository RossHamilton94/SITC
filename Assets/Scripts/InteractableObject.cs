using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    Material interactingMat;
    [SerializeField]
    Material notInteractingMat;
    bool leftInteracting = false;
    bool rightInteracting = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale = new Vector3(2f, 2f, 2f);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "LeftTentacleMain")
        {
            if(!rightInteracting)
                GetComponent<MeshRenderer>().material = interactingMat;
            leftInteracting = true;
            col.GetComponentInParent<BossController>().AddInteractiveObject(transform, true);
        }
        if(col.gameObject.name == "RightTentacleMain")
        {
            if (!leftInteracting)
                GetComponent<MeshRenderer>().material = interactingMat;
            rightInteracting = true;
            col.GetComponentInParent<BossController>().AddInteractiveObject(transform, false);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "LeftTentacleMain")
        {
            if (!rightInteracting)
                GetComponent<MeshRenderer>().material = notInteractingMat;
            leftInteracting = false;
            col.GetComponentInParent<BossController>().RemoveInteractiveObject(transform, true);
        }
        if (col.gameObject.name == "RightTentacleMain")
        {
            if (!leftInteracting)
                GetComponent<MeshRenderer>().material = notInteractingMat;
            rightInteracting = false;
            col.GetComponentInParent<BossController>().RemoveInteractiveObject(transform, false);
        }
    }
}
