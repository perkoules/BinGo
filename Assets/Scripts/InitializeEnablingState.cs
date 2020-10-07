using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver))]
public class InitializeEnablingState : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    private Button teamnameSetterButton;

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        teamnameSetterButton = GetComponent<Button>();
    }

    private void Start()
    {
        string teamname = playerDataSaver.GetTeamname();
        if (teamname == "-")
        {
            teamnameSetterButton.gameObject.SetActive(true);
        }
        else
        {
            teamnameSetterButton.gameObject.SetActive(false);
        }
    }
}