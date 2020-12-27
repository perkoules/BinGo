using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class TouchLongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public ModalWindowManager modalWindow;
    public bool isHolding = false;
    public float timeToHold = 2;
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    private void Update()
    {
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (isHolding)
            {
                timeToHold -= Time.deltaTime;
                if(timeToHold <= 0)
                {
                    modalWindow.OpenWindow();
                    timeToHold = 2f;
                    isHolding = false;
                    touch.phase = TouchPhase.Ended;
                }
            }
        }
    }
}
