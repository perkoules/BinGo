using GoogleARCore.Examples.HelloAR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RePositionCamera : MonoBehaviour
{
    public Transform arcoreDevice, arPlayer;

    public void ReCenter()
    {
        arcoreDevice.position = arPlayer.position + Vector3.up * 5;
        arcoreDevice.rotation = arPlayer.rotation;
    }

    public void Invokebutton(Button btn)
    {
        btn.onClick.Invoke();
    }
}