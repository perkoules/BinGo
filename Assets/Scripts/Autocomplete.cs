using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Autocomplete : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    public TMP_InputField emailInput, passwordInput;
    public void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
    }
    private void Start()
    {
        PlayerPrefs.DeleteKey(playerDataSaver.GetIsGuest().ToString());
        playerDataSaver.SetIsGuest(0);
        emailInput.text = playerDataSaver.GetEmail();
        passwordInput.text = playerDataSaver.GetPassword();
    }
}
