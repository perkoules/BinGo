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

    public int shieldAvailable, projectileAvailable = 0;
    public int shieldUsed, projectileUsed = 0;


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
        playerDataSaver = GetComponent<PlayerDataSaver>();
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


        shieldUsed = playerDataSaver.GetShieldUsed();
        shieldAvailable = playerDataSaver.GetRecycleCollected() - shieldUsed;
        projectileUsed = playerDataSaver.GetProjectileUsed();
        projectileAvailable = playerDataSaver.GetWasteCollected() - projectileUsed;

        currentState = BattleState.PlayerTurn;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        battleText.text = "Your turn to attack...";
        shieldBtn.interactable = false;
        if (projectileAvailable > 0)
        {
            attackBtn.interactable = true;
        }
    }

    /// <summary>
    /// What happens when player is attacking. (Triggered from
    /// a button onclick event in the editor)
    /// </summary>
    public void OnPlayerAttack()
    {
        if(currentState != BattleState.PlayerTurn)
        {
            return;
        }
        projectileUsed++;
        projectileAvailable = projectileAvailable - projectileUsed;
        playerDataSaver.SetProjectileUsed(projectileUsed);
        attackAmount.text = projectileAvailable.ToString();
        attackBtn.interactable = false;
        Instantiate(prefabAttack, player.transform.position, player.transform.rotation);
        StartCoroutine(PlayerAttack());
    }
    IEnumerator PlayerAttack()
    {        
        battleText.text = "You attacked!";
        yield return new WaitUntil(() => playerProjectileDead == true);
        playerProjectileDead = false;
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
        shieldAvailable = shieldAvailable - shieldUsed;
        playerDataSaver.SetShieldUsed(shieldUsed);
        shieldAmount.text = shieldAvailable.ToString();
    }

    IEnumerator EnemyTurn()
    {
        battleText.text = "Enemy's turn. Shield yourself.";
        if (shieldAvailable > 0)
        {
            shieldBtn.interactable = true;
        }
        yield return new WaitForSeconds(2);
        enemyScript.TriggerAttack();
        yield return new WaitUntil(() => enemyProjectileDead == true); 
        enemyProjectileDead = false;
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
        shieldBtn.interactable = false;
        attackBtn.interactable = false;
        if(currentState == BattleState.Won)
        {
            enemy = null;
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
        currentState = BattleState.Idle;
        battlePanel.SetActive(false);
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
}
