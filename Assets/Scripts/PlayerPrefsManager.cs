using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private const string register = "RegisterFromGuest";
    public void SetWasRegistered(string reg)
    {
        PlayerPrefs.SetString(register, reg);
    }
    public string GetWasregistered()
    {
        return PlayerPrefs.GetString(register);
    }
}
