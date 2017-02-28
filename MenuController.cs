using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using SmartLocalization;


public class MenuController : MonoBehaviour
{
    #region Menu Sections
    enum MenuState { TITLESCREEN, MAINMENU, PLAYERJOIN, LEVELSELECT, LOADLEVEL, OPTIONS, AUDIO, GRAPHICS, CONTROLS, CREDITS }; //, LMSSCREEN, TWOVSTWOSCREEN };
    MenuState currentMenuState = MenuState.TITLESCREEN;
    #endregion

    public InputController[] inputController;

    #region Player Select Variables

    public GameObject playerSelectCameras;
    bool[] playerJoined = new bool[4];
    public GameObject[] playerSelectBackgrounds;
    public GameObject[] blackoutScreen;
    public GameObject[] whiteoutScreen169;
    public GameObject[] whiteoutScreen1610;
    public GameObject[] whiteoutScreen43;

    public GameObject[] aToJoinImages;
    #region Stats Objects and Holder
    public GameObject[] p1SpeedFill;
    public GameObject[] p1WeightFill;
    public GameObject[] p1HandlingFill;
    public GameObject[] p2SpeedFill;
    public GameObject[] p2WeightFill;
    public GameObject[] p2HandlingFill;
    public GameObject[] p3SpeedFill;
    public GameObject[] p3WeightFill;
    public GameObject[] p3HandlingFill;
    public GameObject[] p4SpeedFill;
    public GameObject[] p4WeightFill;
    public GameObject[] p4HandlingFill;
    public GameObject[] leftArrows;
    public GameObject[] rightArrows;
    public GameObject leftLevelSelectArrows;
    public GameObject rightLevelSelectArrows;
    public GameObject leftGameModeArrow;
    public GameObject rightGameModeArrow;
    public GameObject[] leftShoulders;
    public GameObject[] rightShoulders;
    public GameObject[] analogueSticks;
    public GameObject[] keyboardIcons;
    //0 = LB, 1 = RB, 2 = Q, 3 = E
    public Sprite[] buttonSprites;

    //StatsFillHolder[Hovercraft, (Speed, Weight, Handling), ValueIcons]
    public GameObject[,,] StatsFillHolder = new GameObject[4, 3, 5];
    public bool[] lockedIn = new bool[4];
    bool[] playerWait = new bool[4];
    #endregion
    public GameObject[] hovercraftTypeName;
    private int[,] hovercraftStats = new int[4, 3];
    public int currentLevelChoice = 0;
    public GameObject levelChoiceText;
    public int currentGameModeChoice = 0;
    public int currentNoOfLives = 5;
    public GameObject gameModeChoiceText;
    public int numberOfLevels;
    public int numberOfGameModes;
    public string[] gameModeStringKeys;
    public int keyboardInSlot = 4;
    public GameObject[] audioSources;
    public AudioClip[] audioClips;
    #endregion

    #region Canvas Holders
    //public GameObject[] resolutionParent;
    public GameObject titleScreen;
    public GameObject mainMenuScreen;
    public GameObject optionsScreen;
    public GameObject creditsScreen;
    public GameObject audioScreen;
    public GameObject graphicsScreen;
    public GameObject controlsScreen;
    public GameObject playerJoinScreen;
    public GameObject levelSelectScreen;
    public GameObject loadingScreen;
    [SerializeField]
    private GameObject[] canvasHolder;
    [SerializeField]
    private GameObject[] defaultButtons;
    #endregion

    #region Game Mode Screens
    public GameObject lMSScreen;
    public GameObject lMSDefaultButton;
    public GameObject lMSLeftArrow;
    public GameObject lMSRightArrow;
    public Text noOfLivesText;
    public GameObject levelSelectButtons;
    public GameObject levelSelectInfoBar;
    public GameObject lMSInfoBar;
    #endregion

    #region Button Variables
    public GameObject mainMenuPlay;
    public GameObject mainMenuSettings;
    public GameObject mainMenuQuit;
    public GameObject levelSelectLevel;
    public GameObject levelSelectGameMode;
    public GameObject levelSelectPlay;
    public GameObject optionsDefaultButton;
    public GameObject audioDefaultButton;
    public GameObject graphicsDefaultButton;
    #endregion

    #region Hovercrafts and Choice Variables
    [Space(10, order = 0)]
    [Header("Hovercrafts and Choice Vars", order = 1)]
    [Space(10, order = 2)]

    public GameObject[] p1Hovercrafts;
    public GameObject[] p2Hovercrafts;
    public GameObject[] p3Hovercrafts;
    public GameObject[] p4Hovercrafts;
    GameObject[,] hovercrafts = new GameObject[4, 4];
    int[] selectedHovercraft = new int[4];
    int noOfHovercrafts = 0;
    bool[] axisHeld = new bool[4];

    int[] playerColour = new int[4];
    int noOfColours = 9;
    public Material[] p1Shaders;
    public Material[] p2Shaders;
    public Material[] p3Shaders;
    public Material[] p4Shaders;
    Material[,] shaders = new Material[4, 4];
    int noOfShaders;
    Color[] colours = new Color[9];
    int currentScreenRatio = 0;

    #endregion

    //public GameObject[] levelVideos;
    public LevelPreviewController levelPreviewController;

    public GameObject joinText;
    public GameObject flashyAButton;

    private int currentButtonSelected = 0;
    public float buttonAnimationTime = 5.0f;
    public GameObject keyboardControlIcons;
    public GameObject controllerControlIcons;
    public GameObject keyboardControlsHolder;
    public GameObject controllerControlsHolder;
    public RuntimeAnimatorController buttonAnimationController;
    bool allReady = false;
    bool[] aiColoursChecked = new bool[4];

    public bool waiting = false;

    public Image loadingBar;

    void Awake()
    {
        levelPreviewController = this.gameObject.GetComponent<LevelPreviewController>();
        GetCurrentRatio();
        for (int i = 0; i < 4; i++)
        {
            playerColour[i] = i;
        }

        #region 2D Hovercraft Array Setup

        for (int i = 0; i < 4; i++)
        {
            hovercrafts[0, i] = p1Hovercrafts[i];
            hovercrafts[1, i] = p2Hovercrafts[i];
            hovercrafts[2, i] = p3Hovercrafts[i];
            hovercrafts[3, i] = p4Hovercrafts[i];
        }
        #endregion

        #region 2D Shader Array Setup

        for (int i = 0; i < 4; i++)
        {
            shaders[0, i] = p1Shaders[i];
            shaders[1, i] = p2Shaders[i];
            shaders[2, i] = p3Shaders[i];
            shaders[3, i] = p4Shaders[i];
        }
        #endregion

        for (int i = 0; i < 4; i++)
        {
            inputController[i].SetPlayer(i);
        }
        this.GetComponent<OptionsController>().SetInputControllers(inputController);
        noOfHovercrafts = p1Hovercrafts.Length - 1;
        noOfShaders = p1Shaders.Length;
        SetColours();
        SetHovercraftStats();
        PopulateStatFillHolder();
    }

    void Start()
    {
        for (int i = 0; i < canvasHolder.Length; i++)
        {
            canvasHolder[i].SetActive(false);
        }
        currentMenuState = MenuState.TITLESCREEN;
        SwitchCanvas(0);
        Time.timeScale = 1.0f;
        StartCoroutine(Wait(0.1f));
    }

