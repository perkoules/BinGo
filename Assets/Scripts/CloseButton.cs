using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public GameObject[] panels;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Clicked);
    }

    public void Clicked()
    {
        if (button.name.Contains("Close"))
        {
            foreach (var panel in panels)
            {
                panel.SetActive(false);
                if (panel == panels[0])
                {
                    panel.SetActive(true);
                }
            }
        }
    }
}