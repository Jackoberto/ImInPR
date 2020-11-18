﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BandUI))]
public class BandBehaviour : MonoBehaviour
{
    public Band bandConfig;
    public int baseCashGenerated;
    public float generateInterval = 12f;
    [Range(0f, 1f)] public float likelyHoodToGenerateTask;
    private BandExperience awareness;
    private BandExperience popularity;
    private float _elapsedTime;

    public int CurrentLevel
    {
        get => PlayerPrefs.GetInt($"{bandConfig.name}_Level", 1);
        set => PlayerPrefs.SetInt($"{bandConfig.name}_Level", value);
    }

    public int RequiredExp => 100 + (5 * (CurrentLevel - 1));

    public void SetUp(Band band)
    {
        bandConfig = band;
        
        awareness = new BandExperience(this, "Awareness", bandConfig.name);
        popularity = new BandExperience(this, "Popularity", bandConfig.name);
        GetComponent<BandUI>().SetUp(band);
        UpdateUI();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime < generateInterval) return;
        FindObjectOfType<GameManager>().cash.Add(baseCashGenerated * CurrentLevel);
        _elapsedTime -= generateInterval;
        if (Random.Range(0f, 1f) < likelyHoodToGenerateTask)
        {
            GenerateTask();
        }

    }

    void GenerateTask()
    {
        var newTask = FindObjectOfType<GameManager>().taskGenerator.SpawnTask(this);
        if (newTask == null) return;
        newTask.OnRewardCollected += OnReward;
        newTask.OnDestroyed += UnsubscribeFromTask;
    }

    public void CheckIfLeveledUp()
    {
        if (awareness.Amount == RequiredExp && popularity.Amount == RequiredExp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        CurrentLevel++;
        awareness.Amount = 0;
        popularity.Amount = 0;
        UpdateUI();
    }


    public void OnReward(RewardAmount[] rewardAmounts)
    {
        foreach (var rewardAmount in rewardAmounts)
        {
            switch (rewardAmount.type.name)
            {
                case "Awareness":
                    awareness.Amount += rewardAmount.amount;
                    break;
                case "Popularity":
                    popularity.Amount += rewardAmount.amount;
                    break;
            }
        }
        UpdateUI();
    }
    
    void UpdateUI()
    {
        GetComponent<BandUI>().UpdateUI(CurrentLevel, CalculateDistanceToNextLevel(popularity), CalculateDistanceToNextLevel(awareness));
    }

    float CalculateDistanceToNextLevel(BandExperience experience)
    {
        return (float)experience.Amount / RequiredExp;
    }

    void UnsubscribeFromTask(BandTask task)
    {
        task.OnDestroyed -= UnsubscribeFromTask;
        task.OnRewardCollected -= OnReward;
    }
}
