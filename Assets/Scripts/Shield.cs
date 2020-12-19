using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shield : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject prefabShield, shieldPoint;
    private GameObject go;
    bool holding = false;

    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (btn.interactable)
        {
            go = Instantiate(prefabShield, shieldPoint.transform.position, Quaternion.identity, shieldPoint.transform);
            Destroy(go, 10f);
            holding = true;
        }
    }
    private void Update()
    {
        if (go != null)
        {                
            if (holding && go.transform.localScale.x < 10)
            {
                go.transform.localScale += Vector3.one * 0.1f;
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
