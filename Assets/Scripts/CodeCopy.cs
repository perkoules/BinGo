using TMPro;
using UnityEngine;

public class CodeCopy : MonoBehaviour
{
    public void CopyTextToClipboard(TextMeshProUGUI textToCopy)
    {
        TextEditor editor = new TextEditor
        {
            text = textToCopy.text
        };
        editor.SelectAll();
        editor.Copy();
    }
}