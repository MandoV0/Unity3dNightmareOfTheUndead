using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gets called by Bootstraper and Instantiates all the persistent Managers 
/// </summary>
public class BootstraperObject : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 144;

        // Loading Screen
        var loadingScreen = Resources.Load<LoadingScreen>("@LoadingScreen");
        Instantiate(loadingScreen);
    }
}
