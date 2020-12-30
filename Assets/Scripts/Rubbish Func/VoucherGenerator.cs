using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoucherGenerator : MonoBehaviour
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public TextMeshProUGUI codeProduced;
    public void GenerateCode(string nameWorth)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 10; i++)
        {
            int rand = UnityEngine.Random.Range(0, chars.Length);
            sb.Append(chars[rand]);
        }
        sb.Append(nameWorth);
        codeProduced.text = sb.ToString();
        gameObject.GetComponentInParent<GiftCardEnabler>().CardUsed(Convert.ToInt32(nameWorth));
    }
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