using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BadgeExplanation : MonoBehaviour
{
    [SerializeField] 
    private GameObject prefabNotification;
    [SerializeField]
    private string textToShow;
    [SerializeField]
    private RectTransform parent;
    private Button btn;
    private TextMeshProUGUI myText;


    void Awake()
    {
        btn = GetComponent<Button>();
        myText = prefabNotification.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        btn.onClick.AddListener(InitNotification);
    }

    private void InitNotification()
    {
        myText.text = textToShow;
        GameObject go = Instantiate(prefabNotification, parent);
        Destroy(go, 2f);
    }
}
