using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftCardEnabler : MonoBehaviour
{
    public TextMeshProUGUI coinsAvailable;
    public List<TextMeshProUGUI> cards;
    private PlayfabManager playfabManager;
    private int coins = 0;

    private void OnEnable()
    {
        playfabManager = FindObjectOfType<PlayfabManager>();
        EnableCards();
    }

    public void EnableCards()
    {
        coins = playfabManager.coinsAvailable;
        foreach (var crd in cards)
        {
            if (coins >= Convert.ToInt32(crd.text))
            {
                crd.GetComponentInParent<Button>().interactable = true;
            }
            else if (coins < Convert.ToInt32(crd.text))
            {
                crd.GetComponentInParent<Button>().interactable = false;
            }
        }
    }

    public void CardUsed(int coinsUsed)
    {
        playfabManager.coinsAvailable -= coinsUsed;
        EnableCards();
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerCoins",
            FunctionParameter = new
            {
                cloudCoinsAvailable = playfabManager.coinsAvailable
            },
            GeneratePlayStreamEvent = true,
        },
        result => Debug.Log("Sent " + playfabManager.coinsAvailable + " coins to cloudscript"),
        error => Debug.Log(error.GenerateErrorReport()));
        //playfabManager.CoinsDisplay();
    }
}