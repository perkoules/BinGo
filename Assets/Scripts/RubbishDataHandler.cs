using System.Collections.Generic;
using UnityEngine;

public class RubbishDataHandler : MonoBehaviour
{
    public Dictionary<string, int> placeRubbishPair;
    public Dictionary<string, int> districtRubbishPair;
    public Dictionary<string, int> regionRubbishPair;
    public Dictionary<string, int> countryRubbishPair;

    

    private void OnEnable()
    {
        placeRubbishPair = new Dictionary<string, int>();
        districtRubbishPair = new Dictionary<string, int>();
        regionRubbishPair = new Dictionary<string, int>();
        countryRubbishPair = new Dictionary<string, int>();
    }

    public void ShowDic()
    {
        Debug.Log("=================Place=================");
        foreach (var item in placeRubbishPair)
        {
            Debug.Log(item.Key + " -- " + item.Value);
        }
        Debug.Log("=================District=================");
        foreach (var item in districtRubbishPair)
        {
            Debug.Log(item.Key + " -- " + item.Value);
        }
        Debug.Log("=================Region=================");
        foreach (var item in regionRubbishPair)
        {
            Debug.Log(item.Key + " -- " + item.Value);
        }
        Debug.Log("=================Country=================");
        foreach (var item in countryRubbishPair)
        {
            Debug.Log(item.Key + " -- " + item.Value);
        }
    }
}
