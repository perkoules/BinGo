using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch myTouch = Input.GetTouch(0);

            //Swipe Right
            if (myTouch.phase == TouchPhase.Began && myTouch.position.x >= Screen.width / 2)
            {
            }
            if (myTouch.phase == TouchPhase.Ended && myTouch.position.x < Screen.width / 2)
            {
                anim.SetTrigger("OpenTask");
                anim.ResetTrigger("CloseTask");
            }
            //Swipe Left
            if (myTouch.phase == TouchPhase.Began && myTouch.position.x < Screen.width / 2)
            {
            }
            if (myTouch.phase == TouchPhase.Ended && myTouch.position.x >= Screen.width / 2)
            {
                anim.SetTrigger("CloseTask");
                anim.ResetTrigger("OpenTask");
            }
        }
    }

    public void OnTaskComplete()
    {
        Destroy(gameObject);
    }
}