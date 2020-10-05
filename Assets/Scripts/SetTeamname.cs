using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetTeamname : MonoBehaviour
{
    public TMP_InputField nameToSet;
    public TextMeshProUGUI teamnameText;
    public Button setWindowButton;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SubmitTeamName);
    }

    private void SubmitTeamName()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Permission = UserDataPermission.Public,
            Data = new Dictionary<string, string>() {            
            {"TeamName", nameToSet.text} }
        },
        result => Debug.Log(nameToSet.text +  " added"),
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
        teamnameText.text = nameToSet.text;
        setWindowButton.gameObject.SetActive(false);
    }
}
