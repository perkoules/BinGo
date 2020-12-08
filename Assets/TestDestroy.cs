using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TestDestroy : MonoBehaviour
{
    private Button btn;
    public TextMeshProUGUI ww, rr;
    public int w, r = 0;

    public delegate void GainAmmoShield(int waste, int recycled);
    public static event GainAmmoShield OnGainAmmoShield;
    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(Gain);
    }
    public void Gain()
    {
        w = Random.Range(1, 100);
        r = Random.Range(1, 100);
        OnGainAmmoShield(w, r);
    }
}
