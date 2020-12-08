using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AmmoShieldController : MonoBehaviour
{
    public PlayerDataSaver dataSaver;
    public TextMeshProUGUI shieldAmount, attackAmount;    
    private int sh, att = 0;

    private void OnEnable()
    {
        shieldAmount.text = dataSaver.GetRecycleCollected().ToString();
        attackAmount.text = dataSaver.GetWasteCollected().ToString();
    }

    public void ShieldUsed()
    {
        sh = Convert.ToInt32(shieldAmount);
        sh--;
        shieldAmount.text = sh.ToString();
    }

    public void ProjectileUsed()
    {
        att = Convert.ToInt32(shieldAmount);
        att--;
        attackAmount.text = att.ToString();
    }

    public void SendAmmoShieldusedToCloud()
    {
        gameObject.SetActive(false);
        //update player info
    }
}
