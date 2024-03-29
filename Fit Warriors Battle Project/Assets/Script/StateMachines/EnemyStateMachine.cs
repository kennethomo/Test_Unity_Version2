﻿/*
 * FitWarriorsBattleProject
 * @Author: Dakota Ruhl
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : MonoBehaviour
{
    private BattleStateMachine BSM;
    public BaseEnemy enemy;
    public Animator anim;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;
    //for the ProgressBar
    public Image ProgressBar;
    private float curCooldown = 0f;
    private float maxCooldown = 5f;
    //this vector for animations startpos
    private Vector3 startPosition;
    public GameObject Selector;
    //timeforaction stuff
    private bool actionStarted = false;
    public GameObject heroToAttack;
    private float animSpeed = 10f;
    //When the game is over ENEMY DEAD
    private bool dead = false; 



    void Start()
    {
        currentState = TurnState.PROCESSING;
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startPosition = transform.position;
        anim = GetComponent<Animator>();
        setAllAnimatorsFalse();   //set all animation parameters false
    }

    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpgradeProgressBar();
                break;
            case (TurnState.CHOOSEACTION):
                if (BSM.turn)
                {
                    ChooseAction();
                    currentState = TurnState.WAITING;
                    BSM.turn = false;
                }
                break;
            case (TurnState.DEAD):
                GameObject performer = GameObject.Find("Enemy");
                setDead();                             //set animation paramter for Dead to TRUE
                dead = true; 
                break;
            case (TurnState.WAITING):
                //idle state
                setTakingDamage();                     //This state is when the enemy is sitting still waiting to be hit, so we set animation paramter for Damage to TRUE
                break;
            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                setAttacking();                       //set animation paramter for Attack to TRUE
                break;
        }
    }

    void UpgradeProgressBar()
    {
        curCooldown = curCooldown + Time.deltaTime;
        if (curCooldown >= maxCooldown)
        {
            currentState = TurnState.CHOOSEACTION;
        }
    }

    void ChooseAction()
    {
        TurnHandler myAttack = new TurnHandler();
        myAttack.Attacker = enemy.theName;
        myAttack.Type = "Enemy"; 
        myAttack.AttacksGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.HeroesInBattle[Random.Range (0, BSM.HeroesInBattle.Count)];   //random target from heroes on field //for later 

        int num = Random.Range(0, enemy.attacks.Count);         //random number to pick attack 
        myAttack.chosenAttack = enemy.attacks[num]; 
        BSM.CollectActions(myAttack);
        //Debug.Log(this.gameObject + " has chosen " + myAttack.chosenAttack.attackName + " and does " + myAttack.chosenAttack.attackDamage + " damage"); 
    }

    private IEnumerator TimeForAction()
    {
        if(actionStarted)
            yield break;
        

        actionStarted = true;
        //animate the enemy near the hero to attack
        Vector3 enemyPosition = new Vector3(heroToAttack.transform.position.x+1.5f, heroToAttack.transform.position.y, heroToAttack.transform.position.z);
        while (MoveTowardsEnemy(enemyPosition)) {yield return null;}

        //wait a bit
        yield return new WaitForSeconds(1.5f);

        //do damage
        doDamage();

        //animate back to start position
        Vector3 firstPosition = startPosition;
        while (MoveTowardsStart(firstPosition)) { yield return null; }

        //remove this perfromer from the list in BSM
        BSM.PerformList.RemoveAt(0);

        //reset the BSM -> wait
        BSM.performStates = BattleStateMachine.PerformAction.WAIT; 
        //end coroutine
        actionStarted = false;
        //reset the enemy state
        curCooldown = 0f;
        currentState = TurnState.PROCESSING; 
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        if (!dead)
        {
            setWalking();   //set animation paramter for Walking to TRUE
            return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
        }
        else
            return false;
    }

    private bool MoveTowardsStart(Vector3 target)
    {
        setIdle();      //set animation paramter for Idle to TRUE
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    void doDamage()
    {
        float calc_damage = enemy.curATK + BSM.PerformList[0].chosenAttack.attackDamage;  //enemy current attack + the chosen attack damage
        heroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage);
    }

    public void TakeDamage(float getDamageAmount)
    {
        enemy.curHP -= getDamageAmount;
        if (enemy.curHP <= 0)
        {
            currentState = TurnState.DEAD;
        }

        if (enemy.curHP < enemy.baseHP)
        {
            ProgressBar.transform.localScale = 
                new Vector3(Mathf.Clamp(enemy.curHP / enemy.baseHP, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
            if (this.enemy.curHP <= 0)
            {
                ProgressBar.transform.localScale =
                    new Vector3(Mathf.Clamp(0, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
            }
        }
    }

    private void setAttacking()
    {
        anim.SetBool("isAttacking", true);
        anim.SetBool("isWalking", false);
        anim.SetBool("isDead", false);
        anim.SetBool("isTakingDamage", false);
        anim.SetBool("isIdle", false);
    }

    private void setWalking()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", true);
        anim.SetBool("isDead", false);
        anim.SetBool("isTakingDamage", false);
        anim.SetBool("isIdle", false);
    }

    private void setDead()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isDead", true);
        anim.SetBool("isTakingDamage", false);
        anim.SetBool("isIdle", false);
    }

    private void setIdle()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isDead", false);
        anim.SetBool("isTakingDamage", false);
        anim.SetBool("isIdle", true);
    }

    public void setTakingDamage()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isDead", false);
        anim.SetBool("isTakingDamage", true);
        anim.SetBool("isIdle", true);
    }

    private void setAllAnimatorsFalse()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isDead", false);
        anim.SetBool("isTakingDamage", false);
        anim.SetBool("isIdle", true);
    }

}
