using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ObjectiveSystem : MonoBehaviour
{
    [SerializeField]
    public int damagePerCharge = 10;

    public Transform objectiveSpawnList;
    public Transform objectivePrefab;

    public List<GameObject> objectives;
    public int currentObjectiveCount = 0;

    private Vector3[] objectiveSpawns;

    private List<Objective> chargedObjs;
    private BossController boss;

    void Start()
    {
        //Debug.Log("TurretSystem started");

        SpawnObjectives();
    }

    void Awake()
    {
        //Debug.Log("TurretSystem awake");
        Init();
    }

    void Init()
    {
        //Debug.Log("TurretSystem initialising");

        if (objectiveSpawns == null) Debug.Log("Error: Please attach a container object to store the objective spawns in to this script.");
        if (objectivePrefab == null) Debug.Log("Error: Please attach an objective prefab object to this script.");

        int i = 0;
        objectiveSpawns = new Vector3[objectiveSpawnList.childCount];

        chargedObjs = new List<Objective>();

        for (i = 0; i < objectiveSpawnList.childCount; i++)
        {
            objectiveSpawns[i] = objectiveSpawnList.GetChild(i).transform.position;
        }
    }

    public void SpawnObjectives()
    {
        //Debug.Log("TurretSystem spawning");

        foreach (Vector3 spawnPoint in objectiveSpawns)
        {
            GameObject tempObj = Instantiate(objectivePrefab.gameObject, spawnPoint, Quaternion.Euler(new Vector3(0, 90, 0))) as GameObject;

            objectives.Add(tempObj);
            currentObjectiveCount++;
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
                obj.Fire(boss.transform.position);
            }

            chargedObjs.Clear();

            boss.Damage(damagePerCharge);
            
        }
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
