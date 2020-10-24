using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDestroyer : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.name.Contains("Monster"))
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
