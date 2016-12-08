﻿using UnityEngine;
using System.Collections;
using System;

public class PlayerController : Entity
{
    [Serializable]
    public class GroundState
    {
        public GameObject player;
        public float width;
        public float height;
        public float length = 1.0f;

        //GroundState constructor.  Sets offsets for raycasting.
        public GroundState(GameObject playerRef)
        {
            player = playerRef;
            width = player.GetComponentInChildren<Collider>().bounds.extents.x;
            height = player.GetComponentInChildren<Collider>().bounds.extents.y;
        }

        //Returns whether or not player is touching wall.
        public bool isWall()
        {
            bool left = Physics.Raycast(new Vector3(player.transform.position.x - width, player.transform.position.y + (0.5f * height), player.transform.position.z), -Vector2.right, length);
            bool right = Physics.Raycast(new Vector3(player.transform.position.x + width, player.transform.position.y + (0.5f * height), player.transform.position.z), Vector2.right, length);

            Debug.DrawRay(new Vector3(player.transform.position.x - width, player.transform.position.y + (0.5f * height), player.transform.position.z), -Vector2.right, Color.red, 0.1f);
            Debug.DrawRay(new Vector3(player.transform.position.x + width, player.transform.position.y + (0.5f * height), player.transform.position.z), Vector2.right, Color.red, 0.1f);

            Debug.Log("Wall: " + left + " " + right);

            if (left || right)
                return true;
            else
                return false;
        }

        //Returns whether or not player is touching ground.
        public bool isGround()
        {
            bool bottom1 = Physics.Raycast(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), -Vector2.up, length);
            bool bottom2 = Physics.Raycast(new Vector3(player.transform.position.x + (width - 0.2f), player.transform.position.y , player.transform.position.z), -Vector2.up, length);
            bool bottom3 = Physics.Raycast(new Vector3(player.transform.position.x - (width - 0.2f), player.transform.position.y , player.transform.position.z), -Vector2.up, length);

            Debug.DrawRay(new Vector3(player.transform.position.x, player.transform.position.y , player.transform.position.z), -Vector2.up, Color.green, 0.1f);
            Debug.DrawRay(new Vector3(player.transform.position.x + (width - 0.2f), player.transform.position.y , player.transform.position.z), -Vector2.up, Color.green, 0.1f);
            Debug.DrawRay(new Vector3(player.transform.position.x - (width - 0.2f), player.transform.position.y , player.transform.position.z), -Vector2.up, Color.green, 0.1f);

            Debug.Log("Ground: " + (bottom1 || bottom2 || bottom3));

            if (bottom1 || bottom2 || bottom3)
                return true;
            else
                return false;
        }

        //Returns whether or not player is touching wall or ground.
        public bool isTouching()
        {
            if (isGround() || isWall())
                return true;
            else
                return false;
        }

