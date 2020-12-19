using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Swipe : MonoBehaviour
{
    private Vector3 fp;  
    private Vector3 lp;  
    private float dragDistance;
    public Button[] buttons;
    public TabGroup tabGroup;

    void Start()
    {
        dragDistance = Screen.height * 15 / 100; 
    }

    void Update()
    {
        if (Input.touchCount == 1) 
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) 
            {
                lp = touch.position;  

                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {
                        int current = Array.FindIndex(buttons, btn => btn.GetComponent<Image>().color == tabGroup.pressedColor);
                        if ((lp.x > fp.x)) 
                        {
                            if(current == 0)
                            {
                                current = buttons.Length;
                            }
                            ExecuteEvents.Execute<IPointerClickHandler>(buttons[current - 1].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                        }
                        else
                        {
                            if(current == buttons.Length - 1)
                            {
                                current = -1;
                            }
                            ExecuteEvents.Execute<IPointerClickHandler>(buttons[current + 1].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                        }
                    }
                    else
                    {   
                        if (lp.y > fp.y)
                        {   //Up swipe
                            
                        }
                        else
                        {   //Down swipe
                            Debug.Log("Sync Data");
                        }
                    }
                }
                else
                {   
                    //It's a tap as the drag distance is less than 20% of the screen height
                    
                }
            }
        }
    }
}
