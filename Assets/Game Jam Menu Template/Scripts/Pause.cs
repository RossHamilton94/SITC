using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Pause : MonoBehaviour {

    public bool isPaused;                               //Boolean to check if the game is paused or not
    public GameObject pausePanel;

    //Awake is called before Start()
    void Awake()
	{ 
	}

	// Update is called once per frame
	void Update () {
        bool startPressed = false;
        if (Input.GetButtonDown("LStartP1") || Input.GetButtonDown("LStartP2") || Input.GetButtonDown("LStartP3") || Input.GetButtonDown("LStartP4")
            || Input.GetButtonDown("MStartP1") || Input.GetButtonDown("MStartP2") || Input.GetButtonDown("MStartP3") || Input.GetButtonDown("MStartP4"))
            startPressed = true;

        //Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
        if (startPressed && !isPaused)
        {
            //Call the DoPause function to pause the game
            DoPause();
        }
        //If the button is pressed and the game is paused and not in main menu
        else if (startPressed && isPaused)
        {
            //Call the UnPause function to unpause the game
            UnPause();
        }

        ////Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
        //if (Input.GetButtonDown("Cancel") && !isPaused)
        //{
        //    //Call the DoPause function to pause the game
        //    DoPause();
        //}
        ////If the button is pressed and the game is paused and not in main menu
        //else if (Input.GetButtonDown("Cancel") && isPaused)
        //{
        //    //Call the UnPause function to unpause the game
        //    UnPause();
        //}

    }


	public void DoPause()
	{
		//Set isPaused to true
		isPaused = true;

        //Blur the screen
        Camera.main.gameObject.GetComponent<BlurOptimized>().enabled = true;

		//Set time.timescale to 0, this will cause animations and physics to stop updating
		Time.timeScale = 0;
         
        pausePanel.SetActive(true);
	}


	public void UnPause()
	{
		//Set isPaused to false
		isPaused = false;

        //Unblur the screen
        Camera.main.gameObject.GetComponent<BlurOptimized>().enabled = false;

        //Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
        Time.timeScale = 1;
         
        pausePanel.SetActive(false);
    }


}
