using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlagSelection : MonoBehaviour
{
    public Sprite noneSelected;
    public Image selectedFlag;
    public string selectedCountryName;
    public Dictionary<string, Sprite> countries = new Dictionary<string, Sprite>();

    public TMP_Dropdown dropdown;
    public List<Sprite> countryFlag = new List<Sprite>();
    public List<string> countryName = new List<string>();

    public List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

    private void Awake()
    {
        dropdown = dropdown.GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        Image[] imagesObj = gameObject.GetComponentsInChildren<Image>();
        foreach (var item in imagesObj)
        {
            countryFlag.Add(item.sprite);
            countryName.Add(item.sprite.name);
            options.Add(new TMP_Dropdown.OptionData { image = item.sprite, text = item.sprite.name });
        }
        dropdown.AddOptions(options);
        
    }
    public Sprite AssignImage(string countryName)
    {
        return countryFlag.Find(spr => spr.name == countryName);        
    }

}
