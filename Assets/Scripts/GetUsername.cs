using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GetUsername : MonoBehaviour
{
    private TextMeshProUGUI myText;
    PlayfabManager pfm;

    private void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
        pfm = FindObjectOfType<PlayfabManager>();
        pfm.OnChangedTextEvent += Pfm_OnChangedTextEvent;
    }
    private void Pfm_OnChangedTextEvent(string txt)
    {
        myText.text = txt.Replace(txt.First(), char.ToUpper(txt.First()));
    }
}
