using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
    public static Container container;
    public List<Image> imageContainer;
    private void OnEnable()
    {
        container = this;
    }
    private void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Image img = transform.GetChild(i).GetComponentInChildren<Image>();
            imageContainer.Add(img);
        }        
    }
}
