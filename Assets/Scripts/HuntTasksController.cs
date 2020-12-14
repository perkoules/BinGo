using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuntTasksController : MonoBehaviour
{
    private Button myButton;
    public Button[] taskImages;
    public RawImage warning, tick;
    public static HuntTasksController Instance { get; set; }
    private Animator anim;
    public bool isOn = true;
    private void OnEnable()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
        anim = GetComponent<Animator>();
        anim.ResetTrigger("OpenTask");
        anim.ResetTrigger("CloseTask");
    }
    private void Awake()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(TogglePanel);
        ScavengerHunt.OnTaskCompleted += ScavengerHunt_OnTaskCompleted;
    }

    public void TogglePanel()
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


    private void ScavengerHunt_OnTaskCompleted(GameObject obj, string tasks, bool done)
    {
        char[] tasksArray = tasks.ToCharArray();
        for (int i = 0; i < tasksArray.Length; i++)
        {
            if (tasksArray[i] == '1')
            {
                taskImages[i].interactable = false;
            }
        }
        if(taskImages.All(img => img.interactable == false))
        {
            warning.gameObject.SetActive(false);
            tick.gameObject.SetActive(true);
            Destroy(gameObject, 10f);
        }
    }
}
