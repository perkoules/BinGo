using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationValidity : MonoBehaviour
{
    private Button registerButton;
    public TMP_InputField usernameInputField, passwordInputField, emailInputField;
    public Color32 colorDefault;

    void Start()
    {
        colorDefault = new Color32(90, 216, 98, 255);
        registerButton = GetComponent<Button>();
        usernameInputField = usernameInputField.GetComponent<TMP_InputField>();
        passwordInputField = passwordInputField.GetComponent<TMP_InputField>();
        emailInputField = emailInputField.GetComponent<TMP_InputField>();
        usernameInputField.onValueChanged.AddListener(CheckLength);
        passwordInputField.onValueChanged.AddListener(CheckLength);
        emailInputField.onValueChanged.AddListener(CheckEmail);
    }
    private void CheckEmail(string email)
    {
        /*if (email.EndsWith("@gmail.com") || email.EndsWith("@outlook.com") || email.EndsWith("@yahoo.com"))
        {
            emailInputField.image.color = colorDefault;
        }
        else
        {
            emailInputField.image.color = Color.red;
        }*/
    }
    private void CheckLength(string str)
    {
        if(usernameInputField.text.Length < 5)
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
        if(passwordInputField.image.color == colorDefault && usernameInputField.image.color == colorDefault && emailInputField.image.color == colorDefault)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
