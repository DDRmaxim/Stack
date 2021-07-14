using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FBManager : MonoBehaviour
{
    public static FBManager manager;
    void Awake()
    {
        manager = this;

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }

        DontDestroyOnLoad(this);
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void StartLevel(string lvl)
    {
        var tutParams = new Dictionary<string, object>();
        tutParams["Level"] = lvl;

        FB.LogAppEvent(
            "Start Level",
            parameters: tutParams
        );

        Debug.Log($"FB Log Event - Start: {lvl}");
    }

    public void LevelComplete(string lvl)
    {
        var tutParams = new Dictionary<string, object>();
        tutParams["Level"] = lvl;

        FB.LogAppEvent(
            "Level Completed",
            parameters: tutParams
        );

        Debug.Log($"FB Log Event - Complete: {lvl}");
    }
}
