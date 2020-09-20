using TMPro;
using UnityEngine;
public class TextDisplay : MonoBehaviour
{
    private TextMeshProUGUI currentText;
    private void OnEnable()
    {
        currentText = GetComponent<TextMeshProUGUI>();
        //currentText.text
    }
}
