using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    private enum BossType
    {
        OCTOPUS
    }
    [SerializeField]
    private BossType bossType = BossType.OCTOPUS;

    public delegate void DamageAction(int pid, float health, float current_health, float amount);
    public static event DamageAction OnDamage;

    //General Variables
    InputController iC;
    public EntityManager em = null;
    Vector2 leftStick = Vector2.zero;
    Vector2 rightStick = Vector2.zero;
    bool leftStickActive = true;
    bool rightStickActive = true;
    public int playerIndex = 0;
    private Transform entities;
    Rigidbody leftArmRB;
    Rigidbody rightArmRB;
    public bool usingKeyboard = false;

    //Editable General Variables
    [SerializeField]
    Transform leftArmMovePoint;
    [SerializeField]
    Transform rightArmMovePoint;
    [SerializeField]
    private float armSpeed = 5f;
    [SerializeField]
    float attackCooldownLength = 2.0f;
    [SerializeField]
    float attackTime = 1.0f;
    public float baseHealth = 100.0f;
    public float currentHealth = 100.0f;

    //Raycast Specific Variables
    RaycastHit rayhit;

    //Octopus Specific Variables
    bool leftObjHeld = false;
    bool rightObjHeld = false;
    [SerializeField]
    Transform leftTenticleObjHolder;
    [SerializeField]
    Transform rightTenticleObjHolder;
    SlamController leftCollider;
    SlamController rightCollider;
    [SerializeField]
    List<Transform> leftTenticleObjects = new List<Transform>();
    [SerializeField]
    List<Transform> rightTenticleObjects = new List<Transform>();

    GameObject leftReticule = null;
    GameObject rightReticule = null;

    void Start()
    {
        iC = GetComponent<InputController>();
        iC.SetPlayer(playerIndex);
        leftCollider = leftArmMovePoint.GetComponent<SlamController>();
        rightCollider = rightArmMovePoint.GetComponent<SlamController>();
        leftArmRB = leftArmMovePoint.GetComponent<Rigidbody>();
        rightArmRB = rightArmMovePoint.GetComponent<Rigidbody>();
        entities = transform.parent.transform;
        switch (bossType)
        {
            case BossType.OCTOPUS:
                {
                    rightTenticleObjects = new List<Transform>();
                    leftTenticleObjects = new List<Transform>();
                    break;
                }
        }
    }

    void Update()
    {
        InputHandler();

        switch (bossType)
        {
            case BossType.OCTOPUS:
                {
                    //OctopusSpecificInput();
                    break;
                }
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            Damage(25);
        }

        if (GameManager.instance.state == GameManager.GameState.GAMEOVER && (iC.PressedStart() || Input.GetKeyDown(KeyCode.Return)))
        {
            SceneManager.LoadScene(0);
        }
    }

    void FixedUpdate()
    {
        #region Update tracking

        CalculateReticule(leftArmMovePoint.position, 0);
        CalculateReticule(rightArmMovePoint.position, 1);

        #endregion

        #region Movement Code

        if (leftStickActive)
            leftArmMovePoint.transform.Translate(new Vector3(leftStick.x * armSpeed * Time.deltaTime, leftStick.y * armSpeed * Time.deltaTime, 0f), Space.World);
        if (rightStickActive)
            rightArmMovePoint.transform.Translate(new Vector3(rightStick.x * armSpeed * Time.deltaTime, rightStick.y * armSpeed * Time.deltaTime, 0f), Space.World);

        // if (leftArmMovePoint.transform.position.y < 0)
        // {
        //     Vector3 tempPos = leftArmMovePoint.transform.position;
        //     tempPos.y = 0;
        //     leftArmMovePoint.transform.position = tempPos;
        // }
        // else if (leftArmMovePoint.transform.position.y > 32)
        // {
        //     Vector3 tempPos = leftArmMovePoint.transform.position;
        //     tempPos.y = 32;
        //     leftArmMovePoint.transform.position = tempPos;
        // }
        // if (rightArmMovePoint.transform.position.y < 0)
        // {
        //     Vector3 tempPos = rightArmMovePoint.transform.position;
        //     tempPos.y = 0;
        //     rightArmMovePoint.transform.position = tempPos;
        // }
        // else if (rightArmMovePoint.transform.position.y > 32)
        // {
        //     Vector3 tempPos = rightArmMovePoint.transform.position;
        //     tempPos.y = 32;
        //     rightArmMovePoint.transform.position = tempPos;
        // }

        #endregion

        #region Limitations

        // X Limitations
        // if (leftArmMovePoint.transform.position.x < 10)
        // {
        //     Vector3 tempPos = leftArmMovePoint.transform.position;
        //     tempPos.x = 10;
        //     leftArmMovePoint.transform.position = tempPos;
        // }
        // else if (leftArmMovePoint.transform.position.x > 52)
        // {
        //     Vector3 tempPos = leftArmMovePoint.transform.position;
        //     tempPos.x = 52;
        //     leftArmMovePoint.transform.position = tempPos;
        // }
        // if (rightArmMovePoint.transform.position.x < 12)
        // {
        //     Vector3 tempPos = rightArmMovePoint.transform.position;
        //     tempPos.x = 12;
        //     rightArmMovePoint.transform.position = tempPos;
        // }
        // else if (rightArmMovePoint.transform.position.x > 57)
        // {
        //     Vector3 tempPos = rightArmMovePoint.transform.position;
        //     tempPos.x = 57;
        //     rightArmMovePoint.transform.position = tempPos;
        // }

        #endregion

        //Set up for boss specific FixedUpdate
        switch (bossType)
        {
            case BossType.OCTOPUS:
                {
                    break;
                }
        }
    }

    public void Damage(int amount)
    {
        //if (baseHealth - amount <= 0.0f)
        //{
        //    Debug.Log("The boss has been slain.");
        //} 
        //else
        //{
        //    baseHealth -= amount;
        //    if (OnDamage != null)
        //    {
        //        OnDamage(3, baseHealth, currentHealth, amount);    // Always 3 because our boss is the 4th player
        //    }
        //}
<<<<<<< HEAD

        currentHealth -= amount;

        if (currentHealth <= 50)
        {
            ChangePlaces();
        }

=======
        currentHealth -= amount;
>>>>>>> ad8626c0fd88fb5133a7074d495423c33fcf42fe
        em.ui.UpdateBossHealth(currentHealth, baseHealth);
        em.CheckWinState();
    }

