using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstTabControl : MonoBehaviour
{
    public WindowManager windowManager;
    public Button firstWindowButton;
    private int index;
    private void OnEnable()
    {
        index = windowManager.currentWindowIndex;
        if(index == 0)
        {
            firstWindowButton.onClick.Invoke();
        }
    }   
}
