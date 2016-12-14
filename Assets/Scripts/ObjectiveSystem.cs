using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ObjectiveSystem : MonoBehaviour
{
    [SerializeField]
    public int damagePerCharge = 10;
    public float secsBetweenBatterySpawns = 5.0f;
    public int maxBatteries = 2;

    private float timeSinceLastBattery = 0.0f;

    private int lastBatterySpawn = 0;

    public Transform objectiveSpawnList;
    public Transform objectivePrefab;

    public Transform batterySpawnList;
    public Transform batteryPrefab;

    public List<GameObject> objectives;
    public int currentObjectiveCount = 0;

    private Transform[] objectiveSpawns;
    private Transform[] batterySpawns;

    private List<Objective> chargedObjs;
    private BossController boss;

    void Start()
    {
        //Debug.Log("TurretSystem started");

        SpawnObjectives();
        SpawnBatteries();
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

        chargedObjs = new List<Objective>();

        for (i = 0; i < objectiveSpawnList.childCount; i++)
        {
            objectiveSpawns[i] = objectiveSpawnList.GetChild(i).transform;
        }
        for (i = 0; i < batterySpawnList.childCount; i++)
        {
            batterySpawns[i] = batterySpawnList.GetChild(i).transform;
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

        //foreach (Vector3 spawnPoint in batterySpawns)
        //{
        //    GameObject tempObj = Instantiate(batteryPrefab.gameObject, spawnPoint, Quaternion.Euler(new Vector3(0, 90, 0))) as GameObject;

        //    tempObj.transform.parent = GameObject.Find("BatteryHolder").transform;
        //}
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
                obj.Fire(boss.transform.position);
            }

            chargedObjs.Clear();

            boss.Damage(damagePerCharge);
            
        }

        timeSinceLastBattery = timeSinceLastBattery + Time.deltaTime;

        SpawnBatteries();
    }

    public void RegisterChargedObj(Objective chargedObj)
    {
        if (!chargedObjs.Contains(chargedObj))
        {
            chargedObjs.Add(chargedObj);
        }
    }

    public void RegisterDechargedObj(Objective dechargedObj)
    {
        chargedObjs.Remove(dechargedObj);
    }
}