<<<<<<< HEAD
    // CHANGE PLACES!
    public void ChangePlaces()
    {
        GameManager.instance.cm.Play(1);
    }

=======
>>>>>>> ad8626c0fd88fb5133a7074d495423c33fcf42fe
    IEnumerator SlamAttack(bool attackWithLeft)
    {
        if (attackWithLeft)
        {
            leftStickActive = false;
            leftArmRB.isKinematic = false;
            leftArmRB.AddForce(new Vector3(0, -0.002f, 0));
            leftCollider.isActive = true;
        }
        else
        {
            rightStickActive = false;
            rightArmRB.isKinematic = false;
            rightArmRB.AddForce(new Vector3(0, -0.002f, 0));
            rightCollider.isActive = true;
        }

        // Wait for the attack to smash
        yield return new WaitForSeconds(attackTime);

        if (attackWithLeft)
            leftCollider.isActive = false;
        else
            rightCollider.isActive = false;

        // Give the player back control of the arm when the cooldown expires
        yield return new WaitForSeconds(attackCooldownLength - attackTime);

        if (attackWithLeft)
        {
            leftArmRB.isKinematic = true;
            leftStickActive = true;
        }
        else
        {
            rightArmRB.isKinematic = true;
            rightStickActive = true;
        }
    }

    void CalculateReticule(Vector3 armPosition, int tentacleid)
    {
        Physics.Raycast(new Ray(armPosition, Vector3.down), out rayhit);

        if (rayhit.collider != null)
        {
            if (rayhit.collider.tag == "Wall")
            {
                if (leftReticule == null || rightReticule == null)
                {
                    switch (tentacleid)
                    {
                        case 0:
                            leftReticule = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            leftReticule.transform.position = rayhit.point;
                            break;
                        case 1:
                            rightReticule = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            rightReticule.transform.position = rayhit.point;
                            break;
                        default:
                            Debug.Log("Created tentacle with id: " + tentacleid);
                            break;
                    }
                }
                else
                {
                    switch (tentacleid)
                    {
                        case 0:
                            leftReticule.transform.position = rayhit.point;
                            break;
                        case 1:
                            rightReticule.transform.position = rayhit.point;
                            break;
                        default:
                            Debug.Log("Tracking tentacle...");
                            break;
                    }
                }
            }
        }
    }

    void InputHandler()
    {
        if (leftStickActive)
        {
            if (!usingKeyboard)
            {
                leftStick = new Vector2(iC.LeftHorizontal(), iC.LeftVertical());

                if (iC.LeftTrigger() > 0)
                    StartCoroutine(SlamAttack(true));
            }
            else
            {
                leftStick = new Vector2(Input.GetAxis("LeftHorizontal"), Input.GetAxis("LeftVertical"));

                if (Input.GetButtonDown("LeftSlam"))
                    StartCoroutine(SlamAttack(true));
            }
        }
        if (rightStickActive)
        {
            if (!usingKeyboard)
            {
                rightStick = new Vector2(iC.RightHorizontal(), iC.RightVertical());

                if (iC.RightTrigger() > 0)
                    StartCoroutine(SlamAttack(false));
            }
            else
            {
                rightStick = new Vector2(Input.GetAxis("RightHorizontal"), Input.GetAxis("RightVertical"));

                if (Input.GetButtonDown("RightSlam"))
                    StartCoroutine(SlamAttack(false));
            }
        }
    }

    #region Octopus Methods
