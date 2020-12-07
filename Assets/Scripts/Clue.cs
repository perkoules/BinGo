using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clue : MonoBehaviour
{
    public List<ScavengerHuntTask> random;
    public List<ScavengerHuntTask> tasks;

    public GameObject prefabTutorial;

    private Button batButton;

    void Awake()
    {
        batButton = GetComponent<Button>();
        batButton.onClick.AddListener(ShowText);
    }

    private void ShowText()
    {
        //Check Which Objective and ONLY 1
        int taskToShowInfo = ScavengerHunt.Instance.CurrentHuntTask();        
        GameObject go = Instantiate(prefabTutorial, gameObject.transform);
        go.GetComponent<BatTextControl>().textToShow.text = tasks[taskToShowInfo].TextToShow;
    }
}
