using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;      // Singleton to help access global vars from other scripts
    public EntityManager em;                        // Drag the entity manager from the same object here
    public AudioManager am;                         // Drag the audio manager from the same object here
    public CutsceneManager cm;                         // Drag the cutscene manager from the same object here
    public int playerCount = 1;                    // How many players are gonna be playing this game?

    // State flags help pin down what went wrong in what stage for easier debugging
    public enum GameState
    {
        DEFAULT,        // Default state 
        INITIALISING,   // Getting the game ready
        READY,          // Play state, start running logic here
        PAUSED,         // Set this flag is we're paused, stop logic
        LOADING,        // If the level needs loading before the players can move, set this
        SAVING,         // If the level is being saved/serialized, handle this
        PROCESSING,     // Any post processing done after loading, trigger this flag
        GAMEOVER
    };
    public GameState state = GameState.INITIALISING;

    // Called when this object is instantiated in the scene
    void Awake()
    {
        #region Singleton Check

        bool reinit = false;
        if (instance != null) {
            reinit = true;
        }

        if (instance == null)
        {
            instance = this;
        } 
        else if (instance != this)
        {
            Destroy(gameObject); // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
        }

        #endregion

        // If an instance already exists, the start function won't be called, lets call it manually
        if (reinit)
        {
            Debug.Log("The game manager has restarted the game");
            Init();
        }
        Time.timeScale = 1.0f;
        // Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        //Debug.Log("The game manager has started the game");
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == GameState.GAMEOVER)
        {
            //Time.timeScale = 0.0f;
        }
    }

    void Init()
    {
        // We're initialising the game scene
        SetState(GameState.INITIALISING);

        // Init the scene manager first
        em.Init();

        // Spawn players
        em.SpawnPlayers(PlayerPrefs.GetInt("NoOfPlayers"));

        // Play the intro cutscene
        cm.Play(0);

        // We're good to go
        SetState(GameState.READY);

    }

    public GameState GetState()
    {
        return state;
    }

    public void SetState(GameState _state)
    {
        state = _state;
    }
}
