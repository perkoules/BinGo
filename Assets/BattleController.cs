using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    public BattleState currentState;
    public GameObject prefabAttack, prefabPlayerDeath, enemy, battlePanel;
    public Button shieldBtn, attackBtn;
    public TextMeshProUGUI battleText;
    private Camera player;
    private Enemy enemyScript;

    public bool playerProjectileDead, playerProjectileHit = false;
    public bool enemyProjectileDead, enemyProjectileHit = false;

    public static BattleController Instance { get; set; }
    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Awake()
    {
        MonsterDestroyer.OnMonsterClicked += MonsterDestroyer_OnMonsterClicked;
        player = Camera.main;
        battleText.text = "Battle info...";
        currentState = BattleState.Idle;
    }

    private void MonsterDestroyer_OnMonsterClicked(string rayTag, GameObject go)
    {
        if (go.CompareTag(rayTag) && rayTag.Equals("MonsterHuntTag"))
        {
            enemy = go;
            enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.PrepareEnemyForBattle();
            currentState = BattleState.Start;
            StartCoroutine(StartBattle());
        }
    }

    private IEnumerator StartBattle()
    {
        battlePanel.SetActive(true);
        battleText.text = "Prepare to battle...";
        yield return new WaitUntil(() => enemyScript.isReadyToBattle == true);

        
        currentState = BattleState.PlayerTurn;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        battleText.text = "Your turn to attack...";
        attackBtn.interactable = true;
    }

    public void OnPlayerAttack()
    {
        if(currentState != BattleState.PlayerTurn)
        {
            return;
        }
        attackBtn.interactable = false;
        Instantiate(prefabAttack, player.transform.position, player.transform.rotation);
        StartCoroutine(PlayerAttack());
    }
    IEnumerator PlayerAttack()
    {
        
        battleText.text = "You attacked!";
        yield return new WaitForSeconds(5f);
        if (playerProjectileHit)
        {
            bool enemyIsDead = enemyScript.TakeDamage();
            if (enemyIsDead)
            {
                currentState = BattleState.Won;
                EndBattle();
            }
            else
            {
                currentState = BattleState.EnemyTurn;
                StartCoroutine(EnemyTurn());
            }
        }
        else if (!playerProjectileHit)
        {
            StartCoroutine(EnemyTurn());
        }
    }    
    IEnumerator EnemyTurn()
    {
        battleText.text = "Enemy's turn. Shield yourself.";
        shieldBtn.interactable = true;
        yield return new WaitForSeconds(3);
        enemyScript.AttackAmmo();
        if (enemyProjectileHit)
        {
            currentState = BattleState.Lost;
            EndBattle();
        }
        else if (!enemyProjectileHit)
        {
            currentState = BattleState.PlayerTurn;
            PlayerTurn();
        }

    }

    private void EndBattle()
    {
        if(currentState == BattleState.Won)
        {
            enemyScript.EnemyLost();
            battleText.text = "You WON. That's not good for my Lord!";
        }
        else if(currentState == BattleState.Lost)
        {
            enemyScript.EnemyWon();
            battleText.text = "You lost. Try again Loser!";
            GameObject plDeath = Instantiate(prefabPlayerDeath, player.transform.position + Vector3.forward * 5, Quaternion.identity);
            Destroy(plDeath, 3f);
        }
        //battlePanel.SetActive(true);
    }

    public void PlayerAttackHitResult(bool isProjDead, bool didHit)
    {
        playerProjectileDead = isProjDead;
        playerProjectileHit = didHit;
    }

    public void EnemyAttackHitResult(bool isEnProjDead, bool didEnHit)
    {
        enemyProjectileDead = isEnProjDead;
        enemyProjectileHit = didEnHit;
    }
}
