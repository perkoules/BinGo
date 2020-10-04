using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodeCopy : MonoBehaviour
{
    private TextMeshProUGUI textToCopy;
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
        Debug.Log("Copied code: " + textToCopy.text);
    }
}
