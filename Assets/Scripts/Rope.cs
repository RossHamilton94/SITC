// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rope : MonoBehaviour
{
    //============================
    //==	Physics Based 3D Rope ==
    //==	File: Rope_Tube.js	==
    //==	By: Jacob Fletcher	==
    //==	Use and alter Freely	==
    //============================
    //To see other things I have created, visit me at www.reverieinterative.com
    //How To Use:
    // ( BASIC )
    // 1. Simply add this script to the object you want a rope teathered to
    // 3. Assign the other end of the rope as the "Target" object in this script
    // 4. Play and enjoy!
    // (About Character Joints)
    // Sometimes your rope needs to be very limp and by that I mean NO SPRINGY EFFECT.
    // In order to do this, you must loosen it up using the swingAxis and twist limits.
    // For example, On my joints in my drawing app, I set the swingAxis to (0,0,1) sense
    // the only axis I want to swing is the Z axis (facing the camera) and the other settings to around -100 or 100.
    public Transform target;
    public Material material;
    public float ropeWidth = 0.5f;
    public float resolution = 0.5f;
    public float ropeDrag = 0.1f;
    public float ropeMass = 0.5f;
    public float radialSegments = 6;
    public bool startRestrained = true;
    public bool endRestrained = false;
    public bool useMeshCollision = false;

    // Private Variables (Only change if you know what your doing)
    private Vector3[] segmentPos;
    private GameObject[] joints;
    private GameObject tubeRenderer;
    private TubeRenderer line;
    private int segments = 4;
    private bool rope = false;

    // Joint Settings
    public Vector3 swingAxis = new Vector3(0, 1, 0);
    public float lowTwistLimit = 0.0f;
    public float highTwistLimit = 0.0f;
    public float swing1Limit = 20.0f;


    void OnDrawGizmos()
    {
        if (target)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawWireSphere((transform.position + target.position) / 2, ropeWidth);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ropeWidth);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target.position, ropeWidth);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ropeWidth);
        }
    }

    void Awake()
    {
        if (target)
        {
            BuildRope();
        }
        else
        {
            Debug.LogError("You must have a gameobject attached to target: " + this.name, this);
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("Reset rope"))
        {
            Reset();
            BuildRope();
        }
    }

    void Reset()
    {
        Destroy(target.gameObject.GetComponent<CharacterJoint>());

        for (int i = 0; i < joints.Length; i++)
        {
            Destroy(joints[i].gameObject);
        }

        Destroy(tubeRenderer.gameObject);

    }

    void LateUpdate()
    {
        if (target)
        {
            // Does rope exist? If so, update its position
            if (rope)
            {
                line.SetPoints(segmentPos, ropeWidth, Color.white);
                line.enabled = true;
                segmentPos[0] = transform.position;
                for (int s = 1; s < segments; s++)
                {
                    segmentPos[s] = joints[s].transform.position;
                }
            }
        }
    }

    void BuildRope()
    {
        tubeRenderer = new GameObject("TubeRenderer_" + gameObject.name);
        line = tubeRenderer.AddComponent<TubeRenderer>();
        line.useMeshCollision = useMeshCollision;
        // Find the amount of segments based on the distance and resolution
        // Example: [resolution of 1.0f = 1 joint per unit of distance]
        segments = (int)(Vector3.Distance(transform.position, target.position) * resolution);
        if (material)
        {
            material.SetTextureScale("_MainTex", new Vector2(1, segments + 2));
            if (material.GetTexture("_BumpMap"))
                material.SetTextureScale("_BumpMap", new Vector2(1, segments + 2));
        }
        line.vertices = new TubeRenderer.TubeVertex[segments];
        line.crossSegments = (int)radialSegments;
        line.material = material;
        segmentPos = new Vector3[segments];
        joints = new GameObject[segments];
        segmentPos[0] = transform.position;
        segmentPos[segments - 1] = target.position;
        // Find the distance between each segment
        float segs = segments - 1;
        Vector3 seperation = ((target.position - transform.position) / segs);
        for (int s = 0; s < segments; s++)
        {
            // Find the each segments position using the slope from above
            Vector3 vector = (seperation * s) + transform.position;
            segmentPos[s] = vector;
            //Add Physics to the segments
            AddJointPhysics(s);
        }
        // Attach the joints to the target object and parent it to this object 
        CharacterJoint end = target.gameObject.AddComponent<CharacterJoint>();
        end.connectedBody = joints[joints.Length - 1].transform.GetComponent<Rigidbody>();
        end.swingAxis = swingAxis;

        // end.lowTwistLimit.limit = lowTwistLimit;
        // end.highTwistLimit.limit = highTwistLimit;
        // end.swing1Limit.limit = swing1Limit;

        SoftJointLimit ltl = new SoftJointLimit();
        ltl = end.lowTwistLimit;
        ltl.limit = lowTwistLimit;
        end.lowTwistLimit = ltl;

        SoftJointLimit htl = new SoftJointLimit();
        htl = end.highTwistLimit;
        htl.limit = highTwistLimit;
        end.highTwistLimit = htl;

        SoftJointLimit s1l = new SoftJointLimit();
        s1l = end.swing1Limit;
        s1l.limit = swing1Limit;
        end.swing1Limit = s1l;





        target.parent = transform;

        if (endRestrained)
        {
            end.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (startRestrained)
        {
            transform.GetComponent<Rigidbody>().isKinematic = true;
        }
        // Rope = true, The rope now exists in the scene!
        rope = true;
    }

    void AddJointPhysics(int n)
    {
        joints[n] = new GameObject("Joint_" + n);
        joints[n].transform.parent = transform;

        Rigidbody rigid = joints[n].AddComponent<Rigidbody>();
        if (!useMeshCollision)
        {
            SphereCollider col = joints[n].AddComponent<SphereCollider>();
            col.radius = ropeWidth;
        }

        CharacterJoint ph = joints[n].AddComponent<CharacterJoint>();
        ph.swingAxis = swingAxis;

        SoftJointLimit ltl = new SoftJointLimit();
        ltl = ph.lowTwistLimit;
        ltl.limit = lowTwistLimit;
        ph.lowTwistLimit = ltl;

        SoftJointLimit htl = new SoftJointLimit();
        htl = ph.highTwistLimit;
        htl.limit = highTwistLimit;
        ph.highTwistLimit = htl;

        SoftJointLimit s1l = new SoftJointLimit();
        s1l = ph.swing1Limit;
        s1l.limit = swing1Limit;
        ph.swing1Limit = s1l;

        //ph.breakForce = ropeBreakForce; <--------------- TODO
        joints[n].transform.position = segmentPos[n];
        rigid.drag = ropeDrag;
        rigid.mass = ropeMass;
        if (n == 0)
        {
            ph.connectedBody = transform.GetComponent<Rigidbody>();
        }
        else
        {
            ph.connectedBody = joints[n - 1].GetComponent<Rigidbody>();
        }
    }
}

