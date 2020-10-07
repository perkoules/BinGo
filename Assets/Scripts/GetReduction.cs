using PlayFab;
using PlayFab.ClientModels;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetReduction : MonoBehaviour
{
    public TextMeshProUGUI coinsAvailable;
    public TextMeshProUGUI codeText;
    private PlayfabManager playfabManager;
    private Button voucher;
    public SliderController sliderController;
    private int coins = 0;
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private void OnEnable()
    {
        playfabManager = FindObjectOfType<PlayfabManager>();
        coins = playfabManager.coinsAvailable;
        voucher = GetComponent<Button>();
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
        playfabManager.coinsAvailable -= coinsUsed;
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