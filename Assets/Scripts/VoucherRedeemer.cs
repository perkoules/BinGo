using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoucherRedeemer : MonoBehaviour
{
    //codeReaveal and voucherWorth names are swapped 
    public TextMeshProUGUI codeReveal;
    private Button voucher;
    public TextMeshProUGUI voucherWorth;
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private void OnEnable()
    {
        voucher = GetComponent<Button>();
    }
    private void Start()
    {
        voucher.onClick.AddListener(GenerateCode);
    }

    private void GenerateCode()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 10; i++)
        {
            int rand = UnityEngine.Random.Range(0, chars.Length);
            sb.Append(chars[rand]);
        }
        sb.Append(codeReveal.text);
        voucherWorth.text = sb.ToString();
        gameObject.GetComponentInParent<GiftCardEnabler>().CardUsed(Convert.ToInt32(codeReveal.text));
    }
}
