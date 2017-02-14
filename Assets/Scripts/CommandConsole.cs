using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CommandConsole : MonoBehaviour
{
    public static CommandConsole instance = null;

    public GameObject commandPanel;
    public MenuController mc;

    public InputField commandInput;
    public Text previousCommands;
    public string[] commandHistory = new string[10];
    bool active = false;

    void Awake()
    {
        #region Singleton Check

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a CommandConsole.
        }

        #endregion
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote) && active)
        {
            active = false;
            commandPanel.SetActive(active);
        }
        else if (Input.GetKeyDown(KeyCode.BackQuote) && !active)
        {
            active = true;
            commandPanel.SetActive(active);
            commandInput.text = "";
            EventSystem.current.SetSelectedGameObject(commandPanel);
            EventSystem.current.SetSelectedGameObject(commandInput.gameObject);
        }
        if(mc == null && SceneManager.GetActiveScene().name == "Menu")
        {
            mc = GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>();
        }

        if (Input.GetKeyDown(KeyCode.F1) && active)
        {
            commandInput.text = "start-2-0-0-1";
        }
        if (Input.GetKeyDown(KeyCode.F2) && active)
        {
            commandInput.text = "start-2-1-0-1";
        }
    }

    public void CheckCommand()
    {
        string commandToCheck = commandInput.text.ToLower();
        if (!commandToCheck.Contains("`"))
            previousCommands.text = previousCommands.text + "\n";

        if(commandToCheck == "" || commandToCheck == null)
        {
            previousCommands.text = previousCommands.text + "Please enter a command...";
        }
        else if (commandToCheck.Contains("`"))
        {

        }
        else if(commandToCheck.Contains("time"))
        {
            if(commandToCheck.Contains("stop") && !commandToCheck.Contains("start"))
            {
                Time.timeScale = 0;
                previousCommands.text = previousCommands.text + commandToCheck;
                UpdateCommandHistory(commandToCheck);
            }
            else if (commandToCheck.Contains("start") && !commandToCheck.Contains("stop"))
            {
                Time.timeScale = 1;
                previousCommands.text = previousCommands.text + commandToCheck;
                UpdateCommandHistory(commandToCheck);
            }
            else
            {
                previousCommands.text = previousCommands.text + "Did not recognise: '" + commandToCheck + "'.";
            }
        }
        else if (commandToCheck.Contains("player"))
        {
            if (commandToCheck.Contains("charge"))
            {
                if (commandToCheck.Contains("0"))
                {
                    UpdateCommandHistory(commandToCheck);
                }
                else if (commandToCheck.Contains("1"))
                {
                    UpdateCommandHistory(commandToCheck);
                }
            }
            
        }
        else if(commandToCheck.Contains("scene"))
        {
            if (commandToCheck.Contains("reset"))
            {
                UpdateCommandHistory(commandToCheck);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (commandToCheck.Contains("load"))
            {
                if(commandToCheck.Contains("1"))
                {
                    UpdateCommandHistory(commandToCheck);
                    SceneManager.LoadScene("Level_1");
                }
                else if(commandToCheck.Contains("menu"))
                {
                    UpdateCommandHistory(commandToCheck);
                    SceneManager.LoadScene("Menu");
                }
            }
        }
        else if(commandToCheck.Contains("start"))
        {
            // start-4-0-1-1
            string tempCommandToCheck = commandToCheck;
            string[] splitCommand = tempCommandToCheck.Split('-');
            int noOfPlayers = 0;
            int bossPlayer = 0;
            int keyboardPlayer = 0;
            int levelToLoad = 0;
            int.TryParse(splitCommand[1], out noOfPlayers);
            int.TryParse(splitCommand[2], out bossPlayer);
            int.TryParse(splitCommand[3], out keyboardPlayer);
            int.TryParse(splitCommand[4], out levelToLoad);
            if(!((noOfPlayers > 4 || noOfPlayers <2) || (bossPlayer > 3 || noOfPlayers < 0) || (keyboardPlayer > 3 || keyboardPlayer < 0) || (levelToLoad > 1 || levelToLoad < 1)))
            {
                UpdateCommandHistory(commandToCheck);
                mc.currentNoOfPlayers = noOfPlayers;
                mc.currentlyTheBoss = bossPlayer;
                mc.currentKeyboardPlayer = keyboardPlayer;
                for (int i = 0; i < noOfPlayers; i++)
                {
                    mc.joinedState[i] = true;
                }
                mc.LoadLevel(levelToLoad);
                previousCommands.text = previousCommands.text + "Loading level, Please wait.";
            }
            else
            {
                previousCommands.text = previousCommands.text + "Check number format in: '" + commandToCheck + "'.";
            }
        }
        else
        {
            previousCommands.text = previousCommands.text + "Did not recognise: '" + commandToCheck + "'.";
        }
    }

    void UpdateCommandHistory(string lastCommand)
    {
        //Check if command used previously
        int previousCommandMatch = 10;
        for (int i = 0; i < commandHistory.Length; i++)
        {
            if(commandHistory[i] == lastCommand)
            {
                previousCommandMatch = i;
            }
        }
        if(previousCommandMatch < 10)
        {
            for (int i = previousCommandMatch; i > 1; i--)
            {
                commandHistory[i] = commandHistory[(i - 1)];
            }
            commandHistory[0] = lastCommand;
        }
        else
        {
            int commandHistorySlotFree = 10;
            for (int i = 0; i < commandHistory.Length; i++)
            {
                if(commandHistorySlotFree == 10 && (commandHistory[i] == "" || commandHistory[i] == null))
                {
                    commandHistorySlotFree = i;
                    //Debug.Log(i);
                }
            }
            if (commandHistorySlotFree < 10)
            {
                for (int i = commandHistorySlotFree; i > 1; i--)
                {
                    commandHistory[i] = commandHistory[(i - 1)];
                }
                commandHistory[0] = lastCommand;
            }
            else
            {
                for (int i = commandHistory.Length; i > 1; i--)
                {
                    commandHistory[i] = commandHistory[(i - 1)];
                }
                commandHistory[0] = lastCommand;
            }
        }
    }
}
