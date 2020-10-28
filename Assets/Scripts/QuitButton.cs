using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    public GameObject message, arImage;
    private Button myBtn;

    private void Awake()
    {
        myBtn = GetComponent<Button>();
    }

    void Start()
    {
        myBtn.onClick.AddListener(QuitApplicationProcess);
    }

    private void QuitApplicationProcess()
    {
        if (arImage.activeSelf)
        {
            message.SetActive(true);
        }
    }
}
