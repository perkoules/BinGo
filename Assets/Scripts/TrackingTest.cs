using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackingTest : MonoBehaviour
{
    private ARSession aRSession;
    public ARTrackedImageManager imageManager;
    public Image frame;

    // Start is called before the first frame update
    void Start()
    {
        aRSession = GetComponent<ARSession>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in imageManager.trackables)
        {
            if(item.trackingState == TrackingState.Tracking)
            {
                frame.color = Color.green;
            }
            else
            {
                frame.color = Color.white;
            }
        }
    }
}
