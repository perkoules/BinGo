using PlayFab;
using PlayFab.ClientModels;
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
    private Button voucher;
    private int coins = 0;
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        voucher = GetComponent<Button>();
        coins = playerDataSaver.GetCoinsAvailable();
    }

    private void Start()
    {
        voucher.onClick.AddListener(GenerateCode);
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
        ReductionUsed(sliderController.coinsUsed);
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

    public void ReductionUsed(int coinsUsed)
    {
        int newCoins = playerDataSaver.GetCoinsAvailable() - coinsUsed;
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
        foreach (var item in FindObjectsOfType<InitializeText>())
        {
            if (item.gameObject.name == "CoinsCollectedNumber")
            {
                item.Displayer("CoinsCollectedNumber");
            }
            if (item.gameObject.name == "VoucherText")
            {
                item.Displayer("VoucherText");
            }
        }
    }
}