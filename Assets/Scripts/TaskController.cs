using UnityEngine;
using UnityEngine.EventSystems;

public class TaskController : MonoBehaviour, IPointerClickHandler
{
    private Animator anim;
    public bool isOn = true;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        anim.ResetTrigger("OpenTask");
        anim.ResetTrigger("CloseTask");
        anim.SetTrigger("Reset");
        TaskChecker.Instance.FindActiveTasks();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isOn)
        {
            anim.SetBool("Open", isOn);
            isOn = false;
            anim.SetTrigger("OpenTask");
            anim.ResetTrigger("CloseTask");
        }
        else
        {
            anim.SetBool("Open", isOn);
            isOn = true;
            anim.SetTrigger("CloseTask");
            anim.ResetTrigger("OpenTask");
        }
    }

    private void OnDestroy()
    {
        
    }
}