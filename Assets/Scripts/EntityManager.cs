using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour
{
    public UIController ui;
    public Transform bossContainer;
    public Transform entityContainer;
    public Transform inactiveEntityContainer;
    public Transform spawnPointsList;
    public Transform bossSpawnPoint;
    public Transform enemyContainer;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject[] bossPrefabs;
    private int bossIndex = 0;

    public Transform enemyPrefab;
    public float aiSpawnMinX = -6.0f;
    public float aiSpawnMaxX = 74.0f;
    public float enemyStartHeight = 50.0f;
    public int numberOfEnemies = 3;
    public List<GameObject> enemies;

    bool[] joinedState = new bool[4];

    public int currentPlayerCount = 0;    // 0 indexed

    public int clonesRemaining = 0;

    int playerUsingKeyboard = 4;

    public List<GameObject> players;
    public GameObject[] pressStartText;
    private Transform[] spawnPoints;

    public bool spawnSquidlings = false;
    public Transform squidSpawns;
    public int squidlingsPerPhase = 5;
    //bool waiting = false;

    // Called when this object is instantiated in the scene
    void Awake()
    {
        Init();
    }

    void LateUpdate()
    {
        // This will run in the game manager logic loop, check here for deaths/game states/things not tied to the players/physics step
        CheckWinState();
    }

    public void Init()
    {
        if (entityContainer == null) Debug.Log("Error: Please attach a container object to store the entities in to this script.");
        if (spawnPointsList == null) Debug.Log("Error: Please attach a spawn points object to this script.");
        if (playerPrefab == null) Debug.Log("Error: Please attach a player prefab object to this script.");

        int i = 0;
        spawnPoints = new Transform[spawnPointsList.childCount];
        for (i = 0; i < spawnPointsList.childCount; i++)
        {
            spawnPoints[i] = spawnPointsList.GetChild(i).transform;
        }

        clonesRemaining = PlayerPrefs.GetInt("InitialClones");
        bossIndex = PlayerPrefs.GetInt("BossIndex");
        playerUsingKeyboard = PlayerPrefs.GetInt("KeyboardIndex");

        for (int j = 0; j < 4; j++)
        {
            if (PlayerPrefs.GetInt(("Player" + (j + 1) + "Joined")) == 0)
                joinedState[j] = false;
            else
                joinedState[j] = true;
        }

        //if (bossIndex == 4)
        //    bossIndex = 3;
    }

    Vector3 RandomSpawnPoint()
    {
        return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position;
    }

    public void SpawnPlayers(int player_count)
    {
        if (bossIndex == 4)
        {
            bossIndex = UnityEngine.Random.Range(0, player_count);
            while (!joinedState[bossIndex])
            {
                bossIndex = UnityEngine.Random.Range(0, player_count);
            }
        }
        int playersSpawned = 0;
        for (int i = 0; i < player_count; i++)
        {
            if (i == bossIndex)
            {
                GameObject temp_player = Instantiate(bossPrefabs[0], bossSpawnPoint.position, bossSpawnPoint.rotation) as GameObject;
                temp_player.transform.parent = bossContainer.transform;
                temp_player.GetComponent<BossController>().em = this;

                if (temp_player.GetComponent<BossController>() != null)
                    temp_player.GetComponent<BossController>().playerIndex = bossIndex;
                if (playerUsingKeyboard == i)
                {
                    temp_player.GetComponent<BossController>().usingKeyboard = true;
                }
                else
                {
                    temp_player.GetComponent<BossController>().usingKeyboard = false;
                }
                players.Add(temp_player);
            }
            else
            {
                GameObject temp_player = Instantiate(playerPrefab, spawnPoints[playersSpawned].position, spawnPoints[playersSpawned].rotation) as GameObject;
                temp_player.transform.parent = entityContainer.transform;
                temp_player.GetComponent<PlayerController>().em = this;

                // Handles all scripts that inherit from entity
                if (temp_player.GetComponent<Entity>() != null)
                    temp_player.GetComponent<Entity>().id = currentPlayerCount;
                if (playerUsingKeyboard == i)
                {
                    temp_player.GetComponent<PlayerController>().usingKeyboard = true;
                }
                else
                {
                    temp_player.GetComponent<PlayerController>().usingKeyboard = false;
                }
                temp_player.GetComponent<PlayerController>().cloneNumber = playersSpawned;
                players.Add(temp_player);
                playersSpawned++;
                if (!joinedState[i])
                {
                    temp_player.GetComponent<PlayerController>().SetPlayerInactive(entityContainer.transform);
                    temp_player.transform.parent = inactiveEntityContainer.transform;
                }
            }
            currentPlayerCount++;
        }
    }

    public void SpawnEnemy(int noToSpawn)
    {
        spawnSquidlings = false;
        for (int i = noToSpawn; i > 0; i--)
        {
            //Randomise X
            float randX = UnityEngine.Random.Range(aiSpawnMinX, aiSpawnMaxX);

            GameObject tempObj = Instantiate(enemyPrefab.gameObject, new Vector3(randX, enemyStartHeight, 6.0f), Quaternion.identity) as GameObject;

            tempObj.transform.parent = enemyContainer.transform;
            enemies.Add(tempObj);
        }
    }

    public void SpawnPhaseSquids(int spawnCount)
    {
        List<Transform> spawns = new List<Transform>(squidSpawns.GetAllChildren());        
        for (int i = 0; i < spawnCount; i++)
        {
            BezierCurve squidTravelCurve = spawns[UnityEngine.Random.Range(0, spawns.Count)].GetComponent<BezierCurve>();
            // Debug.Log(squidTravelCurve.GetPointAt(0));
            GameObject squid = Instantiate(
                enemyPrefab.gameObject,
                squidTravelCurve.GetAnchorPoints()[0].position,
                Quaternion.identity)
            as GameObject;
            squid.transform.parent = enemyContainer.transform;
            enemies.Add(squid);
            StartCoroutine(MoveSquid(squid, 2.0f, squidTravelCurve));
        }

    }

    IEnumerator MoveSquid(GameObject g, float travelTime, BezierCurve curve)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < travelTime)
        {
            float reciprocalTime = elapsedTime / travelTime;
            g.transform.position =
                curve.GetPointAt(reciprocalTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void CheckWinState()
    {
        if (ui.bossHealthImage.fillAmount == 0)
        {
            ui.winnerText.text = "Clones Win!";
            ui.SwitchCanvas(1);
            GameManager.instance.SetState(GameManager.GameState.GAMEOVER);
        }

        if (entityContainer.childCount == 0)
        {
            ui.winnerText.text = "Boss Wins!";
            ui.SwitchCanvas(1);
            GameManager.instance.SetState(GameManager.GameState.GAMEOVER);
        }
    } 

    public void Update()
    {  

    }
}
