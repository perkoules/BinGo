using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingData : MonoBehaviour
{
    public ModalWindowManager gettingDataMessage;
    
    public void FocusWindowChanged()
    {
        LeaderboardManager.OnDataRetrieved += CloseWindow;
        gettingDataMessage.OpenWindow();        
    }

    private void CloseWindow()
    {
        LeaderboardManager.OnDataRetrieved -= CloseWindow;
        gettingDataMessage.CloseWindow();
    }
}
