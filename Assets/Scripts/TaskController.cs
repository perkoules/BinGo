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
        TaskChecker.Instance.FindActiveTasks();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isOn)
        {
            isOn = false;
            anim.SetTrigger("OpenTask");
        }
        else
        {
            isOn = true;
            anim.SetTrigger("CloseTask");
        }
    }

}