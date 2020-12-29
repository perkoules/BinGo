using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoPoints : MonoBehaviour
{
    private void Awake()
    {
        MonsterDestroyer.OnMonsterClicked += MonsterDestroyer_OnMonsterClicked;
    }

    private void MonsterDestroyer_OnMonsterClicked(string rayTag, GameObject go)
    {
        if (gameObject.CompareTag(rayTag) && go == this.gameObject)
        {
            //Send coins to cloudscript
            Destroy(this.gameObject);
        }
    }
}
