using TMPro;
using UnityEngine;

public class CodeCopy : MonoBehaviour
{
    private TextMeshProUGUI textToCopy;
    private TextMeshProUGUI myText;
    public GameObject prefabNotification;
    public RectTransform parent;
    private void Start()
    {
        textToCopy = GetComponent<TextMeshProUGUI>();
    }

    public void CopyText()
    {
        TextEditor editor = new TextEditor
        {
            text = textToCopy.text
        };
        editor.SelectAll();
        editor.Copy();
        myText = prefabNotification.GetComponentInChildren<TextMeshProUGUI>();
        myText.text = "Code copied to clipboard";
        GameObject go = Instantiate(prefabNotification, parent);
        Destroy(go, 2f);
    }
}