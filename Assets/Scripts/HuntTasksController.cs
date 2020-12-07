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
        ScavengerHunt.OnTaskCompleted += ScavengerHunt_OnTaskCompleted;
        myButton.onClick.AddListener(TogglePanel);
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


    private void ScavengerHunt_OnTaskCompleted(string obj)
    {
        int index = Array.FindIndex(taskImages, i => i.gameObject.name.Contains(obj));
        taskImages[index].interactable = false;
        if(taskImages.All(img => img.interactable == false))
        {
            warning.gameObject.SetActive(false);
            tick.gameObject.SetActive(true);
        }
        else
        {
            ShowDirectionsToPoint(index);
        }
    }

    public void ShowDirectionsToPoint(int task)
    {
        //task++
        //get directions On Map
    }
}
