using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatTextControl : MonoBehaviour
{
    public TextMeshProUGUI textToShow;
    public Button myButton;

    private void Awake()
    {
        myButton.onClick.AddListener(ButtonInteraction);
    }

    private void ButtonInteraction()
    {
        Destroy(gameObject);
    }
}
