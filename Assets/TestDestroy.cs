using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public delegate void MonsterClicked();
    public static event MonsterClicked OnMonsterClicked;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                OnMonsterClicked();
                /*GameObject go = hit.transform.gameObject;
                if (go.tag == "MonsterTag" && !monsterGotHit)
                {
                    monsterGotHit = true;
                    go.GetComponent<RoamingMonster>().DestroyObject();
                    Death();
                }*/
            }
        }
    }
}
