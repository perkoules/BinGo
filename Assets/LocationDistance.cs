using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationDistance : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float distance;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        text = GameObject.FindGameObjectWithTag("ttt").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(text == null)
        {
            text = GameObject.FindGameObjectWithTag("ttt").GetComponent<TextMeshProUGUI>();
        }
        distance = Vector3.Distance(this.gameObject.transform.position, player.transform.position);
        text.text = distance.ToString();
    }
}
