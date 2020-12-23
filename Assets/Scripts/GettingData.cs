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
        //gettingDataMessage.OpenWindow();
    }

    private void CloseNotification()
    {
        gettingDataMessage.CloseWindow();
    }

    private void OnDisable()
    {
        
    }
}