    void Update()
    {
        if (currentScreenRatio != PlayerPrefs.GetInt("ScreenRatio"))
            GetCurrentRatio();
        GetInput();
        //Highlight the Last Man Standing button if switched to it before the end of activation animation
        if(lMSDefaultButton.GetComponent<Animator>().runtimeAnimatorController != null)
        {
            if (lMSDefaultButton.GetComponent<ButtonSelectedCheck>().selected)
            {
                if (lMSDefaultButton.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Normal"))
                {
                    lMSDefaultButton.GetComponent<Animator>().SetTrigger("Highlighted");
                }
                lMSInfoBar.SetActive(true);
                levelSelectInfoBar.SetActive(false);
            }
            else
            {
                lMSInfoBar.SetActive(false);
                levelSelectInfoBar.SetActive(true);
            }
            
        }
        
    }

    void GetInput()
    {
        for (int i = 0; i < 4; i++)
        {
            if (inputController[i].LeftHorizontal() < 0.1f && inputController[i].LeftHorizontal() > -0.1f && axisHeld[i])
                axisHeld[i] = false;
        }
        if ((Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && keyboardInSlot < 4)
        {
            axisHeld[keyboardInSlot] = false;
        }
        
        switch (currentMenuState)
        {
            case MenuState.TITLESCREEN:
                for (int i = 0; i < 4; i++)
                {
                    if (!waiting && (inputController[i].PressedStart() || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || inputController[i].PressedA()))
                    {
                        levelPreviewController.SetLevelToShow(0);
                        SwitchCanvas(1);
                    }
                }
                if (Input.GetKeyDown(KeyCode.F12))
                {
                    currentLevelChoice = 1;
                    LevelSelectToGame();
                }
                if (Input.GetKeyDown(KeyCode.Escape) && !waiting)
                {
                    QuitGame();
                }

                return;

            case MenuState.MAINMENU:
                playerSelectCameras.SetActive(false);
                for (int i = 0; i < 4; i++)
                {
                    if (inputController[i].PressedB() || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        SwitchCanvas(0);
                    }
                }
                return;

            case MenuState.OPTIONS:
                for (int i = 0; i < 4; i++)
                {
                    if ((inputController[i].PressedB() && !playerJoined[0] && !playerJoined[1] && !playerJoined[2] && !playerJoined[3]) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        SwitchCanvas(1);

                    }
                }
                return;

            case MenuState.GRAPHICS:
                for (int i = 0; i < 4; i++)
                {
                    if ((inputController[i].PressedB() && !playerJoined[0] && !playerJoined[1] && !playerJoined[2] && !playerJoined[3]) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        SwitchCanvas(5);
                    }
                }
                return;

            case MenuState.AUDIO:
                for (int i = 0; i < 4; i++)
                {
                    if ((inputController[i].PressedB() && !playerJoined[0] && !playerJoined[1] && !playerJoined[2] && !playerJoined[3]) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        SwitchCanvas(5);
                    }
                }
                return;

            case MenuState.CREDITS:
                for (int i = 0; i < 4; i++)
                {
                    if ((inputController[i].PressedB() && !playerJoined[0] && !playerJoined[1] && !playerJoined[2] && !playerJoined[3]) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        SwitchCanvas(1);
                    }
                }
                return;

            #region case PLAYERJOIN
            case MenuState.PLAYERJOIN:
                playerSelectCameras.SetActive(true);
                for (int i = 0; i < 4; i++)
                {
                    #region Player Join
                    //Player join with controller
                    if (inputController[i].PressedA() && !waiting)
                    {
                        if (!playerJoined[i])
                        {
                            StartCoroutine(PlayerWait(i, 0.1f));
                            PlayerJoin(i);
                        }
                        else
                        {
                            if (i == keyboardInSlot)
                            {
                                StartCoroutine(CheckKeyboardSlot());
                                lockedIn[i] = false;
                                if (currentScreenRatio == 0)
                                    whiteoutScreen169[i].SetActive(false);
                                else if (currentScreenRatio == 1)
                                    whiteoutScreen1610[i].SetActive(false);
                                else if (currentScreenRatio == 2)
                                    whiteoutScreen43[i].SetActive(false);

                                //Display change of slot for keyboard player
                                //Debug.Log("Keyboard changed to slot: " + keyboardInSlot);
                            }
                            else
                            {
                                if (!lockedIn[i] && !playerWait[i])
                                {
                                    lockedIn[i] = true;
                                    audioSources[i].GetComponent<AudioSource>().PlayOneShot(audioClips[1]);
                                    if (currentScreenRatio == 0)
                                        whiteoutScreen169[i].SetActive(true);
                                    else if (currentScreenRatio == 1)
                                        whiteoutScreen1610[i].SetActive(true);
                                    else if (currentScreenRatio == 2)
                                        whiteoutScreen43[i].SetActive(true);
                                    StartCoroutine(PlayerWait(i, 0.1f));
                                }
                            }
                        }
                    }

                    if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && !waiting)
                    {
                        if (keyboardInSlot < 4)
                        {
                            if (!lockedIn[keyboardInSlot] && !playerWait[keyboardInSlot])
                            {
                                lockedIn[keyboardInSlot] = true;
                                audioSources[keyboardInSlot].GetComponent<AudioSource>().PlayOneShot(audioClips[1]);
                                if (currentScreenRatio == 0)
                                    whiteoutScreen169[keyboardInSlot].SetActive(true);
                                else if (currentScreenRatio == 1)
                                    whiteoutScreen1610[keyboardInSlot].SetActive(true);
                                else if (currentScreenRatio == 2)
                                    whiteoutScreen43[keyboardInSlot].SetActive(true);
                                StartCoroutine(PlayerWait(keyboardInSlot, 0.1f));
                            }
                            else if (lockedIn[keyboardInSlot] && !playerWait[keyboardInSlot])
                            {
                                if (playerJoined[0] == lockedIn[0] && playerJoined[1] == lockedIn[1] && playerJoined[2] == lockedIn[2] && playerJoined[3] == lockedIn[3])
                                {
                                    JoinToLevelSelectScreen();
                                }
                            }
                        }
                        else
                        {
                            if ((playerJoined[0] == lockedIn[0] && playerJoined[1] == lockedIn[1] && playerJoined[2] == lockedIn[2] && playerJoined[3] == lockedIn[3]) && (playerJoined[0] && playerJoined[1] && playerJoined[2] && playerJoined[3]))
                            {
                                JoinToLevelSelectScreen();
                            }
                            else
                            {
                                StartCoroutine(CheckKeyboardSlot());
                            }
                        }
                    }

                    #endregion

                    #region Unjoin
                    if (inputController[i].PressedB() && i != keyboardInSlot)
                    {
                        if (playerJoined[i] && !lockedIn[i] && !playerWait[i])
                        {
                            playerJoined[i] = false;
                            hovercrafts[i, selectedHovercraft[i]].SetActive(false);
                            aToJoinImages[i].SetActive(true);
                            blackoutScreen[i].SetActive(true);
                            StartCoroutine(PlayerWait(i, 0.1f));
                        }
                        else if (playerJoined[i] && lockedIn[i])
                        {
                            if (currentScreenRatio == 0)
                                whiteoutScreen169[i].SetActive(false);
                            if (currentScreenRatio == 1)
                                whiteoutScreen1610[i].SetActive(false);
                            if (currentScreenRatio == 2)
                                whiteoutScreen43[i].SetActive(false);
                            lockedIn[i] = false;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (keyboardInSlot < 4)
                        {
                            if (playerJoined[keyboardInSlot] && !lockedIn[keyboardInSlot] && !playerWait[keyboardInSlot])
                            {
                                StartCoroutine(Wait(0.1f));
                                playerJoined[keyboardInSlot] = false;
                                hovercrafts[keyboardInSlot, selectedHovercraft[keyboardInSlot]].SetActive(false);
                                aToJoinImages[keyboardInSlot].SetActive(true);
                                blackoutScreen[keyboardInSlot].SetActive(true);
                                keyboardInSlot = 4;
                            }
                            else if (playerJoined[keyboardInSlot] && lockedIn[keyboardInSlot])
                            {
                                if (currentScreenRatio == 0)
                                    whiteoutScreen169[keyboardInSlot].SetActive(false);
                                if (currentScreenRatio == 1)
                                    whiteoutScreen1610[keyboardInSlot].SetActive(false);
                                if (currentScreenRatio == 2)
                                    whiteoutScreen43[keyboardInSlot].SetActive(false);
                                lockedIn[keyboardInSlot] = false;
                                StartCoroutine(PlayerWait(keyboardInSlot, 0.1f));
                            }
                        }
                    }
                    #endregion

                    if (playerJoined[0] == lockedIn[0] && playerJoined[1] == lockedIn[1] && playerJoined[2] == lockedIn[2] && playerJoined[3] == lockedIn[3])
                    {
                        if ((playerJoined[0] || playerJoined[1] || playerJoined[2] || playerJoined[3]) && !allReady)
                        {
                            allReady = true;
                            joinText.GetComponent<SmartLocalization.Editor.LocalizedText>().localizedKey = ("Start");
                            LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture);
                            flashyAButton.GetComponent<Animator>().SetTrigger("Activate");
                        }

                    }
                    else
                    {
                        if (allReady)
                        {
                            allReady = false;
                            joinText.GetComponent<SmartLocalization.Editor.LocalizedText>().localizedKey = ("JoinReady");
                            LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture);
                            flashyAButton.GetComponent<Animator>().SetTrigger("Deactivate");
                        }
                    }

                    if ((inputController[i].PressedStart() || inputController[i].PressedA()) && playerJoined[i] && !playerWait[i])
                    {
                        if (playerJoined[0] == lockedIn[0] && playerJoined[1] == lockedIn[1] && playerJoined[2] == lockedIn[2] && playerJoined[3] == lockedIn[3])
                        {
                            SwitchCanvas(3);
                        }
                    }

                    #region Hovercraft & Colour Select
                    #region Hovercraft Select
                    //Controller craft select
                    if (inputController[i].LeftHorizontal() > 0.15f && playerJoined[i] && !axisHeld[i] && !lockedIn[i] && keyboardInSlot != i)
                    {
                        axisHeld[i] = true;
                        int currentCraft = selectedHovercraft[i];
                        if (selectedHovercraft[i] >= noOfHovercrafts)
                        {
                            selectedHovercraft[i] = 0;
                        }
                        else
                        {
                            selectedHovercraft[i]++;
                        }
                        audioSources[i].GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
                        hovercrafts[i, selectedHovercraft[i]].GetComponent<BounceSpin>().SetStartRotation();
                        SetHovercraft(i, currentCraft, selectedHovercraft[i]);
                        rightArrows[i].GetComponent<Animator>().SetTrigger("Activate");
                        analogueSticks[i].GetComponent<Animator>().SetTrigger("RightAnim");
                    }
                    else if (inputController[i].LeftHorizontal() < -0.15f && playerJoined[i] && !axisHeld[i] && !lockedIn[i] && keyboardInSlot != i)
                    {
                        axisHeld[i] = true;
                        int currentCraft = selectedHovercraft[i];
                        if (selectedHovercraft[i] <= 0)
                        {
                            selectedHovercraft[i] = noOfHovercrafts;
                        }
                        else
                        {
                            selectedHovercraft[i]--;
                        }
                        audioSources[i].GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
                        hovercrafts[i, selectedHovercraft[i]].GetComponent<BounceSpin>().SetStartRotation();
                        SetHovercraft(i, currentCraft, selectedHovercraft[i]);
                        leftArrows[i].GetComponent<Animator>().SetTrigger("Activate");
                        analogueSticks[i].GetComponent<Animator>().SetTrigger("LeftAnim");
                    }
                    //Keyboard craft select
                    if (keyboardInSlot < 4)
                    {
                        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && playerJoined[keyboardInSlot] && !axisHeld[keyboardInSlot] && !lockedIn[keyboardInSlot])
                        {
                            axisHeld[keyboardInSlot] = true;
                            int currentCraft = selectedHovercraft[keyboardInSlot];
                            if (selectedHovercraft[keyboardInSlot] >= noOfHovercrafts)
                            {
                                selectedHovercraft[keyboardInSlot] = 0;
                            }
                            else
                            {
                                selectedHovercraft[keyboardInSlot]++;
                            }
                            audioSources[keyboardInSlot].GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
                            hovercrafts[keyboardInSlot, selectedHovercraft[keyboardInSlot]].GetComponent<BounceSpin>().SetStartRotation();
                            SetHovercraft(keyboardInSlot, currentCraft, selectedHovercraft[keyboardInSlot]);
                            rightArrows[keyboardInSlot].GetComponent<Animator>().SetTrigger("Activate");
                        }
                        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && playerJoined[keyboardInSlot] && !axisHeld[keyboardInSlot] && !lockedIn[keyboardInSlot])
                        {
                            axisHeld[keyboardInSlot] = true;
                            int currentCraft = selectedHovercraft[keyboardInSlot];
                            if (selectedHovercraft[keyboardInSlot] <= 0)
                            {
                                selectedHovercraft[keyboardInSlot] = noOfHovercrafts;
                            }
                            else
                            {
                                selectedHovercraft[keyboardInSlot]--;
                            }
                            audioSources[keyboardInSlot].GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
                            hovercrafts[keyboardInSlot, selectedHovercraft[keyboardInSlot]].GetComponent<BounceSpin>().SetStartRotation();
                            SetHovercraft(keyboardInSlot, currentCraft, selectedHovercraft[keyboardInSlot]);
                            leftArrows[keyboardInSlot].GetComponent<Animator>().SetTrigger("Activate");
                        }
                    }

                    #endregion
                    #region Colour Select
                    if (inputController[i].PressedRightShoulder() && playerJoined[i] && !lockedIn[i])
                    {
                        if (playerColour[i] >= noOfColours - 1)
                        {
                            playerColour[i] = 0;
                        }
                        else
                        {
                            playerColour[i]++;
                        }
                        audioSources[i].GetComponent<AudioSource>().PlayOneShot(audioClips[Random.Range(2, audioClips.Length)]);
                        StartCoroutine(ColourCheck(i, true));
                        rightShoulders[i].GetComponent<Animator>().SetTrigger("Activate");
                    }
                    else if (inputController[i].PressedLeftShoulder() && playerJoined[i] && !lockedIn[i])
                    {
                        if (playerColour[i] == 0)
                        {
                            playerColour[i] = noOfColours - 1;
                        }
                        else
                        {
                            playerColour[i]--;
                        }
                        audioSources[i].GetComponent<AudioSource>().PlayOneShot(audioClips[Random.Range(2, audioClips.Length)]);
                        StartCoroutine(ColourCheck(i, false));
                        leftShoulders[i].GetComponent<Animator>().SetTrigger("Activate");
                    }
                    if (keyboardInSlot < 4)
                    {
                        if (Input.GetKeyDown(KeyCode.E) && !axisHeld[keyboardInSlot] && playerJoined[keyboardInSlot] && !lockedIn[keyboardInSlot])
                        {
                            axisHeld[keyboardInSlot] = true;
                            if (playerColour[keyboardInSlot] >= noOfColours - 1)
                            {
                                playerColour[keyboardInSlot] = 0;
                            }
                            else
                            {
                                playerColour[keyboardInSlot]++;
                            }
                            audioSources[keyboardInSlot].GetComponent<AudioSource>().PlayOneShot(audioClips[Random.Range(2, audioClips.Length)]);
                            //Debug.Log(playerColour[keyboardInSlot]);
                            StartCoroutine(ColourCheck(keyboardInSlot, true));
                            rightShoulders[keyboardInSlot].GetComponent<Animator>().SetTrigger("Activate");
                        }
                        else if (Input.GetKeyDown(KeyCode.Q) && !axisHeld[keyboardInSlot] && playerJoined[keyboardInSlot] && !lockedIn[keyboardInSlot])
                        {
                            axisHeld[keyboardInSlot] = true;
                            if (playerColour[keyboardInSlot] == 0)
                            {
                                playerColour[keyboardInSlot] = noOfColours - 1;
                            }
                            else
                            {
                                playerColour[keyboardInSlot]--;
                            }
                            audioSources[keyboardInSlot].GetComponent<AudioSource>().PlayOneShot(audioClips[Random.Range(2, audioClips.Length)]);
                            StartCoroutine(ColourCheck(keyboardInSlot, false));
                            leftShoulders[keyboardInSlot].GetComponent<Animator>().SetTrigger("Activate");
                        }
                    }
                    #endregion
                    #endregion

                    if (((inputController[i].PressedB() && !playerWait[i]) || (Input.GetKeyDown(KeyCode.Backspace) && !waiting) || (Input.GetKeyDown(KeyCode.Escape) && !waiting)) && !playerJoined[0] && !playerJoined[1] && !playerJoined[2] && !playerJoined[3])
                    {
                        SwitchCanvas(1);
                    }
                }
                return;
            #endregion

