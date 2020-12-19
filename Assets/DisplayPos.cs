using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayPos : MonoBehaviour
{
    public TextMeshProUGUI txt;
    

    // Update is called once per frame
    void Update()
    {
        txt.text = string.Format($"x: {0} , y: {1} , z: {2} ", gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
