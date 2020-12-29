using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            Raycasting(Input.mousePosition);
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("Android");
            Raycasting(Input.GetTouch(0).position);
        }
#endif
    }
    public delegate void MonsterClicked(string rayTag, GameObject go);
    public static event MonsterClicked OnMonsterClicked;
    private void Raycasting(Vector3 position)
    {
        
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                OnMonsterClicked(hit.transform.gameObject.tag, hit.transform.gameObject);
            }
        
    }
}
