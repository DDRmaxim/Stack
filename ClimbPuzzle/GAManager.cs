using UnityEngine;
using GameAnalyticsSDK;

public class GAManager : MonoBehaviour
{
    public static GAManager manager;

    void Awake()
    {
        manager = this;

        GameAnalytics.Initialize();

        DontDestroyOnLoad(this);
    }

    public void StartLevel(string lvl)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, lvl);

        Debug.Log($"GA Event - Start: {lvl}");
    }

    public void LevelComplete(string lvl)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, lvl);

        Debug.Log($"GA Event - Complete: {lvl}");
    }
}
