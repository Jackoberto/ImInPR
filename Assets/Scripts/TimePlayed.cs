﻿using UnityEngine;
using System;

public static class TimePlayed
{
    private static DateTime lastUpdatedTime;

    public static void Initialize(Cash cash)
    {
        lastUpdatedTime = DateTime.Now;
        var temp = PlayerPrefs.GetString("GameDestroyTime");
        var totalPlayTime = PlayerPrefs.GetString("TotalPlayTime");
        Debug.Log(FormatPlayTime(DateTime.Parse(totalPlayTime)));
        if (temp != "")
        {
            DateTime destroyedTime = DateTime.Parse(temp);
            Debug.Log(lastUpdatedTime);
            Debug.Log(destroyedTime);
            var difference = (lastUpdatedTime - destroyedTime).TotalMinutes;
            Debug.Log(difference);
            cash.TryAdd(Mathf.RoundToInt((float)(BusinessCard.GetCashPerMin() * difference)));
        }
    }

    public static string GetTimePlayed()
    {
        return FormatPlayTime(UpdateTimePlayed());
    }
    
    public static DateTime UpdateTimePlayed()
    {
        var currentTime = DateTime.Now;
        var totalPlayTime = DateTime.Parse(PlayerPrefs.GetString("TotalPlayTime"));
        var difference = currentTime - lastUpdatedTime;
        var newTime = totalPlayTime.Add(difference);
        SaveTimePlayed();
        lastUpdatedTime = currentTime;
        return newTime;
    }
    
    static string FormatPlayTime(DateTime playTime)
    {
        var result = "";
        var days = playTime.Day - 1;
        var hours = playTime.Hour;
        var minutes = playTime.Minute;
        if (days != 0)
        {
            result += $"{days}";
            if (days > 1)
                result += " Days ";
            else 
                result += " Day ";
        }
        if (hours != 0)
        {
            result += $"{hours}";
            if (hours > 1)
                result += " Hours ";
            else
                result += " Hour ";
        }
        if (minutes != 0)
        {
            result += $"{minutes}";
            if (minutes > 1)
                result += " Minutes ";
            else
                result += " Minute ";
        }
        return result;
    }

    public static void SaveTimePlayed()
    {
        PlayerPrefs.SetString("GameDestroyTime", DateTime.Now.ToString());
        var startTime = DateTime.Parse(PlayerPrefs.GetString("TotalPlayTime", DateTime.MinValue.ToString())) + (DateTime.Now - lastUpdatedTime);
        PlayerPrefs.SetString("TotalPlayTime", startTime.ToString());
    }
}