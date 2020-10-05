using UnityEngine;

public class TabAdjust : MonoBehaviour
{
    public TabGroup tabGroup;
    public TabButton tabToOpen;
    public GameObject tabResultToShow;

    private void OnEnable()
    {
        tabGroup.selectedTab = tabToOpen;
        tabResultToShow.SetActive(true);
    }
}