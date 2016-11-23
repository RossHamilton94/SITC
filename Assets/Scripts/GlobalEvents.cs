using UnityEngine;
using System.Collections;

public class GlobalEvents : MonoBehaviour
{ 
    public delegate void UIRefreshAction();
    public static event UIRefreshAction OnRefresh;

    public static void TriggerRefresh()
    {
        if (OnRefresh != null)
        {
            OnRefresh();
        }
    }
}