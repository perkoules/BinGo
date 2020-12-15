using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetScav : MonoBehaviour
{
    public TextMeshProUGUI txt;    

    public void Minus()
    {
        GameObject go = FindObjectOfType<BattleController>().enemy;
        go.gameObject.transform.localScale -= Vector3.one * 0.05f;
        txt.text = go.transform.localScale.x.ToString();
    }
    public void Plus()
    {
        GameObject go = FindObjectOfType<BattleController>().enemy;
        go.gameObject.transform.localScale += Vector3.one * 0.05f;
        txt.text = go.transform.localScale.x.ToString();
    }
}