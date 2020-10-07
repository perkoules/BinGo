using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerDataSaver))]
public class InitializeText : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    private TextMeshProUGUI myText;

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        myText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Displayer(gameObject.name);
    }

    public void Displayer(string which)
    {
        switch (which)
        {
            case "Username":
                myText.text = playerDataSaver.GetUsername().Replace(playerDataSaver.GetUsername().First(), char.ToUpper(playerDataSaver.GetUsername().First()));
                break;

            case "CoinsCollectedNumber":
                myText.text = playerDataSaver.GetCoinsAvailable().ToString();
                break;

            case "LvlNumber":
                myText.text = playerDataSaver.GetProgressLevel().ToString();
                break;

            case "RubbishCollectedText":
                int allRubbish = playerDataSaver.GetWasteCollected() + playerDataSaver.GetRecycleCollected();
                myText.text = allRubbish.ToString();
                break;

            case "VoucherText":
                float voucher = (playerDataSaver.GetCoinsAvailable() / 100.0f);
                myText.text = voucher.ToString(("F2")) + " £";
                break;

            case "TeamNameText":
                string teamname = playerDataSaver.GetTeamname();
                myText.text = teamname;
                if (teamname == "-")
                {
                    //playerStats.teamnameSetterButton.gameObject.SetActive(true);
                }
                else
                {
                    //playerStats.teamnameSetterButton.gameObject.SetActive(false);
                }
                break;

            default:
                break;
        }
    }
}