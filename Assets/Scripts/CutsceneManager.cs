﻿using UnityEngine;
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

    internal void Play(int index)
    {
        Debug.Log("Playing cutscene at index: " + index);
        LetterboxBegin();
        cutscenes[index].Play();
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
