using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; set; }
    public delegate void BattleCompleted(int shieldAvailable, int projectileAvailable);
    public static event BattleCompleted OnBattleCompleted;


    public BattleState currentState;
    public TextMeshProUGUI shieldAmount, attackAmount;
    public GameObject prefabAttack, prefabPlayerDeath, enemy, battlePanel;
    public Button shieldBtn, attackBtn;
    public TextMeshProUGUI battleText;

    private PlayerDataSaver playerDataSaver;
    private Camera player;
    private Enemy enemyScript;
    public bool playerProjectileDead, playerProjectileHit = false;
    public bool enemyProjectileDead, enemyProjectileHit = false;

    private int shieldAvailable, projectileAvailable = 0;
    private int shieldUsed, projectileUsed = 0;


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
        playerDataSaver = GetComponent<PlayerDataSaver>();
        shieldUsed = playerDataSaver.GetShieldUsed();
        shieldAvailable = Convert.ToInt32(shieldAmount) - shieldUsed;
        projectileUsed = playerDataSaver.GetProjectileUsed();
        projectileAvailable = Convert.ToInt32(attackAmount) - projectileUsed;
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
        if (ProjectileAvailable() > 0)
        {
            attackBtn.interactable = true;
        }
    }

    public void OnPlayerAttack()
    {
        if(currentState != BattleState.PlayerTurn)
        {
            return;
        }
        projectileUsed++;
        attackAmount.text = ProjectileAvailable().ToString();
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

    public void ShieldPressed()
    {
        shieldUsed++;
        shieldAmount.text = ShieldAvailable().ToString();
    }

    IEnumerator EnemyTurn()
    {
        battleText.text = "Enemy's turn. Shield yourself.";
        if (ShieldAvailable() > 0)
        {
            shieldBtn.interactable = true;
        }
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
            battleText.text = "You lost, losing Loser!";
            GameObject plDeath = Instantiate(prefabPlayerDeath, player.transform.position + Vector3.forward * 5, Quaternion.identity);
            Destroy(plDeath, 3f);
        }
        battlePanel.SetActive(true);
        playerDataSaver.SetShieldUsed(shieldUsed);
        playerDataSaver.SetProjectileUsed(projectileUsed);
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
    public int ShieldAvailable()
    {
        shieldAvailable -= shieldUsed;
        return shieldAvailable;
    }

    public int ProjectileAvailable()
    {
        projectileAvailable -= projectileUsed;
        return projectileAvailable;
    }
}
