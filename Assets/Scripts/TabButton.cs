using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TabGroup tabgroup;
    public Image background;
    [SerializeField] string infoTextToShow;
    public TextMeshProUGUI infoText;
    private void Start()
    {
        background = GetComponent<Image>();
        tabgroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (infoText != null)
        {
            infoText.text = infoTextToShow;
        }
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