using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationValidity : MonoBehaviour
{
    private Button registerButton;
    public TMP_InputField usernameInputField, passwordInputField;
    public Color32 colorDefault;

    void Start()
    {
        colorDefault = new Color32(90, 216, 98, 255);
        registerButton = GetComponent<Button>();
        usernameInputField = usernameInputField.GetComponent<TMP_InputField>();
        passwordInputField = passwordInputField.GetComponent<TMP_InputField>();
        usernameInputField.onEndEdit.AddListener(CheckLength);
        passwordInputField.onEndEdit.AddListener(CheckLength);
    }

    private void CheckLength(string str)
    {
        if(usernameInputField.text.Length < 8)
        {
            usernameInputField.image.color = Color.red;
        }
        else
        {
            usernameInputField.image.color = colorDefault;
        }
        if (passwordInputField.text.Length < 8)
        {
            passwordInputField.image.color = Color.red;
        }
        else
        {
            passwordInputField.image.color = colorDefault;
        }
    }

    public bool IsValid()
    {
        if(passwordInputField.image.color == colorDefault && usernameInputField.image.color == colorDefault)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
