using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerDataSaver), typeof(TextMeshProUGUI))]
public class InitializeText : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    private TextMeshProUGUI myText;

    private void OnEnable()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        myText = GetComponent<TextMeshProUGUI>();
        Displayer(gameObject.name);
    }
    public void ReInitialize()
    {
        Displayer(gameObject.name);
    }
    public void Displayer(string which)
    {
        if (playerDataSaver.GetIsGuest() == 1)
        {
            myText.text = "*";
        }
        else
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

                case "LvlNumberNext":
                    myText.text = (playerDataSaver.GetProgressLevel() + 1).ToString();
                    break;

                case "RubbishCollectedText":
                    myText.text = playerDataSaver.GetRubbishCollected().ToString();
                    break;

                case "VoucherText":
                    float voucher = (playerDataSaver.GetCoinsAvailable() / 100.0f);
                    myText.text = voucher.ToString(("F2")) + " £";
                    break;

                case "TeamNameText":
                    string teamname = playerDataSaver.GetTeamname();
                    myText.text = teamname;
                    break;

                default:
                    break;
            }
        }
    }
}