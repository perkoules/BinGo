using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlantTreeCondition : MonoBehaviour
{
    public ModalWindowManager plantTreeWindow;
    public void CheckAmount(TextMeshProUGUI amountTxt)
    {
        int amount = System.Convert.ToInt32(amountTxt.text);
        if (amount > 0)
        {
            plantTreeWindow.OpenWindow();
        }
    }
}
