using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RubbishCollection : MonoBehaviour
{
    public TextMeshProUGUI rubbishText;
    public int rubbishCollected = 0;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Increase);
    }
    
    private void Increase()
    {
        rubbishCollected += 1;
        rubbishText.text = rubbishCollected.ToString();
        FindObjectOfType<PlayfabManager>().SetRubbishCollection(rubbishCollected);
    }

}
