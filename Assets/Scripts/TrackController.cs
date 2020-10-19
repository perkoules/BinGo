using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    private ARCoreSessionConfig mySession;
    public AugmentedImageDatabase myImageDatabase;

    void Awake()
    {
        mySession = transform.parent.GetComponent<ARCoreSessionConfig>();
    }
    public void StartTracking()
    {
        mySession.AugmentedImageDatabase = myImageDatabase;
    }

    public void StopTracking()
    {
        mySession.AugmentedImageDatabase = null;
    }
}
