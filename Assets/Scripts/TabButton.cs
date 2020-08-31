using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TabGroup tabgroup;
    public Image background;
    
    void Start()
    {
        background = GetComponent<Image>();
        tabgroup.Subscribe(this);
    }
    

    public void OnPointerClick(PointerEventData eventData)
    {
        tabgroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabgroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabgroup.OnTabExit(this);
    }


}
