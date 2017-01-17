using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CommandConsole : MonoBehaviour
{
    public static CommandConsole instance = null;

    public GameObject commandPanel;

    public InputField commandInput;
    public Text previousCommands;
    bool active = false;

    void Awake()
    {
        #region Singleton Check

        if (instance == null)
        {
            instance = this;
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
        }
    }

    public void CheckCommand()
    {
        string commandToCheck = commandInput.text.ToLower();
        previousCommands.text = previousCommands.text + "\n";

        if(commandToCheck == "" || commandToCheck == null)
        {
            previousCommands.text = previousCommands.text + "Please enter a command...";
        }
        else if(commandToCheck.Contains("time"))
        {
            if(commandToCheck.Contains("stop"))
            {
                Time.timeScale = 0;
            }
            else if (commandToCheck.Contains("start"))
            {
                Time.timeScale = 1;
            }
        }
        else if (commandToCheck.Contains("player charge"))
        {
            if (commandToCheck.Contains("0"))
            {
                
            }
            else if (commandToCheck.Contains("1"))
            {
                
            }
        }
    }
}
