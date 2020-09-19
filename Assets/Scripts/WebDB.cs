using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class WebDB : MonoBehaviour
{
    private static string getUsersUrl = "http://localhost/UnityDatabases/GetUsers.php";
    private static string getLoginUrl = "http://localhost/UnityDatabases/GetLogin.php";
    private static string getRegisterUrl = "http://localhost/UnityDatabases/Register.php";

    public TMP_InputField userNameInput, passwordInput;
    public TMP_InputField userNameRegisterInput, passwordRegisterInput, emailRegisterInput;
    public TMP_Dropdown countryRegisterInput, avatarRegisterInput;
    public GameObject warningLogin, warningRegister;

    public RegistrationValidity registrationValidity;

    IEnumerator GetUsers(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(getUsersUrl))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }

    #region Login
    public void CheckCredentials()
    {
        StartCoroutine(Login(userNameInput.text, passwordInput.text));
    }
    IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post(getLoginUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string dbResult = www.downloadHandler.text;
                Debug.Log(dbResult);
                if (dbResult.Contains("success"))
                {
                    CorrectCredentials();
                }
                else if(dbResult.Contains("Wrong"))
                {
                    WrongCredentials();
                }
            }
        }
    }
    public void CorrectCredentials()
    {
        if (warningLogin.activeSelf)
        {
            warningLogin.SetActive(false);
            SceneManager.LoadScene(1);
        }
    }
    public void WrongCredentials()
    {
        warningLogin.SetActive(true);
    }

    #endregion

    #region Registration

    public void RegisterCredentials()
    {
        if (registrationValidity.IsValid())
        {
            StartCoroutine(Register(userNameRegisterInput.text, passwordRegisterInput.text, emailRegisterInput.text,
                countryRegisterInput.captionText.text, avatarRegisterInput.captionText.text));
        }
        else
        {
            warningRegister.SetActive(true);
            TextMeshProUGUI txt = warningRegister.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = "Check your credentials again";
        }
    }

    IEnumerator Register(string username, string password, string email, string country, string avatar)
    {
        WWWForm form = new WWWForm();
        form.AddField("registerUser", username);
        form.AddField("registerPass", password);
        form.AddField("registerEmail", email);
        form.AddField("registerCountry", country);
        form.AddField("registerAvatar", avatar);
        form.AddField("registerTeam", "no team");
        form.AddField("registerLevel", 1);
        form.AddField("registerRubbish", 0);
        form.AddField("registerCoins", 0);

        using (UnityWebRequest www = UnityWebRequest.Post(getRegisterUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string dbResult = www.downloadHandler.text;
                Debug.Log(dbResult);
                if (dbResult.Contains("taken"))
                {
                    WrongRegistration();
                }
                else if (dbResult.Contains("Creating"))
                {
                    CorrectRegistration();
                }
            }
        }
    }
    public void CorrectRegistration()
    {
        if (warningRegister.activeSelf)
        {
            warningRegister.SetActive(false);
            SceneManager.LoadScene(1);
        }
    }
    public void WrongRegistration()
    {
        warningRegister.SetActive(true);
    }

    #endregion

    

}
