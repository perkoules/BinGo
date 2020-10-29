using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutologinController : MonoBehaviour
{
    private Toggle toggle;


    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(AutologinChecker);
    }

    private void AutologinChecker(bool isOn)
    {
        if (isOn)
        {
            LoginManager.LM.ShouldAutologin(isOn);
        }
    }
}