            #region LEVELSELECT
            case MenuState.LEVELSELECT:
                playerSelectCameras.SetActive(false);
                for (int i = 0; i < 4; i++)
                {
                    if ((inputController[i].LeftHorizontal() > 0.15f || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && playerJoined[i] && !axisHeld[i] && !waiting)
                    {
                        //if (levelSelectLevel[0].GetComponent<ButtonSelectedCheck>().GetSelected() || levelSelectLevel[1].GetComponent<ButtonSelectedCheck>().GetSelected() || levelSelectLevel[2].GetComponent<ButtonSelectedCheck>().GetSelected()
                        if (levelSelectLevel.GetComponent<ButtonSelectedCheck>().GetSelected())
                        {
                            rightLevelSelectArrows.GetComponent<Animator>().SetTrigger("Activate");
                            StartCoroutine(Wait(0.1f));
                            axisHeld[i] = true;
                            if (currentLevelChoice >= (numberOfLevels - 1))
                            {
                                currentLevelChoice = 0;
                                levelPreviewController.SetLevelToShow(currentLevelChoice + 1);
                            }
                            else
                            {
                                currentLevelChoice++;
                                levelPreviewController.SetLevelToShow(currentLevelChoice + 1);
                            }
                            levelChoiceText.GetComponent<SmartLocalization.Editor.LocalizedText>().localizedKey = ("Level" + (currentLevelChoice + 1));
                            LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture);
                        }
                        else if (levelSelectGameMode.GetComponent<ButtonSelectedCheck>().GetSelected())
                        {
                            rightGameModeArrow.GetComponent<Animator>().SetTrigger("Activate");
                            StartCoroutine(Wait(0.1f));
                            axisHeld[i] = true;
                            if (currentGameModeChoice >= (numberOfGameModes - 1))
                            {
                                currentGameModeChoice = 0;
                            }
                            else
                            {
                                currentGameModeChoice++;
                            }
                            gameModeChoiceText.GetComponent<SmartLocalization.Editor.LocalizedText>().localizedKey = (gameModeStringKeys[currentGameModeChoice]);
                            LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture);
                            if (currentGameModeChoice == 1)
                            {
                                StartCoroutine(Wait(0.5f));
                                levelSelectScreen.GetComponent<Animator>().SetTrigger("Activate");
                                Navigation gameModeButton = levelSelectGameMode.GetComponent<Button>().navigation;
                                Navigation playButton = levelSelectPlay.GetComponent<Button>().navigation;
                                Button lMSButton = lMSDefaultButton.GetComponent<Button>();
                                gameModeButton.selectOnDown = lMSButton;
                                playButton.selectOnUp = lMSButton;
                                levelSelectGameMode.GetComponent<Button>().navigation = gameModeButton;
                                levelSelectPlay.GetComponent<Button>().navigation = playButton;
                            }
                            else
                            {
                                StartCoroutine(Wait(0.5f));
                                lMSDefaultButton.GetComponent<Animator>().runtimeAnimatorController = null;
                                levelSelectScreen.GetComponent<Animator>().SetTrigger("Deactivate");
                                Navigation gameModeButton = levelSelectGameMode.GetComponent<Button>().navigation;
                                Navigation playButton = levelSelectPlay.GetComponent<Button>().navigation;
                                gameModeButton.selectOnDown = levelSelectPlay.GetComponent<Button>();
                                playButton.selectOnUp = levelSelectGameMode.GetComponent<Button>();
                                levelSelectGameMode.GetComponent<Button>().navigation = gameModeButton;
                                levelSelectPlay.GetComponent<Button>().navigation = playButton;
                            }
                        }
                        else if (lMSDefaultButton.GetComponent<ButtonSelectedCheck>().GetSelected())
                        {
                            lMSRightArrow.GetComponent<Animator>().SetTrigger("Activate");
                            StartCoroutine(Wait(0.1f));
                            axisHeld[i] = true;
                            if (currentNoOfLives >= 99)
                            {
                                currentNoOfLives = 99;
                                noOfLivesText.text = currentNoOfLives.ToString();
                            }
                            else
                            {
                                currentNoOfLives++;
                                noOfLivesText.text = currentNoOfLives.ToString();
                            }
                        }
                        audioSources[i].GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
                    }
                    if ((inputController[i].LeftHorizontal() < -0.15f || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && playerJoined[i] && !axisHeld[i] && !waiting)
                    {
                        //if (levelSelectLevel[0].GetComponent<ButtonSelectedCheck>().GetSelected() || levelSelectLevel[1].GetComponent<ButtonSelectedCheck>().GetSelected() || levelSelectLevel[2].GetComponent<ButtonSelectedCheck>().GetSelected())
                        if (levelSelectLevel.GetComponent<ButtonSelectedCheck>().GetSelected())
                        {
                            leftLevelSelectArrows.GetComponent<Animator>().SetTrigger("Activate");
                            StartCoroutine(Wait(0.1f));
                            axisHeld[i] = true;
                            if (currentLevelChoice <= 0)
                            {
                                currentLevelChoice = (numberOfLevels - 1);
                                levelPreviewController.SetLevelToShow(currentLevelChoice + 1);
                            }
                            else
                            {
                                currentLevelChoice--;
                                levelPreviewController.SetLevelToShow(currentLevelChoice + 1);
                            }
                            levelChoiceText.GetComponent<SmartLocalization.Editor.LocalizedText>().localizedKey = ("Level" + (currentLevelChoice + 1));
                            LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture);
                        }
                        else if (levelSelectGameMode.GetComponent<ButtonSelectedCheck>().GetSelected())
                        {
                            leftGameModeArrow.GetComponent<Animator>().SetTrigger("Activate");
                            StartCoroutine(Wait(0.1f));
                            axisHeld[i] = true;
                            if (currentGameModeChoice <= 0)
                            {
                                currentGameModeChoice = (numberOfGameModes - 1);
                            }
                            else
                            {
                                currentGameModeChoice--;
                            }
                            gameModeChoiceText.GetComponent<SmartLocalization.Editor.LocalizedText>().localizedKey = (gameModeStringKeys[currentGameModeChoice]);
                            LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture);
                            if (currentGameModeChoice == 1)
                            {
                                StartCoroutine(Wait(0.5f));
                                levelSelectScreen.GetComponent<Animator>().SetTrigger("Activate");
                                //StartCoroutine(AddButtonAnimator(lMSDefaultButton, 0.5f));

                                Navigation gameModeButton = levelSelectGameMode.GetComponent<Button>().navigation;
                                Navigation playButton = levelSelectPlay.GetComponent<Button>().navigation;
                                Button lMSButton = lMSDefaultButton.GetComponent<Button>();
                                gameModeButton.selectOnDown = lMSButton;
                                playButton.selectOnUp = lMSButton;
                                levelSelectGameMode.GetComponent<Button>().navigation = gameModeButton;
                                levelSelectPlay.GetComponent<Button>().navigation = playButton;
                                Debug.Log("Enable Lives");
                            }
                            else
                            {
                                StartCoroutine(Wait(0.5f));
                                lMSDefaultButton.GetComponent<Animator>().runtimeAnimatorController = null;
                                levelSelectScreen.GetComponent<Animator>().SetTrigger("Deactivate");
                                Navigation gameModeButton = levelSelectGameMode.GetComponent<Button>().navigation;
                                Navigation playButton = levelSelectPlay.GetComponent<Button>().navigation;
                                gameModeButton.selectOnDown = levelSelectPlay.GetComponent<Button>();
                                playButton.selectOnUp = levelSelectGameMode.GetComponent<Button>();
                                levelSelectGameMode.GetComponent<Button>().navigation = gameModeButton;
                                levelSelectPlay.GetComponent<Button>().navigation = playButton;
                                Debug.Log("Disable Lives");
                            }
                        }
                        else if (lMSDefaultButton.GetComponent<ButtonSelectedCheck>().GetSelected())
                        {
                            lMSLeftArrow.GetComponent<Animator>().SetTrigger("Activate");
                            StartCoroutine(Wait(0.1f));
                            axisHeld[i] = true;
                            if (currentNoOfLives <= 1)
                            {
                                currentNoOfLives = 1;
                                noOfLivesText.text = currentNoOfLives.ToString();
                            }
                            else
                            {
                                currentNoOfLives--;
                                noOfLivesText.text = currentNoOfLives.ToString();
                            }
                        }
                        audioSources[i].GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
                    }
                    if ((inputController[i].PressedLeftShoulder() || Input.GetKeyDown(KeyCode.Q)) && playerJoined[i] && !axisHeld[i] && !waiting)
                    {
                        if (lMSDefaultButton.GetComponent<ButtonSelectedCheck>().GetSelected())
                        {
                            //leftGameModeArrow.GetComponent<Animator>().SetTrigger("Activate");
                            StartCoroutine(Wait(0.1f));
                            axisHeld[i] = true;
                            if (currentNoOfLives <= 10)
                            {
                                currentNoOfLives = 1;
                                noOfLivesText.text = currentNoOfLives.ToString();
                            }
                            else
                            {
                                currentNoOfLives -= 10;
                                noOfLivesText.text = currentNoOfLives.ToString();
                            }
                        }
                        audioSources[i].GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
                    }
                    if ((inputController[i].PressedRightShoulder() || Input.GetKeyDown(KeyCode.E)) && playerJoined[i] && !axisHeld[i] && !waiting)
                    {
                        if (lMSDefaultButton.GetComponent<ButtonSelectedCheck>().GetSelected())
                        {
                            //rightGameModeArrow.GetComponent<Animator>().SetTrigger("Activate");
                            StartCoroutine(Wait(0.1f));
                            axisHeld[i] = true;
                            if (currentNoOfLives >= 89)
                            {
                                currentNoOfLives = 99;
                                noOfLivesText.text = currentNoOfLives.ToString();
                            }
                            else
                            {
                                currentNoOfLives += 10;
                                noOfLivesText.text = currentNoOfLives.ToString();
                            }
                        }
                        audioSources[i].GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
                    }
                    if (inputController[i].PressedB() || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        LevelSelectToJoinScreen();
                    }

                }
                return;
            #endregion

            case MenuState.CONTROLS:
                for (int i = 0; i < 4; i++)
                {
                    if ((inputController[i].LeftHorizontal() > 0.15f || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && !axisHeld[i] && !waiting)
                    {
                        axisHeld[i] = true;
                        ShowKeyboardControls(true);
                    }
                    if ((inputController[i].LeftHorizontal() < -0.15f || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && !axisHeld[i] && !waiting)
                    {
                        axisHeld[i] = true;
                        ShowKeyboardControls(false);
                    }
                    if (inputController[i].PressedB() || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        SwitchCanvas(5);
                    }

                }
                return;

            case MenuState.LOADLEVEL:
                
                return;
        }
    }

    #region Menu Navigation

    public void SwitchCanvas(int nextMenu)
    {
        if (!waiting)
        {
            StartCoroutine(Wait(0.1f));
            canvasHolder[(int)currentMenuState].SetActive(false);
            currentMenuState = (MenuState)nextMenu;
            canvasHolder[(int)currentMenuState].SetActive(true);
            EventSystem.current.SetSelectedGameObject(canvasHolder[(int)currentMenuState]);
            if (defaultButtons[(int)currentMenuState] != null)
                EventSystem.current.SetSelectedGameObject(defaultButtons[(int)currentMenuState]);
        }
    }

    #endregion

    #region Menu Methods
    void TitleScreenActive(bool isActive)
    {
        titleScreen.SetActive(isActive);
        //titleScreen[1].SetActive(isActive);
        //titleScreen[2].SetActive(isActive);
    }

    void MainMenuScreenActive(bool isActive)
    {
        mainMenuScreen.SetActive(isActive);
        //mainMenuScreen[1].SetActive(isActive);
        //mainMenuScreen[2].SetActive(isActive);
    }

    void OptionsScreenActive(bool isActive)
    {
        optionsScreen.SetActive(isActive);
        //optionsScreen[1].SetActive(isActive);
        //optionsScreen[2].SetActive(isActive);
    }

    void CreditsScreenActive(bool isActive)
    {
        creditsScreen.SetActive(isActive);
        //optionsScreen[1].SetActive(isActive);
        //optionsScreen[2].SetActive(isActive);
    }

    void AudioScreenActive(bool isActive)
    {
        audioScreen.SetActive(isActive);
        //audioScreen[1].SetActive(isActive);
        //audioScreen[2].SetActive(isActive);
    }

    void GraphicsScreenActive(bool isActive)
    {
        graphicsScreen.SetActive(isActive);
        //graphicsScreen[1].SetActive(isActive);
        //graphicsScreen[2].SetActive(isActive);
    }

    void ControlsScreenActive(bool isActive)
    {
        controlsScreen.SetActive(isActive);
        //controlsScreen[1].SetActive(isActive);
        //controlsScreen[2].SetActive(isActive);
    }

    void PlayerJoinScreenActive(bool isActive)
    {
        playerJoinScreen.SetActive(isActive);
        //playerJoinScreen[1].SetActive(isActive);
        //playerJoinScreen[2].SetActive(isActive);
        playerSelectCameras.SetActive(isActive);
    }

    void LevelSelectScreenActive(bool isActive)
    {
        levelSelectScreen.SetActive(isActive);
        //levelSelectScreen[1].SetActive(isActive);
        //levelSelectScreen[2].SetActive(isActive);
    }

    void LoadingScreenActive(bool isActive)
    {
        loadingScreen.SetActive(isActive);
        //loadingScreen[1].SetActive(isActive);
        //loadingScreen[2].SetActive(isActive);
    }
    #endregion

    #region Button Methods

    public void TitleToMenuScreen()
    {
        GetCurrentRatio();
        TitleScreenActive(false);
        MainMenuScreenActive(true);
        currentMenuState = MenuState.MAINMENU;
        StartCoroutine(Wait(0.1f));
        levelPreviewController.SetLevelToShow(0);
        //EventSystem.current.SetSelectedGameObject(resolutionParent[currentScreenRatio]);
        EventSystem.current.SetSelectedGameObject(mainMenuScreen);
        EventSystem.current.SetSelectedGameObject(mainMenuPlay);
    }

    public void MainToTitleScreen()
    {
        if (!waiting)
        {
            MainMenuScreenActive(false);
            TitleScreenActive(true);
            StartCoroutine(Wait(0.1f));
            currentMenuState = MenuState.TITLESCREEN;
        }
    }

    public void MainToCreditsScreen()
    {
        if (!waiting)
        {
            MainMenuScreenActive(false);
            CreditsScreenActive(true);
            StartCoroutine(Wait(0.1f));
            currentMenuState = MenuState.CREDITS;
        }
    }

    public void CreditsToMenuScreen()
    {
        if (!waiting)
        {
            CreditsScreenActive(false);
            MainMenuScreenActive(true);
            StartCoroutine(Wait(0.1f));
            currentMenuState = MenuState.MAINMENU;
            EventSystem.current.SetSelectedGameObject(mainMenuScreen);
            EventSystem.current.SetSelectedGameObject(mainMenuPlay);
        }
    }

    public void MainToJoinScreen()
    {
        if (!waiting)
        {
            MainMenuScreenActive(false);
            PlayerJoinScreenActive(true);
            StartCoroutine(Wait(0.1f));
            currentMenuState = MenuState.PLAYERJOIN;
        }
    }

    public void MainToOptionsScreen()
    {
        GetCurrentRatio();
        MainMenuScreenActive(false);
        OptionsScreenActive(true);
        StartCoroutine(Wait(0.1f));
        currentMenuState = MenuState.OPTIONS;
        //EventSystem.current.SetSelectedGameObject(resolutionParent[currentScreenRatio]);
        EventSystem.current.SetSelectedGameObject(optionsScreen);
        EventSystem.current.SetSelectedGameObject(optionsDefaultButton);
    }

    public void OptionsToAudioScreen()
    {
        GetCurrentRatio();
        OptionsScreenActive(false);
        AudioScreenActive(true);
        StartCoroutine(Wait(0.1f));
        currentMenuState = MenuState.AUDIO;
        //EventSystem.current.SetSelectedGameObject(resolutionParent[currentScreenRatio]);
        EventSystem.current.SetSelectedGameObject(audioScreen);
        EventSystem.current.SetSelectedGameObject(audioDefaultButton);
    }

    public void AudioToOptionsScreen()
    {
        GetCurrentRatio();
        AudioScreenActive(false);
        OptionsScreenActive(true);
        StartCoroutine(Wait(0.1f));
        currentMenuState = MenuState.OPTIONS;
        //EventSystem.current.SetSelectedGameObject(resolutionParent[currentScreenRatio]);
        EventSystem.current.SetSelectedGameObject(optionsScreen);
        EventSystem.current.SetSelectedGameObject(optionsDefaultButton);
    }

    public void OptionsToGraphicsScreen()
    {
        GetCurrentRatio();
        OptionsScreenActive(false);
        GraphicsScreenActive(true);
        StartCoroutine(Wait(0.1f));
        currentMenuState = MenuState.GRAPHICS;
        //EventSystem.current.SetSelectedGameObject(resolutionParent[currentScreenRatio]);
        EventSystem.current.SetSelectedGameObject(graphicsScreen);
        EventSystem.current.SetSelectedGameObject(graphicsDefaultButton);
    }

    public void OptionsToControlsScreen()
    {
        OptionsScreenActive(false);
        ControlsScreenActive(true);
        StartCoroutine(Wait(0.1f));
        currentMenuState = MenuState.CONTROLS;
    }

    public void GraphicsToOptionsScreen()
    {
        GetCurrentRatio();
        GraphicsScreenActive(false);
        OptionsScreenActive(true);
        StartCoroutine(Wait(0.1f));
        currentMenuState = MenuState.OPTIONS;
        //EventSystem.current.SetSelectedGameObject(resolutionParent[currentScreenRatio]);
        EventSystem.current.SetSelectedGameObject(optionsScreen);
        EventSystem.current.SetSelectedGameObject(optionsDefaultButton);
    }

    public void ControlsToOptionsScreen()
    {
        GetCurrentRatio();
        ControlsScreenActive(false);
        OptionsScreenActive(true);
        StartCoroutine(Wait(0.1f));
        currentMenuState = MenuState.OPTIONS;
        //EventSystem.current.SetSelectedGameObject(resolutionParent[currentScreenRatio]);
        EventSystem.current.SetSelectedGameObject(optionsScreen);
        EventSystem.current.SetSelectedGameObject(optionsDefaultButton);
    }

    public void OptionsToMainScreen()
    {
        GetCurrentRatio();
        OptionsScreenActive(false);
        MainMenuScreenActive(true);
        StartCoroutine(Wait(0.1f));
        currentMenuState = MenuState.OPTIONS;
        levelPreviewController.SetLevelToShow(0);
        //EventSystem.current.SetSelectedGameObject(resolutionParent[currentScreenRatio]);
        EventSystem.current.SetSelectedGameObject(mainMenuScreen);
        EventSystem.current.SetSelectedGameObject(mainMenuPlay);
    }

    public void JoinToMainScreen()
    {
        GetCurrentRatio();
        PlayerJoinScreenActive(false);
        MainMenuScreenActive(true);
        StartCoroutine(Wait(0.1f));
        currentMenuState = MenuState.MAINMENU;
        levelPreviewController.SetLevelToShow(0);
        //EventSystem.current.SetSelectedGameObject(resolutionParent[currentScreenRatio]);
        EventSystem.current.SetSelectedGameObject(mainMenuScreen);
        EventSystem.current.SetSelectedGameObject(mainMenuPlay);
    }

    public void LevelSelectToJoinScreen()
    {
        //for (int i = 0; i < 4; i++)
        //{
        //    playerJoined[i] = false;
        //}
        LevelSelectScreenActive(false);
        PlayerJoinScreenActive(true);
        StartCoroutine(Wait(0.1f));
        currentMenuState = MenuState.PLAYERJOIN;
    }

    public void JoinToLevelSelectScreen()
    {
        GetCurrentRatio();
        PlayerJoinScreenActive(false);
        LevelSelectScreenActive(true);
        //StartCoroutine("Wait", 0.5f);
        currentMenuState = MenuState.LEVELSELECT;
        //for (int i = 0; i < numberOfLevels; i++)
        //{
        //    levelVideos[i].SetActive(false);
        //}
        //levelVideos[currentLevelChoice].SetActive(true);
        levelPreviewController.SetLevelToShow(currentLevelChoice + 1);
        for (int i = 0; i < 3; i++)
        {
            levelChoiceText.GetComponent<SmartLocalization.Editor.LocalizedText>().localizedKey = ("Level" + (currentLevelChoice + 1));
        }
        LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture);

        //EventSystem.current.SetSelectedGameObject(resolutionParent[currentScreenRatio]);
        EventSystem.current.SetSelectedGameObject(levelSelectScreen);
        EventSystem.current.SetSelectedGameObject(levelSelectLevel);

    }

    public void LevelSelectToGame()
    {
        //if (currentGameModeChoice == 0)
        //{
            currentMenuState = MenuState.LOADLEVEL;
            LevelSelectScreenActive(false);
            LoadingScreenActive(true);
            StartCoroutine(LoadLevel(currentLevelChoice));
        //}
        //else if (currentGameModeChoice == 1)
        //{
        //    currentMenuState = MenuState.LMSSCREEN;
        //    lMSScreen.SetActive(true);
        //    levelSelectButtons.SetActive(false);
        //    levelSelectInfoBar.SetActive(false);
        //    lMSInfoBar.SetActive(true);
        //    EventSystem.current.SetSelectedGameObject(lMSScreen);
        //    EventSystem.current.SetSelectedGameObject(lMSDefaultButton);
        //}
    }

    public void LMSToLevelSelect()
    {
        currentMenuState = MenuState.LEVELSELECT;
        lMSScreen.SetActive(false);
        levelSelectButtons.SetActive(true);
        levelSelectInfoBar.SetActive(true);
        lMSInfoBar.SetActive(false);
        EventSystem.current.SetSelectedGameObject(levelSelectScreen);
        EventSystem.current.SetSelectedGameObject(levelSelectLevel);
    }

    public void LMSToGame()
    {
        currentMenuState = MenuState.LOADLEVEL;
        LevelSelectScreenActive(false);
        LoadingScreenActive(true);
        StartCoroutine(LoadLevel(currentLevelChoice));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    IEnumerator Wait(float timeToWait)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(timeToWait);
        waiting = false;
    }

    IEnumerator PlayerWait(int player, float timeToWait)
    {
        if (player < 4)
        {
            playerWait[player] = true;
            yield return new WaitForSeconds(timeToWait);
            playerWait[player] = false;
        }
    }

    public void AddLMSButtonAnimator()
    {
        lMSDefaultButton.GetComponent<Animator>().runtimeAnimatorController = buttonAnimationController;
    }

    #region Colour Methods
    void SetColours()
    {
        colours[0] = new Color(1.0f, 0.0f, 0.0f);   //Red
        colours[1] = new Color(1.0f, 1.0f, 0.0f);   //Yellow
        colours[2] = new Color(0.0f, 0.7f, 0.0f);   //Green
        colours[3] = new Color(0.55f, 0.0f, 0.63f); //Purple
        colours[4] = new Color(0.0f, 0.7f, 1.0f);   //Blue
        colours[5] = new Color(0.0f, 0.0f, 0.0f);   //Black
        colours[6] = new Color(1.0f, 1.0f, 1.0f);   //White
        colours[7] = new Color(1.0f, 0.41f, 0.7f);  //Pink
        colours[8] = new Color(1.0f, 0.43f, 0.0f);  //Orange
        for (int i = 0; i < 4; i++)
        {
            ApplyColour(i, colours[playerColour[i]]);
        }
    }

    IEnumerator ColourCheck(int playerNumber, bool increment)
    {
        bool checking = true;
        while (checking)
        {
            bool checkAgain = false;
            if (playerNumber == 0)
            {
                if ((playerColour[playerNumber] == playerColour[1] && playerJoined[1]) || (playerColour[playerNumber] == playerColour[2] && playerJoined[2]) || (playerColour[playerNumber] == playerColour[3] && playerJoined[3]))
                {
                    checkAgain = true;
                }
            }
            else if (playerNumber == 1)
            {
                if ((playerColour[playerNumber] == playerColour[0] && playerJoined[0]) || (playerColour[playerNumber] == playerColour[2] && playerJoined[2]) || (playerColour[playerNumber] == playerColour[3] && playerJoined[3]))
                {
                    checkAgain = true;
                }
            }
            else if (playerNumber == 2)
            {
                if ((playerColour[playerNumber] == playerColour[0] && playerJoined[0]) || (playerColour[playerNumber] == playerColour[1] && playerJoined[1]) || (playerColour[playerNumber] == playerColour[3] && playerJoined[3]))
                {
                    checkAgain = true;
                }
            }
            else if (playerNumber == 3)
            {
                if ((playerColour[playerNumber] == playerColour[0] && playerJoined[0]) || (playerColour[playerNumber] == playerColour[1] && playerJoined[1]) || (playerColour[playerNumber] == playerColour[2] && playerJoined[2]))
                {
                    checkAgain = true;
                }
            }
            if (checkAgain == true)
            {
                checking = true;
                if (increment)
                {
                    if (playerColour[playerNumber] == noOfColours - 1)
                    {
                        playerColour[playerNumber] = 0;
                    }
                    else
                    {
                        playerColour[playerNumber]++;
                    }
                }
                else
                {
                    if (playerColour[playerNumber] == 0)
                    {
                        playerColour[playerNumber] = noOfColours - 1;
                    }
                    else
                    {
                        playerColour[playerNumber]--;
                    }
                }
            }
            else
            {
                checking = false;
            }
            yield return !checking;
        }
        yield return !checking;

        ApplyColour(playerNumber, colours[playerColour[playerNumber]]);
    }

    IEnumerator FinalColourCheck(int playerNumber, bool increment)
    {
        bool checking = true;
        while (checking)
        {
            bool checkAgain = false;
            if (playerNumber == 0)
            {
                if ((playerColour[playerNumber] == playerColour[1]) || (playerColour[playerNumber] == playerColour[2]) || (playerColour[playerNumber] == playerColour[3]))
                {
                    checkAgain = true;
                }
            }
            else if (playerNumber == 1)
            {
                if ((playerColour[playerNumber] == playerColour[0]) || (playerColour[playerNumber] == playerColour[2]) || (playerColour[playerNumber] == playerColour[3]))
                {
                    checkAgain = true;
                }
            }
            else if (playerNumber == 2)
            {
                if ((playerColour[playerNumber] == playerColour[0]) || (playerColour[playerNumber] == playerColour[1]) || (playerColour[playerNumber] == playerColour[3]))
                {
                    checkAgain = true;
                }
            }
            else if (playerNumber == 3)
            {
                if ((playerColour[playerNumber] == playerColour[0]) || (playerColour[playerNumber] == playerColour[1]) || (playerColour[playerNumber] == playerColour[2]))
                {
                    checkAgain = true;
                }
            }
            if (checkAgain == true)
            {
                checking = true;
                if (increment)
                {
                    if (playerColour[playerNumber] == noOfColours - 1)
                    {
                        playerColour[playerNumber] = 0;
                    }
                    else
                    {
                        playerColour[playerNumber]++;
                    }
                }
                else
                {
                    if (playerColour[playerNumber] == 0)
                    {
                        playerColour[playerNumber] = noOfColours - 1;
                    }
                    else
                    {
                        playerColour[playerNumber]--;
                    }
                }
            }
            else
            {
                checking = false;
            }
            yield return !checking;
        }
        yield return !checking;

        ApplyColour(playerNumber, colours[playerColour[playerNumber]]);
    }

    void ApplyColour(int playerNumber, Color colourToApply)
    {
        shaders[playerNumber, 0].SetColor("_PlayerColor", colourToApply);
        shaders[playerNumber, 1].SetColor("_PlayerColor", colourToApply);
        shaders[playerNumber, 2].SetColor("_PlayerColor", colourToApply);
        shaders[playerNumber, 3].SetColor("_PlayerColor", colourToApply);
        aiColoursChecked[playerNumber] = true;
    }
    #endregion

    #region Player Join

    void PlayerJoin(int player)
    {
        if (player > 3)
            return;
        audioSources[player].GetComponent<AudioSource>().PlayOneShot(audioClips[1]);
        playerJoined[player] = true;
        StartCoroutine(ColourCheck(player, true));
        aToJoinImages[player].SetActive(false);
        blackoutScreen[player].SetActive(false);
        hovercraftTypeName[player].SetActive(true);
        selectedHovercraft[player] = 0;
        SetHovercraft(player, selectedHovercraft[player], selectedHovercraft[player]);
    }

    #endregion

    void SetHovercraft(int player, int previousCraft, int newCraft)
    {
        hovercrafts[player, previousCraft].SetActive(false);
        hovercrafts[player, newCraft].SetActive(true);

        for (int i = 0; i < 5; i++)
        {
            StatsFillHolder[player, 0, i].SetActive(false);
            StatsFillHolder[player, 1, i].SetActive(false);
            StatsFillHolder[player, 2, i].SetActive(false);
        }

        for (int i = 0; i < hovercraftStats[newCraft, 0]; i++)
        {
            StatsFillHolder[player, 0, i].SetActive(true);
        }
        for (int i = 0; i < hovercraftStats[newCraft, 1]; i++)
        {
            StatsFillHolder[player, 1, i].SetActive(true);
        }
        for (int i = 0; i < hovercraftStats[newCraft, 2]; i++)
        {
            StatsFillHolder[player, 2, i].SetActive(true);
        }

        if (newCraft == 0)
            hovercraftTypeName[player].GetComponent<SmartLocalization.Editor.LocalizedText>().localizedKey = "Hovercraft";
        else if (newCraft == 1)
            hovercraftTypeName[player].GetComponent<SmartLocalization.Editor.LocalizedText>().localizedKey = "Sportscraft";
        else if (newCraft == 2)
            hovercraftTypeName[player].GetComponent<SmartLocalization.Editor.LocalizedText>().localizedKey = "Heavycraft";
        else if (newCraft == 3)
            hovercraftTypeName[player].GetComponent<SmartLocalization.Editor.LocalizedText>().localizedKey = "Musclecraft";
        LanguageManager.Instance.ChangeLanguage(LanguageManager.Instance.CurrentlyLoadedCulture);
    }

    void SetHovercraftStats()
    {
        #region Hovercraft Stats
        hovercraftStats[0, 0] = 4;
        hovercraftStats[0, 1] = 3;
        hovercraftStats[0, 2] = 3;
        #endregion

        #region Sportscraft Stats
        hovercraftStats[1, 0] = 5;
        hovercraftStats[1, 1] = 1;
        hovercraftStats[1, 2] = 4;
        #endregion

        #region Heavycraft Stats
        hovercraftStats[2, 0] = 1;
        hovercraftStats[2, 1] = 5;
        hovercraftStats[2, 2] = 4;
        #endregion

        #region Musclecraft Stats
        hovercraftStats[3, 0] = 2;
        hovercraftStats[3, 1] = 3;
        hovercraftStats[3, 2] = 5;
        #endregion
    }

    void PopulateStatFillHolder()
    {
        #region Player 1 Stats
        StatsFillHolder[0, 0, 0] = p1SpeedFill[0];
        StatsFillHolder[0, 0, 1] = p1SpeedFill[1];
        StatsFillHolder[0, 0, 2] = p1SpeedFill[2];
        StatsFillHolder[0, 0, 3] = p1SpeedFill[3];
        StatsFillHolder[0, 0, 4] = p1SpeedFill[4];

        StatsFillHolder[0, 1, 0] = p1WeightFill[0];
        StatsFillHolder[0, 1, 1] = p1WeightFill[1];
        StatsFillHolder[0, 1, 2] = p1WeightFill[2];
        StatsFillHolder[0, 1, 3] = p1WeightFill[3];
        StatsFillHolder[0, 1, 4] = p1WeightFill[4];

        StatsFillHolder[0, 2, 0] = p1HandlingFill[0];
        StatsFillHolder[0, 2, 1] = p1HandlingFill[1];
        StatsFillHolder[0, 2, 2] = p1HandlingFill[2];
        StatsFillHolder[0, 2, 3] = p1HandlingFill[3];
        StatsFillHolder[0, 2, 4] = p1HandlingFill[4];
        #endregion

        #region Player 2 Stats
        StatsFillHolder[1, 0, 0] = p2SpeedFill[0];
        StatsFillHolder[1, 0, 1] = p2SpeedFill[1];
        StatsFillHolder[1, 0, 2] = p2SpeedFill[2];
        StatsFillHolder[1, 0, 3] = p2SpeedFill[3];
        StatsFillHolder[1, 0, 4] = p2SpeedFill[4];

        StatsFillHolder[1, 1, 0] = p2WeightFill[0];
        StatsFillHolder[1, 1, 1] = p2WeightFill[1];
        StatsFillHolder[1, 1, 2] = p2WeightFill[2];
        StatsFillHolder[1, 1, 3] = p2WeightFill[3];
        StatsFillHolder[1, 1, 4] = p2WeightFill[4];

        StatsFillHolder[1, 2, 0] = p2HandlingFill[0];
        StatsFillHolder[1, 2, 1] = p2HandlingFill[1];
        StatsFillHolder[1, 2, 2] = p2HandlingFill[2];
        StatsFillHolder[1, 2, 3] = p2HandlingFill[3];
        StatsFillHolder[1, 2, 4] = p2HandlingFill[4];
        #endregion

        #region Player 3 Stats
        StatsFillHolder[2, 0, 0] = p3SpeedFill[0];
        StatsFillHolder[2, 0, 1] = p3SpeedFill[1];
        StatsFillHolder[2, 0, 2] = p3SpeedFill[2];
        StatsFillHolder[2, 0, 3] = p3SpeedFill[3];
        StatsFillHolder[2, 0, 4] = p3SpeedFill[4];

        StatsFillHolder[2, 1, 0] = p3WeightFill[0];
        StatsFillHolder[2, 1, 1] = p3WeightFill[1];
        StatsFillHolder[2, 1, 2] = p3WeightFill[2];
        StatsFillHolder[2, 1, 3] = p3WeightFill[3];
        StatsFillHolder[2, 1, 4] = p3WeightFill[4];

        StatsFillHolder[2, 2, 0] = p3HandlingFill[0];
        StatsFillHolder[2, 2, 1] = p3HandlingFill[1];
        StatsFillHolder[2, 2, 2] = p3HandlingFill[2];
        StatsFillHolder[2, 2, 3] = p3HandlingFill[3];
        StatsFillHolder[2, 2, 4] = p3HandlingFill[4];
        #endregion

        #region Player 4 Stats
        StatsFillHolder[3, 0, 0] = p4SpeedFill[0];
        StatsFillHolder[3, 0, 1] = p4SpeedFill[1];
        StatsFillHolder[3, 0, 2] = p4SpeedFill[2];
        StatsFillHolder[3, 0, 3] = p4SpeedFill[3];
        StatsFillHolder[3, 0, 4] = p4SpeedFill[4];

        StatsFillHolder[3, 1, 0] = p4WeightFill[0];
        StatsFillHolder[3, 1, 1] = p4WeightFill[1];
        StatsFillHolder[3, 1, 2] = p4WeightFill[2];
        StatsFillHolder[3, 1, 3] = p4WeightFill[3];
        StatsFillHolder[3, 1, 4] = p4WeightFill[4];

        StatsFillHolder[3, 2, 0] = p4HandlingFill[0];
        StatsFillHolder[3, 2, 1] = p4HandlingFill[1];
        StatsFillHolder[3, 2, 2] = p4HandlingFill[2];
        StatsFillHolder[3, 2, 3] = p4HandlingFill[3];
        StatsFillHolder[3, 2, 4] = p4HandlingFill[4];
        #endregion
    }

    IEnumerator CheckAIColours()
    {
        bool allAIChecked = false;
        aiColoursChecked = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            if (!playerJoined[i])
                StartCoroutine(FinalColourCheck(i, true));
            else
                aiColoursChecked[i] = true;

            yield return aiColoursChecked[i];
        }
        if (aiColoursChecked[0] && aiColoursChecked[1] && aiColoursChecked[2] && aiColoursChecked[3])
            allAIChecked = true;
        yield return allAIChecked;
        SetPlayerPrefs();
        Debug.Log("Checked AI Colours");
    }

    void SetPlayerPrefs()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerPrefs.SetInt("Player" + i + "Colour", playerColour[i]);
            PlayerPrefs.SetInt("Player" + i + "Hovercraft", selectedHovercraft[i]);
            if (!playerJoined[i])
            {
                PlayerPrefs.SetInt("Player" + i + "IsAI", 1);
            }
            else
                PlayerPrefs.SetInt("Player" + i + "IsAI", 0);
        }
        PlayerPrefs.SetInt("KeyboardInSlot", keyboardInSlot);
        PlayerPrefs.SetInt("CurrentGameMode", currentGameModeChoice);
        PlayerPrefs.SetInt("NoOfLives", currentNoOfLives);
        //Debug.Log(currentScreenRatio);
        //PlayerPrefs.SetInt("ScreenResolution", (int)currentResolution);
    }

    IEnumerator LoadLevel(int sceneToLoad)
    {
        yield return null;
        StartCoroutine(CheckAIColours());
        string levelName = "Level" + (sceneToLoad + 1);

        AsyncOperation result = SceneManager.LoadSceneAsync(levelName);
        result.allowSceneActivation = false;
        float time = 0;



        while (!result.isDone)
        {
            float progress = Mathf.Clamp01((result.progress / 0.9f) * .62f);
            //Debug.Log("Loading Progress: " + (progress * 100) + "%");
            loadingBar.fillAmount = progress;


            if (result.progress == 0.9f)
            {
                //Debug.Log("Done Loading");
                time += Time.deltaTime;
                progress += Mathf.Clamp01(time / 10f);
                loadingBar.fillAmount = progress;
                if (progress >= 1.0f)
                {
                    result.allowSceneActivation = true;
                }

            }
            yield return null;
        }
    }

    void GetCurrentRatio()
    {
        currentScreenRatio = PlayerPrefs.GetInt("ScreenRatio");
    }

    IEnumerator CheckKeyboardSlot()
    {
        bool checkedKeyboardSlot = false;
        if (!playerJoined[0])
        {
            keyboardInSlot = 0;
            StartCoroutine(PlayerWait(keyboardInSlot, 0.1f));
            PlayerJoin(keyboardInSlot);
            checkedKeyboardSlot = true;
        }
        else if (playerJoined[0] && !playerJoined[1])
        {
            keyboardInSlot = 1;
            StartCoroutine(PlayerWait(keyboardInSlot, 0.1f));
            PlayerJoin(keyboardInSlot);
            checkedKeyboardSlot = true;
        }
        else if (playerJoined[0] && playerJoined[1] && !playerJoined[2])
        {
            keyboardInSlot = 2;
            StartCoroutine(PlayerWait(keyboardInSlot, 0.1f));
            PlayerJoin(keyboardInSlot);
            checkedKeyboardSlot = true;
        }
        else if (playerJoined[0] && playerJoined[1] && playerJoined[2] && !playerJoined[3])
        {
            keyboardInSlot = 3;
            StartCoroutine(PlayerWait(keyboardInSlot, 0.1f));
            PlayerJoin(keyboardInSlot);
            checkedKeyboardSlot = true;
        }
        else
        {
            keyboardInSlot = 4;
            StartCoroutine(PlayerWait(keyboardInSlot, 0.1f));
            PlayerJoin(keyboardInSlot);
            Debug.Log("All Slots Filled");
            checkedKeyboardSlot = true;
        }
        yield return checkedKeyboardSlot;
        StartCoroutine(ShowKeyboardButtons(keyboardInSlot));
    }

    IEnumerator ShowKeyboardButtons(int keyboardPlayer)
    {
        bool setButtons = false;
        for (int i = 0; i < 4; i++)
        {
            if (i == keyboardPlayer)
            {
                leftShoulders[i].GetComponent<Image>().sprite = buttonSprites[2];
                rightShoulders[i].GetComponent<Image>().sprite = buttonSprites[3];
                analogueSticks[i].SetActive(false);
                keyboardIcons[i].SetActive(true);
            }
            else
            {
                leftShoulders[i].GetComponent<Image>().sprite = buttonSprites[0];
                rightShoulders[i].GetComponent<Image>().sprite = buttonSprites[1];
                analogueSticks[i].SetActive(true);
                keyboardIcons[i].SetActive(false);
            }
            if (i >= 3)
                setButtons = true;
        }
        yield return setButtons;
    }

    //Public Button to show keyboard controls
    public void ShowKeyboardButton()
    {
        ShowKeyboardControls(true);
    }

    public void ShowControllerButton()
    {
        ShowKeyboardControls(false);
    }

    void ShowKeyboardControls(bool isKeyboard)
    {
        Debug.Log(isKeyboard);
        StartCoroutine(Wait(0.1f));

        Color highlighted = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color unhighlighted = new Color(1.0f, 1.0f, 1.0f, 0.6f);

        if (isKeyboard)
        {
            //for (int i = 0; i < 3; i++)
            //{
            keyboardControlsHolder.SetActive(isKeyboard);
            controllerControlsHolder.SetActive(!isKeyboard);
            keyboardControlIcons.GetComponent<Image>().color = highlighted;
            controllerControlIcons.GetComponent<Image>().color = unhighlighted;
            //}
        }
        else
        {
            //for (int i = 0; i < 3; i++)
            //{
            keyboardControlsHolder.SetActive(isKeyboard);
            controllerControlsHolder.SetActive(!isKeyboard);
            keyboardControlIcons.GetComponent<Image>().color = unhighlighted;
            controllerControlIcons.GetComponent<Image>().color = highlighted;
            //}
        }
    }
}
