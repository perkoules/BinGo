using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInfo
{
    [Header("Player Data:")]
    public string playerUsername;
    public string playerEmail;
    public string playerPassword;
    public Image playerCurrentBadge;
    public Image playerAvatar;
    public Image playerCountry;
    public string playerTeamName;

    [Header("Player Stats:")]
    public int playerCurrentLevel;
    public int playerCoins;
    public int playerRubbish;

    [Header("Rubbish Location Data:")]
    public string rubbishPlace;
    public int rubbishInPlace;
    public string rubbishDistrict;
    public int rubbishInDistrict;
    public string rubbishRegion;
    public int rubbishInRegion;
    public string rubbishCountry;
    public int rubbishInCountry;

}
