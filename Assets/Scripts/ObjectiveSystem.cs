using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ObjectiveSystem : MonoBehaviour
{
    [SerializeField]
    public int damagePerCharge = 25;
    public float secsBetweenBatterySpawns = 5.0f;
    public int maxBatteries = 2;

    private float timeSinceLastBattery = 0.0f;

    private int lastBatterySpawn = 0;

    public Transform objectiveSpawnList;
    public Transform objectivePrefab;

    public Transform batterySpawnList;
    public Transform batteryPrefab;
    private Transform[] batterySpawns;

    public Transform fallingBatterySpawnList;
    public Transform fallingBatteryPrefab;
    private Transform[] fallingBatterySpawns;

    public List<GameObject> objectives;
    public int currentObjectiveCount = 0;

    private Transform[] objectiveSpawns;

    private List<Objective> chargedObjs;
    private BossController boss;
    public Transform firelocation;

    public bool fallingBatteries = true;

    public bool spawnEnemiesOnCharge = true;

    public delegate void RocketAction(Vector3 target, float delay);
    public static event RocketAction OnFire;

    void Start()
    {
        //Debug.Log("TurretSystem started");

        SpawnObjectives();

        if (fallingBatteries)
        {
            SpawnFallingBatteries();
        }
        else
        {
            SpawnBatteries();
        }
    }

    void Awake()
    {
        //Debug.Log("TurretSystem awake");
        Init();
    }

    void Init()
    {
        //Debug.Log("TurretSystem initialising");

        //if (objectiveSpawns == null) Debug.Log("Error: Please attach a container object to store the objective spawns in to this script.");
        //if (objectivePrefab == null) Debug.Log("Error: Please attach an objective prefab object to this script.");

        int i = 0;
        objectiveSpawns = new Transform[objectiveSpawnList.childCount];
        batterySpawns = new Transform[batterySpawnList.childCount];
        fallingBatterySpawns = new Transform[fallingBatterySpawnList.childCount];


        chargedObjs = new List<Objective>();

        for (i = 0; i < objectiveSpawnList.childCount; i++)
        {
            objectiveSpawns[i] = objectiveSpawnList.GetChild(i).transform;
        }
        for (i = 0; i < batterySpawnList.childCount; i++)
        {
            batterySpawns[i] = batterySpawnList.GetChild(i).transform;
        }
        for (i = 0; i < fallingBatterySpawnList.childCount; i++)
        {
            fallingBatterySpawns[i] = fallingBatterySpawnList.GetChild(i).transform;
        }
    }

    public void SpawnObjectives()
    {
        //Debug.Log("TurretSystem spawning");

        for (int i = 0; i < objectiveSpawns.Length; i++)
        {

            // foreach (Vector3 spawnPoint in objectiveSpawns)
            // {
            GameObject tempObj = Instantiate(objectivePrefab.gameObject, objectiveSpawns[i].position, objectiveSpawns[i].rotation) as GameObject;

            objectives.Add(tempObj);

            tempObj.transform.parent = GameObject.Find("ObjectiveHolder").transform;

            currentObjectiveCount++;
        }
    }

    public void SpawnBatteries()
    {
        GameObject[] batteries = GameObject.FindGameObjectsWithTag("Battery");

        if (batteries.Length < maxBatteries)
        {
            if (timeSinceLastBattery >= secsBetweenBatterySpawns)
            {
                int rand = UnityEngine.Random.Range(0, batterySpawns.Length);

                while (rand == lastBatterySpawn)
                {
                    rand = UnityEngine.Random.Range(0, batterySpawns.Length);
                }

                lastBatterySpawn = rand;

                GameObject tempObj = Instantiate(batteryPrefab.gameObject, batterySpawns[rand].position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;

                tempObj.transform.parent = GameObject.Find("BatteryHolder").transform;

                timeSinceLastBattery = 0.0f;
            }
        }

    }


    private void SpawnFallingBatteries()
    {
        GameObject[] batteries = GameObject.FindGameObjectsWithTag("Battery");

        if (batteries.Length < maxBatteries)
        {
            if (timeSinceLastBattery >= secsBetweenBatterySpawns)
            {
                int rand = UnityEngine.Random.Range(0, fallingBatterySpawns.Length);

                while (rand == lastBatterySpawn)
                {
                    rand = UnityEngine.Random.Range(0, fallingBatterySpawns.Length);
                }

                lastBatterySpawn = rand;

                GameObject tempObj = Instantiate(fallingBatteryPrefab.gameObject, fallingBatterySpawns[rand].position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;

                tempObj.transform.parent = GameObject.Find("BatteryHolder").transform;

                tempObj.transform.Rotate(new Vector3(UnityEngine.Random.Range(0.0f, 90.0f), UnityEngine.Random.Range(0.0f, 90.0f), UnityEngine.Random.Range(0.0f, 90.0f)));
                tempObj.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-500.0f, 500.0f), 0.0f, 0.0f));

                timeSinceLastBattery = 0.0f;
            }
        }
    }

    public void Update()
    {
        //Debug.Log("Number of charged turrets = " + chargedObjs.Count);

        if (chargedObjs.Count == objectives.Count)
        {
            //Debug.Log("All turrets charged");

            /* FIRE AT BOSS */

            boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();

            foreach (Objective obj in chargedObjs)
            {
                obj.Fire(boss.transform.FindChild("Pupil").position);
            }

            chargedObjs.Clear();

            // Fire rockets
            FireRockets(boss.transform.FindChild("Pupil").position, 3.0f);

            Invoke("DamageBoss", 2.0f);
            // boss.Damage(damagePerCharge);
        }

        timeSinceLastBattery = timeSinceLastBattery + Time.deltaTime;

        if (fallingBatteries)
        {
            SpawnFallingBatteries();
        }
        else
        {
            SpawnBatteries();
        }
    }

    // Emit the fire rockets event to be picked up by the TargetRocket script
    void FireRockets(Vector3 target, float travelTime)
    {
        if (OnFire != null)
            OnFire(target, travelTime);
    }

    // private void OnGUI()
    // {
    //     if (GUI.Button(new Rect(150, 50, 
    //         200, 25), "LAUNCH THE TORPEDOS"))
    //         FireRockets(new Vector3(0.0f, 0.0f, 0.0f), 3.0f);
    // 
    // }

    void DamageBoss()
    {
        boss.Damage(damagePerCharge);
    }

    public void RegisterChargedObj(Objective chargedObj)
    {
        if (!chargedObjs.Contains(chargedObj))
        {
            chargedObjs.Add(chargedObj);
        }

        GameObject.Find("_GM").GetComponent<EntityManager>().SpawnPhaseSquids();
    }

    public void RegisterDechargedObj(Objective dechargedObj)
    {
        chargedObjs.Remove(dechargedObj);
    }
}
