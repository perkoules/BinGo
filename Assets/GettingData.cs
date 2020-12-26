using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingData : MonoBehaviour
{
    public ModalWindowManager gettingDataMessage;
    private void OnEnable()
    {
        LeaderboardManager.OnDataRetrieved += CloseWindow;
    }
    private void OnDisable()
    {
        LeaderboardManager.OnDataRetrieved -= CloseWindow;
    }
    public void FocusWindowChanged()
    {
        gettingDataMessage.OpenWindow();
    }

    private void CloseWindow()
    {
        gettingDataMessage.CloseWindow();
    }
}
