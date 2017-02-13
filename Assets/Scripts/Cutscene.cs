using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Cutscene : MonoBehaviour
{ 
    public List<SceneTransition> transitions = new List<SceneTransition>();
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void Play()
    {        
        foreach (SceneTransition item in transitions)
        {
            item.Play();
        }
    }
}