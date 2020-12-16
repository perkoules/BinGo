using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePositionCamera : MonoBehaviour
{
    public Transform arcoreDevice, arPlayer;

    public void ReCenter()
    {
        arcoreDevice.position = arPlayer.position;
        arcoreDevice.rotation = arPlayer.rotation;
    }

}
