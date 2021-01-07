using GoogleARCore.Examples.AugmentedImage;
using Michsky.UI.ModernUIPack;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

[RequireComponent(typeof(PlayerDataSaver))]
public class LootHandler : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;

    public delegate void AdjustValues(int coins);
    public static event AdjustValues OnValuesAdjusted;

    public ModalWindowManager scanMessage, logoMessage;

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
    }
    private void Start()
    {
        BookForceField.OnBookObtained += BookForceField_OnBookObtained;
        //LogoPoints.OnLogoFound += LogoObtained;
        AugmentedImageVisualizer.OnImageFound += LogoObtained;
    }

    private void BookForceField_OnBookObtained()
    {
        scanMessage.OpenWindow();
    }

    private void LogoObtained()
    {
        logoMessage.OpenWindow();
        //LogoPoints.OnLogoFound -= LogoObtained;
        AugmentedImageVisualizer.OnImageFound -= LogoObtained;
    }

    /// <summary>
    /// Triggerde by logoMessage modal window
    /// </summary>
    /// <param name="coinsGained"></param>
    public void SendCoins(int coinsGained)
    {
        int newCoins = playerDataSaver.GetCoinsAvailable() + coinsGained;
        playerDataSaver.SetCoinsAvailable(newCoins);
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
        OnValuesAdjusted(newCoins);
    }
}