<<<<<<< HEAD

=======
>>>>>>> ad8626c0fd88fb5133a7074d495423c33fcf42fe
    void OctopusSpecificInput()
    {
        if (!usingKeyboard)
        {
            if (iC.PressedLeftShoulder() && leftStickActive)
            {
                if (leftObjHeld && leftTenticleObjHolder.childCount == 0)
                    leftObjHeld = false;
                if (!leftObjHeld)
                    PickupObject(true);
                else
                    DropObject(true);
            }
            if (iC.PressedRightShoulder() && rightStickActive)
            {
                if (rightObjHeld && rightTenticleObjHolder.childCount == 0)
                    rightObjHeld = false;
                if (!rightObjHeld)
                    PickupObject(false);
                else
                    DropObject(false);
            }
        }
        else
        {
            if (Input.GetButtonDown("LeftPickup") && leftStickActive)
            {
                if (leftObjHeld && leftTenticleObjHolder.childCount == 0)
                    leftObjHeld = false;
                if (!leftObjHeld)
                    PickupObject(true);
                else
                    DropObject(true);
            }
            if (Input.GetButtonDown("RightPickup") && rightStickActive)
            {
                if (rightObjHeld && rightTenticleObjHolder.childCount == 0)
                    rightObjHeld = false;
                if (!rightObjHeld)
                    PickupObject(false);
                else
                    DropObject(false);
            }
        }
    }

    public void AddInteractiveObject(Transform trans, bool leftTenticle)
    {
        if (leftTenticle)
        {
            leftTenticleObjects.Add(trans);
        }
        else
        {
            rightTenticleObjects.Add(trans);
        }
    }

    public void RemoveInteractiveObject(Transform trans, bool leftTenticle)
    {
        if (leftTenticle)
        {
            leftTenticleObjects.Remove(trans);
        }
        else
        {
            rightTenticleObjects.Remove(trans);
        }
    }

    void PickupObject(bool leftTenticle)
    {
        if (leftTenticle)
        {
            if (leftTenticleObjects.Count > 0)
            {
                leftTenticleObjects[0].SetParent(leftTenticleObjHolder);
                leftTenticleObjects[0].GetComponent<Rigidbody>().useGravity = false;
                leftTenticleObjects[0].GetComponent<Rigidbody>().isKinematic = true;
                RemoveInteractiveObject(leftTenticleObjects[0], true);
                leftObjHeld = true;
            }
        }
        else
        {
            if (rightTenticleObjects.Count > 0)
            {
                rightTenticleObjects[0].SetParent(rightTenticleObjHolder);
                rightTenticleObjects[0].GetComponent<Rigidbody>().useGravity = false;
                rightTenticleObjects[0].GetComponent<Rigidbody>().isKinematic = true;
                RemoveInteractiveObject(rightTenticleObjects[0], false);
                rightObjHeld = true;
            }
        }
    }

    void DropObject(bool leftTenticle)
    {
        if (leftTenticle)
        {
            if (leftTenticleObjHolder.childCount > 0)
            {
                leftTenticleObjHolder.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                leftTenticleObjHolder.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                RemoveInteractiveObject(leftTenticleObjects[0], true);
                leftTenticleObjHolder.GetChild(0).SetParent(entities);
                leftObjHeld = false;
            }
        }
        else
        {
            if (rightTenticleObjHolder.childCount > 0)
            {
                rightTenticleObjHolder.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                rightTenticleObjHolder.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                RemoveInteractiveObject(rightTenticleObjects[0], false);
                rightTenticleObjHolder.GetChild(0).SetParent(entities);
                rightObjHeld = false;
            }
        }
    }

    #endregion
}
