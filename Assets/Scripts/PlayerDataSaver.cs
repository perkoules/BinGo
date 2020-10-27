﻿using UnityEngine;

public class PlayerDataSaver : MonoBehaviour
{
    private const string IS_GUEST = "isGuest";
    private const string EMAIL_GIVEN = "EmailGiven";
    private const string PASSWORD_GIVEN = "PasswordGiven";
    private const string USERNAME_GIVEN = "UsernameGiven";
    private const string COUNTRY_GIVEN = "CountryGiven";
    private const string AVATAR_GIVEN = "AvatarGiven";
    private const string TEAMNAME_GIVEN = "TeamnameGiven";
    private const string TASK_BADGES = "TaskBadges";
    private const string WASTE_COLLECTED = "WasteCollected";
    private const string RECYCLE_COLLECTED = "RecycleCollected";
    private const string RUBBISH_COLLECTED = "RubbishCollected";
    private const string COINS_AVAILABLE = "CoinsAvailable";
    private const string PROGRESS_LEVEL = "ProgressLevel";
    private const string MONSTERS_KILLED = "MonstersKilled";
    private const string TREE_LOCATION = "MonstersKilled";


    public void SetRubbishCollected(int amount)
    {
        PlayerPrefs.SetInt(RUBBISH_COLLECTED, amount);
    }

    public void SetRubbishCollected()
    {
        int typRub = GetWasteCollected() + GetRecycleCollected();
        PlayerPrefs.SetInt(RUBBISH_COLLECTED, typRub);
    }

    public int GetRubbishCollected()
    {
        return PlayerPrefs.GetInt(RUBBISH_COLLECTED);
    }
    public void SetTreeLocation(string treeLoc)
    {
        PlayerPrefs.SetString(TREE_LOCATION, treeLoc);
    }

    public string GetTreeLocation()
    {
        return PlayerPrefs.GetString(TREE_LOCATION);
    }

    public void SetMonstersKilled(int monkil)
    {
        PlayerPrefs.SetInt(MONSTERS_KILLED, monkil);
    }

    public int GetMonstersKilled()
    {
        return PlayerPrefs.GetInt(MONSTERS_KILLED);
    }

    public void SetWasteCollected(int waste)
    {
        PlayerPrefs.SetInt(WASTE_COLLECTED, waste);
        SetRubbishCollected();
    }

    public int GetWasteCollected()
    {
        return PlayerPrefs.GetInt(WASTE_COLLECTED);
    }

    public void SetRecycleCollected(int recycle)
    {
        PlayerPrefs.SetInt(RECYCLE_COLLECTED, recycle);
        SetRubbishCollected();
    }

    public int GetRecycleCollected()
    {
        return PlayerPrefs.GetInt(RECYCLE_COLLECTED);
    }

    public void SetCoinsAvailable(int coins)
    {
        PlayerPrefs.SetInt(COINS_AVAILABLE, coins);
    }

    public int GetCoinsAvailable()
    {
        return PlayerPrefs.GetInt(COINS_AVAILABLE);
    }

    public void SetProgressLevel(int level)
    {
        PlayerPrefs.SetInt(PROGRESS_LEVEL, level);
    }

    public int GetProgressLevel()
    {
        return PlayerPrefs.GetInt(PROGRESS_LEVEL);
    }

    public void SetIsGuest(int guest)
    {
        PlayerPrefs.SetInt(IS_GUEST, guest);
    }

    public int GetIsGuest()
    {
        return PlayerPrefs.GetInt(IS_GUEST);
    }    

    public void SetEmail(string usrEmail)
    {
        //userEmail = usrEmail;
        PlayerPrefs.SetString(EMAIL_GIVEN, usrEmail);
    }

    public string GetEmail()
    {
        return PlayerPrefs.GetString(EMAIL_GIVEN);
    }

    public void SetPassword(string usrPassword)
    {
        //userPassword = usrPassword;
        PlayerPrefs.SetString(PASSWORD_GIVEN, usrPassword);
    }

    public string GetPassword()
    {
        return PlayerPrefs.GetString(PASSWORD_GIVEN);
    }

    public void SetUsername(string usrnm)
    {
        //username = usrnm;
        PlayerPrefs.SetString(USERNAME_GIVEN, usrnm);
    }

    public string GetUsername()
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

    public void SetTasks(string tsk)
    {
        PlayerPrefs.SetString(TASK_BADGES, tsk);
    }

    public string GetTasks()
    {
        return PlayerPrefs.GetString(TASK_BADGES);
    }
}