using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float shakeIntensity = 0.5f;
	public int shakes = 10;
	public float speed = 1;

	// Use this for initialization
	void Start () { 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    { 
        // if (GUI.Button(new Rect(10, 70, 100, 100), "SHAKE DAT ASS")) {
			// Camera.main.GetComponent<CameraControl>().Shake(shakeIntensity, shakes, speed);
        // }
    }
 
    void OnEnable()
    {
        SlamController.OnSlam += ScreenShake;
    }

    void OnDisable()
    {
        SlamController.OnSlam -= ScreenShake;
    }

	void ScreenShake()
	{
		Camera.main.GetComponent<CameraControl>().Shake(shakeIntensity, shakes, speed);
	} 

}
