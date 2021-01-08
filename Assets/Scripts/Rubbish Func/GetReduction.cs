using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver))]
public class GetReduction : MonoBehaviour
{
    public SliderController sliderController;
    public TextMeshProUGUI coinsAvailable;
    public TextMeshProUGUI codeText;

    private PlayerDataSaver playerDataSaver;
    private Button btn;
    private int coins = 0;
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        btn = GetComponent<Button>();
        coins = playerDataSaver.GetCoinsAvailable();
    }

    private void Start()
    {
        btn.onClick.AddListener(GenerateCode);
    }

    private void GenerateCode()
    {
        if (codeText != null)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                int rand = UnityEngine.Random.Range(0, chars.Length);
                sb.Append(chars[rand]);
            }
            codeText.text = sb.ToString();
            CopyText(codeText);
        }       
        ReductionUsed();
    }

    public void CopyText(TextMeshProUGUI textToCopy)
    {
        TextEditor editor = new TextEditor
        {
            text = textToCopy.text
        };
        editor.SelectAll();
        editor.Copy();
    }
    public delegate void AdjustValues(int coins);
    public static event AdjustValues OnValuesAdjusted;
    public void ReductionUsed()
    {
        int newCoins = playerDataSaver.GetCoinsAvailable() - sliderController.coinsUsed;
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
        result => Debug.Log("Sent " + newCoins + " coins to cloudscript"),
        error => Debug.Log(error.GenerateErrorReport()));
        OnValuesAdjusted(newCoins);
        sliderController.ResetSlider();
    }
    public void MoneySound()
    {
        MusicController.Instance.PlayMoneySound();
    }
}