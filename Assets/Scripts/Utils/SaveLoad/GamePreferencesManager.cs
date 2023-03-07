using System.Collections.Generic;
using UnityEngine;

public static class GamePreferencesManager
{
    public static void SavePrefs(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public static int LoadPrefs(string key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }
}