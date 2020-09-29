using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInfo
{
    [Header("Player Data:")]
    [SerializeField] private string playerUsername;

    [SerializeField] private string playerEmail;
    [SerializeField] private string playerPassword;
    [SerializeField] private string playerTeamName;
    [SerializeField] private Image playerCurrentBadge;
    [SerializeField] private Image playerAvatar;
    [SerializeField] private Image playerCountry;

    [Header("Player Stats:")]
    [SerializeField] private int playerCurrentLevel;

    [SerializeField] private int playerCoins;
    [SerializeField] private int playerRubbish;

    [Header("Rubbish Location Data:")]
    [SerializeField] private string rubbishPlace;

    [SerializeField] private int rubbishInPlace;
    [SerializeField] private string rubbishDistrict;
    [SerializeField] private int rubbishInDistrict;
    [SerializeField] private string rubbishRegion;
    [SerializeField] private int rubbishInRegion;
    [SerializeField] private string rubbishCountry;
    [SerializeField] private int rubbishInCountry;

    public string PlayerUsername
    {
        get { return playerUsername; }
        set { playerUsername = value; }
    }

    public string PlayerEmail
    {
        get { return playerEmail; }
        set { playerEmail = value; }
    }

    public string PlayerPassword
    {
        get { return playerPassword; }
        set { playerPassword = value; }
    }

    public string PlayerTeamName
    {
        get { return playerTeamName; }
        set { playerTeamName = value; }
    }

    public string RubbishPlace
    {
        get { return rubbishPlace; }
        set { rubbishPlace = value; }
    }

    public string RubbishDistrict
    {
        get { return rubbishDistrict; }
        set { rubbishDistrict = value; }
    }

    public string RubbishRegion
    {
        get { return rubbishRegion; }
        set { rubbishRegion = value; }
    }

    public string RubbishCountry
    {
        get { return rubbishCountry; }
        set { rubbishCountry = value; }
    }

    public int PlayerCurrentLevel
    {
        get { return playerCurrentLevel; }
        set { playerCurrentLevel = value; }
    }

    public int PlayerCoins
    {
        get { return playerCoins; }
        set { playerCoins = value; }
    }

    public int PlayerRubbish
    {
        get { return playerRubbish; }
        set { playerRubbish = value; }
    }

    public int RubbishInPlace
    {
        get { return rubbishInPlace; }
        set { rubbishInPlace = value; }
    }

    public int RubbishInDistrict
    {
        get { return rubbishInDistrict; }
        set { rubbishInDistrict = value; }
    }

    public int RubbishInRegion
    {
        get { return rubbishInRegion; }
        set { rubbishInRegion = value; }
    }

    public int RubbishInCountry
    {
        get { return rubbishInCountry; }
        set { rubbishInCountry = value; }
    }
}