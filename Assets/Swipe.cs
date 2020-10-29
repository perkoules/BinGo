using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    private Vector3 fp;  
    private Vector3 lp;  
    private float dragDistance;


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
                        if ((lp.x > fp.x)) 
                        {   //Right swipe
                            
                        }
                        else
                        {   //Left swipe
                            
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
