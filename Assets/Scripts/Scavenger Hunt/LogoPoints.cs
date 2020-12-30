using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoPoints : MonoBehaviour
{
    public delegate void LogoFound();
    public static LogoFound OnLogoFound;

    private void Awake()
    {
        MonsterDestroyer.OnMonsterClicked += MonsterDestroyer_OnMonsterClicked;
    }

    private void MonsterDestroyer_OnMonsterClicked(string rayTag, GameObject go)
    {
        if (gameObject.CompareTag(rayTag) && go == this.gameObject)
        {
            OnLogoFound?.Invoke();
            Destroy(this.gameObject);
        }
    }    

    private void OnDestroy()
    {
        MonsterDestroyer.OnMonsterClicked -= MonsterDestroyer_OnMonsterClicked;
    }
}
