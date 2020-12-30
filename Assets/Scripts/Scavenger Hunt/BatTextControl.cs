using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatTextControl : MonoBehaviour
{
    public TextMeshProUGUI textToShow;
    public Button close, prev, nxt;

    public List<ScavengerHuntTask> intro;
    private int index = 0;
    private void Awake()
    {
        close.onClick.AddListener(ButtonDestruction);
        prev.onClick.AddListener(PreviousButton);
        nxt.onClick.AddListener(NextButton);
    }
    private void Start()
    {
        textToShow.text = intro[index].TextToShow;
    }
    private void Update()
    {
        if(index == 0)
        {
            prev.gameObject.SetActive(false);
        }
        else if (index == 9)
        {
            nxt.gameObject.SetActive(false);
        }
        else
        {
            prev.gameObject.SetActive(true);
            nxt.gameObject.SetActive(true);
        }
    }
    private void ButtonDestruction()
    {
        Destroy(gameObject);
        MonsterDestroyer.Instance.BattlePanelController(false);
    }
    private void PreviousButton()
    {
        index--;
        if (index >= 1)
        {
            textToShow.text = intro[index].TextToShow;
        }
        else
        {
            index = 0;
        }
    }
    private void NextButton()
    {
        index++;
        if (index <= 9)
        {
            textToShow.text = intro[index].TextToShow;
        }
        else
        {
            index = 9;
        }
    }
}
