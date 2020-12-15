using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetScav : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    private Button btn;
    public delegate void ResetScHun();
    public static event ResetScHun OnResetHun;
    private void Awake()
    {
        btn = GetComponent<Button>();
        playerDataSaver = GetComponent<PlayerDataSaver>();
        btn.onClick.AddListener(ResetHunt);
    }
    private void ResetHunt()
    {
        OnResetHun();
    }
}