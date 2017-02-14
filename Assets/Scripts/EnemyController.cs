using UnityEngine;
using System.Collections;
using System;

public class EnemyController : MonoBehaviour
{

    public bool lockZposition = true;
    public bool lockZrotation = true;
    public bool lockXrotation = true;

    private EntityManager em;
    private GameObject nearestPlayer;
    private Rigidbody rb;

    public float timeBetweenPlayerChecks = 5.0f;
    private float playerCheckTimer = 0.0f;

    private float dirChangeTimer = 0.0f;
    private float nextDirChangeTime = 2.0f;
    public float minDirChangeTime = 2.0f;
    public float maxDirChangeTime = 5.0f;
    private bool moveRight = true;
    private bool playerInRange = false;
    public float playerSearchRadius = 50.0f;

    private float jumpTimer = 0.0f;
    private float nextJumpTime = 2.0f;
    public float minJumpTime = 2.0f;
    public float maxJumpTime = 5.0f;

    public float moveForce = 20.0f;
    public float jumpForce = 150.0f;
    public float speedTarget = 10.0f;
    private Vector3 currentVector;

    private bool fallHasBegun = false;
    private bool hasLanded = false;
    public float attackRange;

    // Use this for initialization
    void Start()
    {
        em = UnityEngine.Object.FindObjectOfType<EntityManager>().GetComponent<EntityManager>();
        rb = gameObject.GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        currentVector = new Vector3();

        moveRight = UnityEngine.Random.value > 0.5f;

        playerCheckTimer = timeBetweenPlayerChecks;
    }

    // Update is called once per frame
    void Update()
    {
        currentVector = new Vector3();

        if (playerCheckTimer >= timeBetweenPlayerChecks)
        {
            FindNearestPlayer();
            playerCheckTimer = 0.0f;
        }

        if (playerInRange && nearestPlayer != null)
        {
            MoveToPlayer();
            AttemptAttack();
        }
        else
        {
            RandomMove();
        }

        if (rb.velocity.x >= speedTarget)
        {
            currentVector.x = 0;
            rb.velocity.Set(speedTarget, rb.velocity.y, 0.0f);
        }
        if (rb.velocity.x <= -speedTarget)
        {
            currentVector.x = 0;
            rb.velocity.Set(-speedTarget, rb.velocity.y, 0.0f);
        }

        playerCheckTimer = playerCheckTimer + Time.deltaTime;
        dirChangeTimer = dirChangeTimer + Time.deltaTime;
        jumpTimer = jumpTimer + Time.deltaTime;
    }

    void AttemptAttack()
    {
        Vector3 distance = nearestPlayer.transform.position - this.transform.position;
        if (Mathf.Abs(distance.magnitude) < attackRange)
        {
            Destroy(nearestPlayer);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(currentVector);

    }

    private void FindNearestPlayer()
    {
        //Only consider players closer than search radius
        float lastSmallestDist = playerSearchRadius;
        bool playerFound = false;

        foreach (Transform player in em.entityContainer.GetComponentInChildren<Transform>())
        {

            Vector3 diff = player.transform.position - transform.position;
            float dist = diff.magnitude;

            if (dist < lastSmallestDist)
            {
                lastSmallestDist = dist;
                nearestPlayer = player.gameObject;
                playerFound = true;
            }

        }

        if (playerFound)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }

    public void CheckIfLanded()
    {
        if (rb.velocity.y < -0.05f)
        {
            fallHasBegun = true;
        }

        if (fallHasBegun && rb.velocity.y > -0.1f)
        {
            hasLanded = true;
        }
    }

    private void MoveToPlayer()
    {
        Vector3 playerPos = nearestPlayer.transform.position;

        if (playerPos.x <= transform.position.x)
        {
            currentVector.x = -moveForce;
        }
        else if (playerPos.x >= transform.position.x)
        {
            currentVector.x = moveForce;
        }

        if (playerPos.y > transform.position.y)
        {
            if (jumpTimer >= nextJumpTime)
            {
                currentVector.y = jumpForce;
                jumpTimer = 0.0f;
                nextJumpTime = UnityEngine.Random.Range(minJumpTime, maxJumpTime);
            }
        }
    }

    private void RandomMove()
    {
        if (dirChangeTimer >= nextDirChangeTime)
        {
            moveRight = !moveRight;
            dirChangeTimer = 0.0f;
            nextDirChangeTime = UnityEngine.Random.Range(minDirChangeTime, maxDirChangeTime);
        }

        if (!hasLanded)
        {
            currentVector = new Vector3(0.0f, 0.0f, 0.0f);
            CheckIfLanded();
        }
        else if (moveRight)
        {
            currentVector.x = moveForce;
        }
        else
        {
            currentVector.x = -moveForce;
        }

        if (jumpTimer >= nextJumpTime)
        {
            currentVector.y = jumpForce;
            jumpTimer = 0.0f;
            nextJumpTime = UnityEngine.Random.Range(minJumpTime, maxJumpTime);
        }
    }

    public void Despawn()
    {
        em.enemies.Remove(gameObject);
        Destroy(gameObject);
    }
}