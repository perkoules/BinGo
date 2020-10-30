﻿using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public TabButton selectedTab;
    public List<GameObject> gameobjectsToSwap;

    public Color pressedColor;
    public Color disabledColor;

    

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }    

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.color = pressedColor;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < gameobjectsToSwap.Count; i++)
        {
            if (i == index)
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
        foreach (TabButton button in tabButtons)
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