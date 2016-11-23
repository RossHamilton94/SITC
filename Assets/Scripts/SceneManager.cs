using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel(string name)
    {
        if (NiceSceneTransition.instance != null)
        {
            NiceSceneTransition.instance.LoadScene(name);
  
            this.gameObject.SetActive(false);
        } else
        {
            Application.LoadLevel(name);
        }
    }

}
