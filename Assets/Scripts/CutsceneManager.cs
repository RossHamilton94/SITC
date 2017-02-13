using UnityEngine;
using System.Collections;
using System;

public class CutsceneManager : MonoBehaviour
{
    public Cutscene[] cutscenes;

    public delegate void LetterboxStart();
    public static event LetterboxStart StartLetterbox;

    public delegate void LetterboxClose();
    public static event LetterboxClose CloseLetterbox;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

<<<<<<< HEAD
    internal void Play(int index)
    {
        Debug.Log("Playing cutscene at index: " + index);
        LetterboxBegin();
        cutscenes[index].Play();
=======
    internal void Play(int sceneIndex)
    { 
        switch (sceneIndex)
        {
            case 0:
                break;
            case 1:
                Debug.Log("Playing cutscene for scene index: " + sceneIndex);
                LetterboxBegin();
                cutscenes[0].Play();
                break;
            default:
                Debug.Log("There was no cutscene to play for scene index: " + sceneIndex);
                break;
        }
>>>>>>> ad8626c0fd88fb5133a7074d495423c33fcf42fe
        Debug.Log(name);
    }

    public static void LetterboxBegin()
    {
        if (StartLetterbox != null)
        {
            StartLetterbox();
        }
    }

    public static void LetterboxEnd()
    {
        if (CloseLetterbox != null)
        {
            CloseLetterbox();
        }
    }
}
