using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shield : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject prefabShield, go;
    bool holding = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        go = Instantiate(prefabShield, Camera.main.transform.position + Vector3.forward * 5, Quaternion.identity, Camera.main.transform);
        holding = true;
    }
    private void Update()
    {
        if (go != null)
        {
            if (holding && go.transform.localScale.x < 5)
            {
                go.transform.localScale += Vector3.one * 0.05f;
            }
            else if (!holding)
            {
                go.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            } 
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
    }
}
