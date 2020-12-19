using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour, IPointerClickHandler
{
    public TabGroup tabgroup;
    public Image background;
    [SerializeField] string infoTextToShow;
    public TextMeshProUGUI infoText;

    private void OnEnable()
    {
        if(tabgroup.selectedTab == this)
        {
            var tab = tabgroup.gameobjectsToSwap.Find(t => t.name.Contains(this.name.Replace("Option","")));
            tab.SetActive(true);
        }
    }

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

}