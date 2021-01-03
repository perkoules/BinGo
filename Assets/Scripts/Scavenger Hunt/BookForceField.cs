using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookForceField : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    public GameObject portalRoot;

    private void OnEnable()
    {
        if(playerDataSaver.GetBookObtained() == 1)
        {
            Destroy(this.gameObject.transform.root.gameObject);
        }
    }

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        LineTrace.OnBookObtained += LineTrace_OnBookObtained;
    }

    private void LineTrace_OnBookObtained()
    {
        playerDataSaver.SetBookObtained(1);
        Destroy(portalRoot);
    }

    private void OnDestroy()
    {
        LineTrace.OnBookObtained -= LineTrace_OnBookObtained;
    }
}
