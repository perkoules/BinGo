using UnityEngine;

public class PlayerPrefManager
{
    private const string REGISTER_FROM_GUEST = "RegisterFromGuest";
    private const string EMAIL_GIVEN = "EmailGiven";
    private const string PASSWORD_GIVEN = "PasswordGiven";
    private const string USERNAME_GIVEN = "UsernameGiven";
    public const string COUNTRY_GIVEN = "CountryGiven";
    public const string AVATAR_GIVEN = "AvatarGiven";
    public const string TEAMNAME_GIVEN = "TeamnameGiven";
    public const string TASK_BADGES = "TaskBadges";

    public void SetGuestPlayerRegistered(string reg)
    {
        PlayerPrefs.SetString(REGISTER_FROM_GUEST, reg);
    }

    public string GetGuestPlayerRegistered()
    {
        return PlayerPrefs.GetString(REGISTER_FROM_GUEST);
    }

    public void SetUserEmail(string usrEmail)
    {
        PlayerPrefs.SetString(EMAIL_GIVEN, usrEmail);
    }

    public string GetUserEmail()
    {
        return PlayerPrefs.GetString(EMAIL_GIVEN);
    }

    public void SetUserPassword(string usrPassword)
    {
        PlayerPrefs.SetString(PASSWORD_GIVEN, usrPassword);
    }

    public string GetUserPassword()
    {
        return PlayerPrefs.GetString(PASSWORD_GIVEN);
    }

    public void SetUserName(string usrnm)
    {
        PlayerPrefs.SetString(USERNAME_GIVEN, usrnm);
    }

    public string GetUserName()
    {
        return PlayerPrefs.GetString(USERNAME_GIVEN);
    }

    public void SetCountry(string coun)
    {
        PlayerPrefs.SetString(COUNTRY_GIVEN, coun);
    }

    public string GetCountry()
    {
        return PlayerPrefs.GetString(COUNTRY_GIVEN);
    }


    public void SetAvatar(string avtr)
    {
        PlayerPrefs.SetString(AVATAR_GIVEN, avtr);
    }

    public string GetAvatar()
    {
        return PlayerPrefs.GetString(AVATAR_GIVEN);
    }

    public void SetTeamname(string tmnm)
    {
        PlayerPrefs.SetString(TEAMNAME_GIVEN, tmnm);
    }

    public string GetTeamname()
    {
        return PlayerPrefs.GetString(TEAMNAME_GIVEN);
    }
}
