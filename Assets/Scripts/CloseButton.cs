using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject[] leaderbordHolders;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Clicked);
    }

    public void Clicked()
    {
        if (button.name.Contains("Close"))            
        {
            foreach (var panel in panels)
            {
                panel.SetActive(false);
                if (panel == panels[0])
                {
                    panel.SetActive(true);
                }
            }
            foreach (var item in leaderbordHolders)
            {
                if (item.activeSelf)
                {
                    item.GetComponent<GetLeaderboard>().ClearLeaderboard();
                }
            } 
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
