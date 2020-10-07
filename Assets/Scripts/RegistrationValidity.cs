using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationValidity : MonoBehaviour
{
    public TMP_InputField usernameInputField, passwordInputField, repeatPasswordInputField, emailInputField;
    public Color32 colorDefault;

    private void Start()
    {
        colorDefault = new Color32(90, 216, 98, 255);
        usernameInputField = usernameInputField.GetComponent<TMP_InputField>();
        passwordInputField = passwordInputField.GetComponent<TMP_InputField>();
        repeatPasswordInputField = repeatPasswordInputField.GetComponent<TMP_InputField>();
        emailInputField = emailInputField.GetComponent<TMP_InputField>();
        usernameInputField.onValueChanged.AddListener(CheckLength);
        passwordInputField.onValueChanged.AddListener(CheckLength);
        repeatPasswordInputField.onValueChanged.AddListener(CheckPasswordSimilarity);
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
        if (usernameInputField.text.Length < 5)
        {
            usernameInputField.image.color = Color.red;
        }
        else
        {
            usernameInputField.image.color = colorDefault;
        }
    }

    private void CheckPasswordSimilarity(string str)
    {
        if (passwordInputField.text.Length >= 8 && passwordInputField.text.Equals(repeatPasswordInputField.text))
        {
            passwordInputField.image.color = colorDefault;
            repeatPasswordInputField.image.color = colorDefault;
        }
        else
        {
            passwordInputField.image.color = Color.red;
            repeatPasswordInputField.image.color = Color.red;
        }
    }

    public bool IsValid()
    {
        if (passwordInputField.image.color == colorDefault
            && usernameInputField.image.color == colorDefault
            && emailInputField.image.color == colorDefault)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}