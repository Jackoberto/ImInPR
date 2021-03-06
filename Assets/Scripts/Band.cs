﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Band", menuName = "Band/Bands")]
public class Band : ScriptableObject
{
    public string name;
    public string shortName;
    public string genre;
    public BandTier Tier;
    [TextArea] public string bioText;
    public Sprite thumbnail;
    
    public GameSong song;

    private int isOwned
    {
        get => PlayerPrefs.GetInt($"{name}_owned", 0);
        set => PlayerPrefs.SetInt($"{name}_owned", value);
    }

    public void SetOwned(bool owned)
    {
        switch (owned)
        {
            case false:
                isOwned = 0;
                break;
            case true:
                isOwned = 1;
               break;
        }
    }

    public bool GetOwned()
    {
        switch (isOwned)
        {
            case 0: return false;
            case 1: return true;
        }
        return false;
    }
}

public enum BandTier
{
    Tier1 = 1, 
    Tier2 = 5, 
    Tier3 = 10
}