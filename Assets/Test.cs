using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public PlayerDataSaver playerDataSaver;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerDataSaver.SetScavHunt(1);
        }
        else
        {
            Debug.Log(playerDataSaver.GetScavHunt());
        }
    }
}
