using UnityEngine;
using UnityEngine.UI;

public class TabAdjust : MonoBehaviour
{
    public TabGroup tabGroup;
    public TabButton tabToOpen;


    private void OnEnable()
    {
        tabGroup.OnTabSelected(tabToOpen);
        //Invoke("ColorInitialization", 0.5f);
    }

    private void ColorInitialization()
    {
        foreach (var item in tabGroup.tabButtons)
        {
            if (item.gameObject.name.Contains("Amazon") || 
                item.gameObject.name.Contains("Players") || 
                item.gameObject.name.Contains("City"))
            {
                item.background.color = tabGroup.pressedColor;
            }
            else
            {
                item.background.color = tabGroup.disabledColor;
            }
        }
    }
}