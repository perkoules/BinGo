using UnityEngine;
using UnityEngine.EventSystems;

public class TaskController : MonoBehaviour, IPointerClickHandler
{
    private Animator anim;
    private bool isOn = true;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnTaskComplete()
    {
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isOn)
        {
            isOn = false;
            anim.SetTrigger("OpenTask");
            anim.ResetTrigger("CloseTask");
        }
        else
        {
            isOn = true;
            anim.SetTrigger("CloseTask");
            anim.ResetTrigger("OpenTask");
        }
    }
}