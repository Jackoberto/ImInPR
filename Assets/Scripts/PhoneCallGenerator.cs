﻿using System.Collections.Generic;
using UnityEngine;

public class PhoneCallGenerator : MonoBehaviour
{
    //private Animator phoneAnimator;
    public int phoneCallRarity;
    public float repeatTime = 20f;
    public List<PhoneEvent> phoneEvents;
    private bool _isSpawned;

    //TODO Implement a exclamation mark spawner
    void Start()
    {
        InvokeRepeating(nameof(IsCalling),2f, repeatTime);
    }

    private void IsCalling()
    {
        if (NumberCallGenerator() == phoneCallRarity - 1 && !_isSpawned)
        { 
            //Spawn Excalmation Mark 
            var instance = Instantiate(FindObjectOfType<GameManager>().PhoneEventPrefab, FindObjectOfType<GameManager>().transform);
            var confirmationPanel = instance.GetComponent<PhoneEventBehaviour>();
            confirmationPanel.SetUp(GeneratePhoneEvent());
            confirmationPanel.OnDestroyed += UnsubscribeFromConfirm;
            _isSpawned = !_isSpawned;
        }
    }

    private int NumberCallGenerator()
    {
        return Random.Range(0, phoneCallRarity);
    }

    PhoneEvent GeneratePhoneEvent()
    {
        var index = 0;
        do
        {
            index = Random.Range(0, phoneEvents.Count);
        } while (5 < phoneEvents[index].level); // TODO Change 1 To OfficeLevel
        return phoneEvents[index];
    }
    
    void UnsubscribeFromConfirm(ConfirmationPanel confirmationPanel)
    {
        confirmationPanel.OnDestroyed -= UnsubscribeFromConfirm;
        _isSpawned = !_isSpawned;
    }
}