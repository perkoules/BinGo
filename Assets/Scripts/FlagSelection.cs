using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class FlagSelection : MonoBehaviour
{
    public Sprite noneSelected;
    public Image selectedFlag;
    public string selectedCountryName;
    public Dictionary<string, Sprite> countries = new Dictionary<string, Sprite>();

    private void Start()
    {
        selectedCountryName ="Greece";
        Image[] imagesObj = gameObject.GetComponentsInChildren<Image>();
        foreach (var item in imagesObj)
        {
            countries.Add(item.sprite.name, item.sprite);
        }
    }

    private void Update()
    {
        if (!countries.ContainsKey(selectedCountryName))
        {
            selectedFlag.sprite = noneSelected;
        }
        else
        {
            selectedFlag.sprite = countries[selectedCountryName];
        }
    }
}
