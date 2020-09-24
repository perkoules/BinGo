using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public TabButton selectedTab;
    public List<GameObject> gameobjectsToSwap;

    public Color pressedColor;
    public Color hoverColor;
    public Color disabledColor;

    private void OnEnable()
    {
        foreach (var item in tabButtons)
        {
            if (item.name.Contains("Amazon"))
            {
                item.background.color = pressedColor;
            }
            else
            {
                item.background.color = disabledColor;
            }
        }
    }
    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.color = hoverColor;
        }
    }
    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.color = pressedColor;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < gameobjectsToSwap.Count; i++)
        {
            if(i == index)
            {
                gameobjectsToSwap[i].SetActive(true);
            }
            else
            {
                gameobjectsToSwap[i].SetActive(false);

            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button  in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.background.color = disabledColor;
        }
    }
    public void Resetter()
    {
        foreach (var item in gameobjectsToSwap)
        {
            item.SetActive(false);
        }
        ResetTabs();
    }
}