        //Returns direction of wall.
        public int wallDirection()
        {
            RaycastHit left_hi, right_hi;
            bool left, right;
            Ray left_ray = new Ray(new Vector3(player.transform.position.x - width, player.transform.position.y, player.transform.position.z), -Vector2.right);
            Ray right_ray = new Ray(new Vector3(player.transform.position.x + width, player.transform.position.y, player.transform.position.z), Vector2.right);

            Debug.DrawRay(left_ray.origin, left_ray.direction, Color.blue, 0.2f);
            Debug.DrawRay(right_ray.origin, right_ray.direction, Color.blue, 0.2f);

            //bool test_a = 
            Physics.Raycast(left_ray.origin, left_ray.direction, out left_hi, length);
            //bool test_b = 
            Physics.Raycast(right_ray.origin, right_ray.direction, out right_hi, length);

            if (left_hi.transform != null)
            {
                left = true;
            }
            else
            {
                left = false;
            }

            if (right_hi.transform != null)
            {
                right = true;
            }
            else
            {
                right = false;
            }

            // bool left = Physics.Raycast(new Vector2(player.transform.position.x - width, player.transform.position.y), -Vector2.right, length);
            // bool right = Physics.Raycast(new Vector2(player.transform.position.x + width, player.transform.position.y), Vector2.right, length);

            if (left)
                return -1;
            else if (right)
                return 1;
            else
                return 0;
        }
    }

    public float wall_push = 2.5f;
    public float shift_mod_factor = 1.5f;
    public float mod_factor = 1.0f;

    public float capture_speed = 0.001f;

    //private Rigidbody rb;
    private InputController ic;
    public EntityManager em;
    public Transform audioPool;
    private AudioSource[] audioSources;

    private Vector3 spawnPos = Vector3.zero;

    [SerializeField]
    public GroundState groundState;

    public bool usingKeyboard = false;

    public float carriedCharge = 0.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        Respawner.OnDeath += PlayAudio;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Respawner.OnDeath -= PlayAudio;
    }

    void Start()
    {
        // Create an object to check if player is grounded or touching wall
        groundState = new GroundState(transform.gameObject);
        //rb = GetComponent<Rigidbody>();
        ic = GetComponent<InputController>();
        ic.SetPlayer(id);
        spawnPos = transform.position;
        audioSources = audioPool.gameObject.GetComponents<AudioSource>();
    }

    private Vector2 input;

    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(new Vector2(115.0f, 15.0f), new Vector2(125.0f, 150.0f)));
    //    GUILayout.TextArea(
    //         "Grounded: " + groundState.isGround() + "\n" +
    //         "Touching: " + groundState.isTouching() + "\n" +
    //         "Wall: " + groundState.isWall() + "\n" +
    //         "Wall Direction: " + groundState.wallDirection() + "\n"
    //    );
    //    GUILayout.EndArea();
    //}

    protected override void Update()
    {
        if (!usingKeyboard)
        {
            if (ic.LeftHorizontal() != 0)
                input.x = ic.LeftHorizontal();
            if (ic.PressedA())
                input.y = 1;
        }
        else
        {
            if (Input.GetAxis("LeftHorizontal") != 0)
                input.x = Input.GetAxis("LeftHorizontal");
            if ( Input.GetButtonDown("Jump"))
            {
                input.y = 1;
                PlayAudio("mb_jump", "Effects");
            }
        }

        //if (ic.RightTrigger() > 0)
        //{
        //    if (energy - energy_cost >= 0)
        //    {
        //        StartCoroutine(AttributeTimer(4.0f));
        //        energy -= energy_cost;
        //    }
        //    else
        //    {
        //        Debug.Log("The player is exhausted, you cannot use them until you regain some energy");
        //    }
        //}
        //if (Input.GetKeyUp(KeyCode.LeftShift) || ic.RightTrigger() == 0)
        //if (ic.RightTrigger() == 0)
        //{
        //    StopCoroutine("AttributeTimer");
        //    mod_factor = 1.0f;
        //}

        //if (transform.position.y < -10.0f)
        //    Respawn();

        base.Update();

        // Reverse player if going different direction
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (input.x == 0) ? transform.localEulerAngles.y : (input.x + 1) * 90, transform.localEulerAngles.z);
    }

    IEnumerator AttributeTimer(float time)
    {
        mod_factor = shift_mod_factor;
        yield return new WaitForSeconds(time);
        mod_factor = 1.0f;
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(
            new Vector2(
                ((input.x * (speed * mod_factor)) - GetComponent<Rigidbody>().velocity.x) * (groundState.isGround() ? (accel * mod_factor) : (air_accel * mod_factor)),
                0)); //Move player.

        GetComponent<Rigidbody>().velocity
            = new Vector2(
                (input.x == 0 && groundState.isGround()) ? 0 : GetComponent<Rigidbody>().velocity.x,
                (input.y == 1 && groundState.isTouching()) ? (jump * mod_factor) : GetComponent<Rigidbody>().velocity.y); //Stop player if input.x is 0 (and grounded) and jump if input.y is 1

        if (groundState.isWall() && !groundState.isGround() && input.y == 1)
            GetComponent<Rigidbody>().velocity = new Vector2(
                -groundState.wallDirection() * (speed * mod_factor) * (wall_push * mod_factor),
                GetComponent<Rigidbody>().velocity.y); //Add force negative to wall direction (with speed reduction)

        input.y = 0;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Pickup")
        {
            col.GetComponent<Pickup>().Use(this);
        }

        if (col.transform.tag == "Battery")
        {
            Debug.Log("battery collision");
            col.GetComponent<Battery>().Use(this);
        }

        if (col.transform.tag == "CapturePoint")
        {
            // Make sure we reset the co-routines running on this script otherwise they'll overlap and fuck up the fill amount lerp
            col.gameObject.GetComponent<Objective>().Reset();
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.transform.tag == "CapturePoint")
        {
            Debug.Log("i'm inside you (the collider)");

            float chargeChange = capture_speed * Time.deltaTime;

            if (carriedCharge > 0)
            {
                col.gameObject.GetComponent<Objective>().active = true;
                carriedCharge = carriedCharge - chargeChange;
                col.gameObject.GetComponent<Objective>().AddCharge(chargeChange);
            }

            else if (carriedCharge < 0)
            {
                carriedCharge = 0;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "CapturePoint")
        {
            Debug.Log("i pulled out (of the collider)");
            col.gameObject.GetComponent<Objective>().active = false;
            // Make sure we reset the co-routines running on this script otherwise they'll overlap and fuck up the fill amount lerp
            col.gameObject.GetComponent<Objective>().Reset();
        }
    }

    public void Respawn(bool vibrate)
    {
#if UNITY_STANDALONE_WIN
        if(vibrate)
            ic.VibrateStart();
#endif
        if (em.clonesRemaining > 0)
        {
            transform.position = spawnPos;
            em.clonesRemaining--;
            em.ui.UpdateCloneNumber(em.clonesRemaining);
        }
        else
        {
            Destroy(gameObject, 1.0f);
        }
    }

    void OnDestroy()
    {
        em.CheckWinState();
    }
     
    #region Audio Controls

    public void PlayAudio(string track, string group)
    {
        Debug.Log("Playing track: " + track + " on channel: " + group);
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                AudioClip clip = Resources.Load("Sounds/" + track) as AudioClip;
                source.PlayOneShot(clip);
            }
        }
    }

    #endregion 

    public void AddCharge(float chargeToAdd)
    {
        carriedCharge = carriedCharge + chargeToAdd;
    }
}
