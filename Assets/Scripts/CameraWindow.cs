using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWindow : MonoBehaviour
{
    public GameObject rubbishCamera, arSession;
    public ScanRubbish scanRubbish;
    private void OnEnable()
    {
        arSession.SetActive(false);
        StartCoroutine(CheckIfEnabled());
    }
    private IEnumerator CheckIfEnabled()
    {
        if (!rubbishCamera.activeSelf)
        {
            yield return new WaitUntil(() => rubbishCamera.activeSelf);
        }
        scanRubbish.Init();
        yield return new WaitForSeconds(2);
        scanRubbish.Play();
    }
}
