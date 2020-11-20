using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddFriend : MonoBehaviour
{
    public TextMeshProUGUI username, level;
    public Image countryImage, avatarImage, levelBadge;
    public TMP_InputField friendToFind;
    public Container flagSelection, avatarSelection, levelBadgeSelection;
    public GameObject objectToHide;
    private Button button;
    public FriendListController friendListController;
    public GameObject userNotfound;

    private void OnEnable()
    {
        button = GetComponent<Button>();

        if (friendListController.friendsExist)
        {
            ImageAdjustment();
        }
    }

    public void ImageAdjustment()
    {
        for (int i = 0; i < 2; i++)
        {
            if (this.button == friendListController.addFriendButtons[i] && !string.IsNullOrEmpty(friendListController.friendsName[i].text)
                && this.button.IsInteractable())
            {
                countryImage.sprite = FindImageFlag(friendListController.friendsFlag[i]);
                avatarImage.sprite = FindImageAvatar(friendListController.friendsAvatar[i]);
                levelBadge.sprite = FindImageLevel(friendListController.friendsLevel[i].text);
                button.interactable = false;
            }
        }
    }

    public void SearchForFriend()
    {

        PlayFabClientAPI.AddFriend(
             new AddFriendRequest() { FriendUsername = friendToFind.text },
             result =>
             {
                 Debug.Log(friendToFind.text + " added as a friend");
                 StartCoroutine(SearchDatabase());
                 button.interactable = false;
             },
             error =>
             {
                 Debug.LogError(error.GenerateErrorReport());
                 if (error.ErrorMessage == "User not found")
                 {
                     StartCoroutine(UserNotFound());
                 }
             });
    }

    private IEnumerator UserNotFound()
    {
        yield return new WaitForSeconds(2);
        userNotfound.SetActive(true);
    }

    
    private IEnumerator SearchDatabase()
    {
        string friendsID = "";
        yield return new WaitForSeconds(1);
        PlayFabClientAPI.GetAccountInfo(
            new GetAccountInfoRequest { Username = friendToFind.text },
            result =>
            {
                friendsID = result.AccountInfo.PlayFabId;
            },
            error => Debug.LogError(error.GenerateErrorReport()));
        yield return new WaitForSeconds(1);
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = friendsID },
        result =>
        {
            if (result.Data == null) Debug.Log("No Data");
            else
            {
                username.text = friendToFind.text;
                avatarImage.sprite = FindImageAvatar(result.Data["Avatar"].Value);
                countryImage.sprite = FindImageFlag(result.Data["Country"].Value);
            }
        },
        error => Debug.Log(error.GenerateErrorReport()));
        yield return new WaitForSeconds(1f);
        GetTeammatesLevel(friendsID);
        yield return new WaitForSeconds(1f);
        levelBadge.sprite = FindImageLevel(level.text);
        objectToHide.SetActive(false);
    }


    private void GetTeammatesLevel(string teammateID)
    {
        PlayFabClientAPI.GetFriendLeaderboard(
            new GetFriendLeaderboardRequest() { StatisticName = "ProgressLevel" },
            result =>
            {
                foreach (var item in result.Leaderboard)
                {
                    if (item.PlayFabId == teammateID)
                    {
                        level.text = item.StatValue.ToString();
                    }
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }

    private Sprite FindImageFlag(string imageToSearch)
    {
        foreach (var img in flagSelection.imageContainer)
        {
            if (img.sprite.name == imageToSearch)
            {
                return img.sprite;
            }
        }
        return null;
    }
    private Sprite FindImageAvatar(string imageToSearch)
    {
        foreach (var img in avatarSelection.imageContainer)
        {
            if (img.sprite.name == imageToSearch)
            {
                return img.sprite;
            }
        }
        return null;
    }
    private Sprite FindImageLevel(string imageToSearch)
    {
        foreach (var img in levelBadgeSelection.imageContainer)
        {
            string imgObj = img.name.Remove(0, 6);
            if (imgObj == imageToSearch)
            {
                return img.sprite;
            }
        }
        return null;
    }
}