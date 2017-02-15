using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public enum MenuState
    {
        MAINMENU,
        PLAYERJOIN,
        LEVELSELECT
    }
    [SerializeField]
    public MenuState menuState = MenuState.MAINMENU;

    //public SceneManager sm = new SceneManager();

    public GameObject[] canvasHolder;
    public GameObject[] defaultButton;
    int currentCanvas = 0;

    public InputController[] ic;

    int[] playerState = new int[4];

    public GameObject[] player1JoinedState;
    public GameObject[] player2JoinedState;
    public GameObject[] player3JoinedState;
    public GameObject[] player4JoinedState;
    public GameObject pressStartImage;
    public bool[] joinedState = new bool[4];

    public GameObject[] currentlyTheBossImage;
    public GameObject[] currentlyAClone;
    public GameObject[] currentlyAClone2;

    public int currentNoOfPlayers = 0;

    public int currentlyTheBoss = 4;
    private int currentNumberOfClones = 5;
    bool keyboardJoined = false;
    public int currentKeyboardPlayer = 4;

    public Text cloneNoText;
    public Slider clonesSlider;

    bool waiting = false;

    public int minNoOfPlayers = 2;

    void Start()
    {
        SwitchCanvas(currentCanvas);
        for (int i = 0; i < 4; i++)
        {
            ic[i].SetPlayer(i);
        }
    }

    void Update()
    {
        if (!waiting)
            InputHandler();
    }

    void InputHandler()
    {
        switch (menuState)
        {
            case MenuState.MAINMENU:
                {
                    break;
                }

            case MenuState.PLAYERJOIN:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (ic[i].PressedA())
                        {
                            if (playerState[i] == 0)
                                ChangeJoinedState(i, 1);
                            else if (playerState[i] == 2)
                            {
                                if (playerState[0] == 0)
                                {
                                    ChangeJoinedState(0, 2);
                                    currentKeyboardPlayer = 0;
                                    keyboardJoined = true;
                                }
                                else if (playerState[1] == 0)
                                {
                                    ChangeJoinedState(1, 2);
                                    currentKeyboardPlayer = 1;
                                    keyboardJoined = true;
                                }
                                else if (playerState[2] == 0)
                                {
                                    ChangeJoinedState(2, 2);
                                    currentKeyboardPlayer = 2;
                                    keyboardJoined = true;
                                }
                                else if (playerState[3] == 0)
                                {
                                    ChangeJoinedState(3, 2);
                                    currentKeyboardPlayer = 3;
                                    keyboardJoined = true;
                                }

                                ChangeJoinedState(i, 1);
                            }
                        }
                        if (ic[i].PressedB())
                        {
                            if (playerState[i] == 1)
                            {
                                ChangeJoinedState(i, 0);
                            }
                            else
                            {
                                if (currentNoOfPlayers == 0)
                                    SwitchCanvas(0);
                            }
                        }

                        if (ic[i].PressedX())
                        {
                            if (currentlyTheBoss == i)
                                SwitchBoss(4);
                            else
                                SwitchBoss(i);
                        }

                        if (ic[i].PressedStart() && currentNoOfPlayers >= minNoOfPlayers)
                        {
                            SwitchCanvas(2);
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Return) && !waiting)
                    {
                        if (playerState[0] == 0 && !keyboardJoined)
                        {
                            ChangeJoinedState(0, 2);
                            currentKeyboardPlayer = 0;
                            keyboardJoined = true;
                        }  
                        else if (playerState[1] == 0 && !keyboardJoined)
                        {
                            ChangeJoinedState(1, 2);
                            currentKeyboardPlayer = 1;
                            keyboardJoined = true;
                        }
                        else if (playerState[2] == 0 && !keyboardJoined)
                        {
                            ChangeJoinedState(2, 2);
                            currentKeyboardPlayer = 2;
                            keyboardJoined = true;
                        }
                        else if (playerState[3] == 0 && !keyboardJoined)
                        {
                            ChangeJoinedState(3, 2);
                            currentKeyboardPlayer = 3;
                            keyboardJoined = true;
                        }  
                        else
                        {
                            if(currentNoOfPlayers >= minNoOfPlayers && keyboardJoined)
                            {
                                
                                SwitchCanvas(2);
                            }
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        if (playerState[0] == 2)
                        {
                            ChangeJoinedState(0, 0);
                            currentKeyboardPlayer = 4;
                            keyboardJoined = false;
                        }
                        else if (playerState[1] == 2)
                        {
                            ChangeJoinedState(1, 0);
                            currentKeyboardPlayer = 4;
                            keyboardJoined = false;
                        }
                        else if (playerState[2] == 2)
                        {
                            ChangeJoinedState(2, 0);
                            currentKeyboardPlayer = 4;
                            keyboardJoined = false;
                        }
                        else if (playerState[3] == 2)
                        {
                            ChangeJoinedState(3, 0);
                            currentKeyboardPlayer = 4;
                            keyboardJoined = false;
                        }
                        else
                        {
                            if (currentNoOfPlayers == 0)
                                SwitchCanvas(0);
                        }
                    }

                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        if (playerState[0] == 2)
                        {
                            if (currentlyTheBoss == 0)
                                SwitchBoss(4);
                            else
                                SwitchBoss(0);
                        }
                        else if (playerState[1] == 2)
                        {
                            if (currentlyTheBoss == 1)
                                SwitchBoss(4);
                            else
                                SwitchBoss(1);
                        }
                        else if (playerState[2] == 2)
                        {
                            if (currentlyTheBoss == 2)
                                SwitchBoss(4);
                            else
                                SwitchBoss(2);
                        }
                        else if (playerState[3] == 2)
                        {
                            if (currentlyTheBoss == 3)
                                SwitchBoss(4);
                            else
                                SwitchBoss(3);
                        }
                    }
                    break;
                }
        }
    }

    void ChangeJoinedState(int playerNo, int newState)
    {
        if (playerNo == currentlyTheBoss)
            SwitchBoss(4);
        if (playerNo == 0)
        {
            player1JoinedState[playerState[playerNo]].SetActive(false);
            playerState[playerNo] = newState;
            player1JoinedState[playerState[playerNo]].SetActive(true);
        }
        else if (playerNo == 1)
        {
            player2JoinedState[playerState[playerNo]].SetActive(false);
            playerState[playerNo] = newState;
            player2JoinedState[playerState[playerNo]].SetActive(true);
        }
        else if (playerNo == 2)
        {
            player3JoinedState[playerState[playerNo]].SetActive(false);
            playerState[playerNo] = newState;
            player3JoinedState[playerState[playerNo]].SetActive(true);
        }
        else if (playerNo == 3)
        {
            player4JoinedState[playerState[playerNo]].SetActive(false);
            playerState[playerNo] = newState;
            player4JoinedState[playerState[playerNo]].SetActive(true);
        }
        int playersJoined = 0;
        for (int i = 0; i < 4; i++)
        {
            if (playerState[i] != 0)
                playersJoined++;
        }
        currentNoOfPlayers = playersJoined;
        if (currentNoOfPlayers >= minNoOfPlayers)
            pressStartImage.SetActive(true);
        else
            pressStartImage.SetActive(false);

        if(newState == 0)
        {
            joinedState[playerNo] = false;
        }
        else
        {
            joinedState[playerNo] = true;
        }
    }

    public void SwitchCanvas(int canvasToSwitchTo)
    {
        if (!waiting)
        {
            StartCoroutine(Wait(0.1f));
            canvasHolder[currentCanvas].SetActive(false);
            currentCanvas = canvasToSwitchTo;
            menuState = (MenuState)currentCanvas;
            canvasHolder[currentCanvas].SetActive(true);
            if (defaultButton[currentCanvas] != null)
            {
                EventSystem.current.SetSelectedGameObject(canvasHolder[currentCanvas]);
                EventSystem.current.SetSelectedGameObject(defaultButton[currentCanvas]);
            }
            NumberOfClonesSet();
            SetPlayerPrefs();
        }
    }

    void SwitchBoss(int newBoss)
    {
        if(currentlyTheBoss < 4)
        {
            currentlyTheBossImage[currentlyTheBoss].SetActive(false);
            currentlyAClone[currentlyTheBoss].SetActive(true);
            currentlyAClone2[currentlyTheBoss].SetActive(true);
        }   
        currentlyTheBoss = newBoss;
        if (currentlyTheBoss < 4)
        {
            currentlyTheBossImage[currentlyTheBoss].SetActive(true);
            currentlyAClone[currentlyTheBoss].SetActive(false);
            currentlyAClone2[currentlyTheBoss].SetActive(false);
        }
            
    }

    IEnumerator Wait(float timeToWait)
    {
        waiting = true;
        //Debug.Log("Waiting! " + timeToWait);
        yield return new WaitForSeconds(timeToWait);
        waiting = false;
    }

    void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt("BossIndex", currentlyTheBoss);
        PlayerPrefs.SetInt("InitialClones", currentNumberOfClones);
        PlayerPrefs.SetInt("KeyboardIndex", currentKeyboardPlayer);
        //PlayerPrefs.SetInt("NoOfPlayers", currentNoOfPlayers);
        PlayerPrefs.SetInt("NoOfPlayers", 4);
        for (int i = 0; i < 4; i++)
        {
            if(joinedState[i])
                PlayerPrefs.SetInt(("Player" + (i + 1) + "Joined"), 1);
            else
                PlayerPrefs.SetInt(("Player" + (i + 1) + "Joined"), 0);
        }
    }

    public void NumberOfClonesSet()
    {
        currentNumberOfClones = (int)clonesSlider.value;
        cloneNoText.text = "Number of clones: " + currentNumberOfClones;
    }

    public void LoadLevel(int levelNumber)
    {
        SetPlayerPrefs();
        //sm.LoadLevel("Level_" + levelNumber);
        SceneManager.LoadScene("Level_" + levelNumber);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
