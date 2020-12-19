using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetScav : MonoBehaviour
{
    public TextMeshProUGUI txt, tasks;

    public PlayerDataSaver pl;
    private void Start()
    {
        if (tasks != null)
        {
            tasks.text = pl.GetScavHunt() + " =>  " + pl.GetHuntProgress();
        }
    }

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

    public void SetToZero()
    {
        var pl = GetComponent<PlayerDataSaver>();
        pl.SetScavHunt(0);
        pl.SetHuntProgress("00000");
    }
}