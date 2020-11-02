using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public List<GameObject> steps;

    private void OnEnable()
    {
        for (int i = 0; i < steps.Count; i++)
        {
            if (i == 0)
            {
                steps[i].SetActive(true);
            }
            else
            {
                steps[i].SetActive(false);
            }
        }
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
