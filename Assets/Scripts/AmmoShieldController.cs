using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AmmoShieldController : MonoBehaviour
{
    public Button shieldButton, attackButton;
    public TextMeshProUGUI shieldAmount, attackAmount;
    public bool isPlayerTurn = false;
    private int sh, att = 0;
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

    public IEnumerator AmmoShieldCooldown()
    {
        shieldButton.interactable = false;
        attackButton.interactable = false;
        yield return new WaitUntil(() => isPlayerTurn == true);
        shieldButton.interactable = true;
        attackButton.interactable = true;
    }

    public void SendAmmoShieldusedToCloud()
    {
        gameObject.SetActive(false);
        //update player info
    }
}
