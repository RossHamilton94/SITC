using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Extensions
{
    public static Transform[] GetAllChildren(this Transform context)
    {
        Transform[] children = new Transform[context.childCount];
        for (int i = 0; i < context.childCount; i++)
        {
            children[i] = context.GetChild(i);
        }
        return children;
    }
}