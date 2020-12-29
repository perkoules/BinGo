using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookForceField : MonoBehaviour
{
    public GameObject portalRoot;

    private void Awake()
    {
        LineTrace.OnBookObtained += LineTrace_OnBookObtained;
    }

    private void LineTrace_OnBookObtained()
    {
        //Maybe dissolve
        Destroy(portalRoot);
    }

    private void OnDestroy()
    {
        LineTrace.OnBookObtained += LineTrace_OnBookObtained;
    }
}
