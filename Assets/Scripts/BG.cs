﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BG : MonoBehaviour
{
    [SerializeField] private Sprite[] images;

    public event Action OnBGChanged;
    public int Level { get; private set; }

    private void Start()
    {
        GetLevelOnStartUp();
    }

    void GetLevelOnStartUp()
    {
        Level = GetLowestLevel();
        GetComponent<Image>().sprite = images[Mathf.Clamp(Level - 1, 0, 4)];
        OnBGChanged?.Invoke();
    }

    public void LevelUp()
    {
        var originalLevel = Level;
        Level = GetLowestLevel();
        GetComponent<Image>().sprite = images[Mathf.Clamp(Level - 1, 0, 4)];
        if (Level != originalLevel)
        {
            OnBGChanged?.Invoke();
            var num = Random.Range(0, 1);
            FindObjectOfType<SoundManager>().officeLevelUpSounds.PlayRandomSound();
        }
    }

    public int GetLowestLevel()
    {
        var upgradables = FindObjectsOfType<OfficeInteractable>().ToList();

        int lowestLvlItem = 5;

        foreach (var upgradable in upgradables)
        {
            if (upgradable.Level <= lowestLvlItem)
            {
                lowestLvlItem = Mathf.Min(upgradable.Level);
            }
        }
        
        return lowestLvlItem;
    }
}
