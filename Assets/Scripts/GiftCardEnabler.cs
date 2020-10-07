using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver))]
public class GiftCardEnabler : MonoBehaviour
{
    public TextMeshProUGUI coinsAvailable;
    public List<TextMeshProUGUI> cards;
    private PlayerDataSaver playerDataSaver;
    private int coins = 0;

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        EnableCards();
    }

    public void EnableCards()
    {
        coins = playerDataSaver.GetCoinsAvailable();
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
        int newCoins = playerDataSaver.GetCoinsAvailable() - coinsUsed;
        playerDataSaver.SetCoinsAvailable(newCoins);
        EnableCards();
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerCoins",
            FunctionParameter = new
            {
                cloudCoinsAvailable = newCoins
            },
            GeneratePlayStreamEvent = true,
        },
        result => Debug.Log("Sent " + playerDataSaver.GetCoinsAvailable() + " coins to cloudscript"),
        error => Debug.Log(error.GenerateErrorReport()));
        //playfabManager.CoinsDisplay();
    }
}