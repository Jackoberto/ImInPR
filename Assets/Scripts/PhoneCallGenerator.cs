﻿using System.Collections.Generic;
using UnityEngine;

public class PhoneCallGenerator : MonoBehaviour
{
    public int phoneCallRarity;
    public float repeatTime = 20f;
    public List<PhoneEvent> phoneEvents;
    private bool _isSpawned;
    
    void Start()
    {
        InvokeRepeating(nameof(IsCalling), repeatTime, repeatTime);
    }

    private void IsCalling()
    {
        
        if (NumberCallGenerator() == phoneCallRarity - 1 && !_isSpawned)
        { 
            var instance = Instantiate(FindObjectOfType<GameManager>().PhoneEventPrefab, FindObjectOfType<GameManager>().transform);
            var confirmationPanel = instance.GetComponent<PhoneEventBehaviour>();
            confirmationPanel.SetUp(GeneratePhoneEvent());
            confirmationPanel.OnDestroyed += UnsubscribeFromConfirm;
            _isSpawned = !_isSpawned;
            instance.SetActive(false);
            GetComponent<Phone>().IncomingCall(instance);
            FindObjectOfType<SoundManager>().PlayGameSound("Ringtone");
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
        } while (FindObjectOfType<BG>().Level < phoneEvents[index].level);
        return phoneEvents[index];
    }
    
    void UnsubscribeFromConfirm(ConfirmationPanel confirmationPanel)
    {
        confirmationPanel.OnDestroyed -= UnsubscribeFromConfirm;
        _isSpawned = !_isSpawned;
    }
}
