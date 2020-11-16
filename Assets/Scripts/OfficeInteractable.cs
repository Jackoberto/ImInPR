﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class OfficeInteractable : MonoBehaviour, IPointerClickHandler
{
    protected GameManager gm;
    public Sprite[] models; 
    public int Level { get; private set; }
    public int levelUpCost;

    public event Action OnLevelUp;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        Level = PlayerPrefs.GetInt($"{name}_Level", 1);
    }

    public virtual void IncreaseLevel()
    {
        if (gm.cash.Spend(ActualCost()))
        {
            Debug.Log($"Upgraded {name}");
            Level++;
            OnLevelUp?.Invoke();
            FindObjectOfType<BG>().LevelUp();
        }
    }

    public int ActualCost()
    {
        return levelUpCost * Level * 3;
    }

    public override string ToString() => $"{this.name} : Level {Level}, costs {ActualCost()} to upgrade.";

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (gm.cash.CanAfford(ActualCost()))
        {
            var confirmInstance= Instantiate(gm.ConfirmationPrefab, gm.transform);
            var confirmationPanel = confirmInstance.GetComponent<ConfirmationPanel>();
            confirmationPanel.SetUp(this.UpgradeText());
            confirmationPanel.OnConfirm += IncreaseLevel;
            confirmationPanel.OnDestroyed += UnsubscribeFromConfirm;
        }
    }
    
    private string UpgradeText() => $"You Are About To Upgrade {this.name} To \n" +
                                    $"Level {Level + 1} : Costs {ActualCost()}";
    
    private void OnDestroy() => PlayerPrefs.SetInt($"{name}_Level", Level);
    
    void UnsubscribeFromConfirm(ConfirmationPanel confirmationPanel)
    {
        confirmationPanel.OnConfirm -= IncreaseLevel;
        confirmationPanel.OnDestroyed -= UnsubscribeFromConfirm;
        Debug.Log($"{this.name} unsubscribed from {confirmationPanel.GetHashCode()}");
    }
}
