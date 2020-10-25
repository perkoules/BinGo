using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDestroyer : MonoBehaviour
{    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                GameObject go = hit.transform.gameObject;
                if (go.name.Contains("Monster"))
                {
                    go.GetComponent<Animator>().SetBool("IsDead", true);
                    Destroy(go, 3f);
                }
            }
        }
    }
}
