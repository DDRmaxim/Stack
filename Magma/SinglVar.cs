using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglVar
{
    public static long scores = 0;
    public static int life = 0;
    public static float gof = 0;
    public static int gofMana = PlayerPrefs.GetInt("gof", 3);
    public static int coin = PlayerPrefs.GetInt("coin", 0);
    public static int level = 1;
    public static string uid = "-1";
    public static string uname = PlayerPrefs.GetString("uname", "");
    public static bool live = false;
    public static int sound = PlayerPrefs.GetInt("sound", 1);
    public static int rain = PlayerPrefs.GetInt("rain", 1);
    public static int music = PlayerPrefs.GetInt("music", 1);
}
