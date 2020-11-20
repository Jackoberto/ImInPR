﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Cash cash;
    public Player player;
    public BandList bands;
    public static List<OfficeInteractable> officeEquipment;
    public TaskGenerator taskGenerator;
    public GameObject ConfirmationPrefab;
    public GameObject PhoneEventPrefab;
    public GameObject CannotAffordPrefab;
    public GameObject OutcomeMessage;
    public GameObject MaxLevelPrefab;
    public GameObject ImagePopUp;

    [Header("Bands UI")]
    public Transform BandUIContainer;
    public GameObject BandUIElement;
    public GameObject BandSelector;

    public bool NewGame => bandsOwned(bands) == 0;
    private void Awake()
    {
        cash = new Cash();
    }

    private void Start()
    {
        officeEquipment = GetComponentsInChildren<OfficeInteractable>().ToList();

        if (NewGame)
        {
            var instance = Instantiate(FindObjectOfType<GameManager>().BandSelector, FindObjectOfType<GameManager>().transform);
            instance.GetComponent<BandSelectorController>().PopulateList(BandTier.Tier1);
        }
        var gameStartTime = DateTime.Now;
        var temp = PlayerPrefs.GetString("GameDestroyTime");
        if (temp != "")
        {
            DateTime destroyedTime = DateTime.Parse(temp);
            Debug.Log(gameStartTime);
            Debug.Log(destroyedTime);
            var difference = (gameStartTime - destroyedTime).Minutes;
            Debug.Log(difference);
            cash.Add(BusinessCard.GetCashPerMin() * difference);
        }
    }

    public static int bandsOwned(BandList bandsList)
    {
        int counter = 0;
        foreach (var band in bandsList.bands)
        {
            if (band.GetOwned())
                counter++;
        }

        return counter;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString("GameDestroyTime", DateTime.Now.ToString());
    }

    [ContextMenu("Delete Save Game")]
    public void DeleteSaveGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
