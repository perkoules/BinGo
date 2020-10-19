using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWindow : MonoBehaviour
{
    public GameObject rubbishCamera, arSession;
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
        rubbishCamera.GetComponent<SimpleDemo>().Init();
        yield return new WaitForSeconds(2);
        rubbishCamera.GetComponent<SimpleDemo>().ClickStart();
    }
}
