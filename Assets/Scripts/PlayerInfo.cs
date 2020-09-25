
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInfo
{
    public string playerUsername;
    public string playerTeamName;
    public Image playerCurrentBadge;
    public Image playerAvatar;
    public Image playerCountry;

    public int playerCurrentLevel;
    public int playerCoins;
    public int playerRubbish;

    [Header("Rubbish Location:")]
    public string rubbishPlace;
    public string rubbishDistrict;
    public string rubbishRegion;
    public string rubbishCountry;
}