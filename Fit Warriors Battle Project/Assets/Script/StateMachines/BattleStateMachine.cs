/*
 * FitWarriorsBattleProject
 * @Author: Dakota Ruhl
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour {

    public HeroStateMachine HSM;
    public PerformAction performStates;
    public List<TurnHandler> PerformList = new List<TurnHandler>();    //list of turnhandler classes
    public List<GameObject> HeroesInBattle = new List<GameObject>();   //List for expansion to multiple heroes/enemies
    public List<GameObject> EnemysInBattle = new List<GameObject>();   // ^^

    //maybe public GameObject HeroInGame = new GameObject;
    //      pulbic GameObject EnemyInGame = new GameObject;

    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }

    public enum HeroGUI
    {
        ACTIVATE,
        WAIITING,
        INPUT1,
        INPUT2,
        DONE
    }

    public HeroGUI HeroInput;
    public List<GameObject> HerosToManage = new List<GameObject>();
    private TurnHandler heroChoice;

    public GameObject enemyButton;
    public Transform Spacer;

    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;

    public bool turn; 

    [System.Obsolete]
    void Start ()
    {
        performStates = PerformAction.WAIT;
        EnemysInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        HeroInput = HeroGUI.ACTIVATE;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);

        EnemyButtons();
    }
	
	// Update is called once per frame
	void Update () {
		switch (performStates)
        {
            case (PerformAction.WAIT):
                if (PerformList.Count > 0)                      //if perform list is populated move to take action
                    performStates = PerformAction.TAKEACTION;
                break;
            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(PerformList[0].Attacker); //give name to performer
                if (PerformList[0].Type == "Enemy")
                { 
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.heroToAttack = PerformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION; //load in the ACTION turn state - starts coroutine. 
                }

                if (PerformList[0].Type == "Hero")
                { 
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                    HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                    HSM.currentState = HeroStateMachine.TurnState.ACTION;
                    turn = true;
                }

                performStates = PerformAction.PERFORMACTION;
                break;
            case (PerformAction.PERFORMACTION):
                //idle state
                break;
        }

        switch (HeroInput)
        {
            case (HeroGUI.ACTIVATE):
                if(HerosToManage.Count > 0)
                {
                    HerosToManage[0].transform.Find("Selector").gameObject.SetActive(false); //hero selector for more than 1 hero
                    heroChoice = new TurnHandler();

                    AttackPanel.SetActive(true);
                    HeroInput = HeroGUI.WAIITING;
                }
                break;
            case (HeroGUI.WAIITING):
                break;
            case (HeroGUI.DONE):
                HeroInputDone();
                break;
        }
	}

    public void CollectActions(TurnHandler input)
    {
        PerformList.Add(input); 
    }

    [System.Obsolete]
    void EnemyButtons()
    {
        foreach(GameObject enemy in EnemysInBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();

            Text buttonText = newButton.transform.FindChild("Text").gameObject.GetComponent<Text>();
            buttonText.text = cur_enemy.enemy.theName;

            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(Spacer, false); 
        }
    }

    public void Input1() //attack button
    {
        heroChoice.Attacker = HerosToManage[0].name;
        heroChoice.AttacksGameObject = HerosToManage[0];
        heroChoice.Type = "Hero";

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);  //for more than one enemy 

    }

    public void Input2(GameObject chosenEnemy) //Enemy selection
    {
        heroChoice.AttackersTarget = chosenEnemy;
        HeroInput = HeroGUI.DONE;
    }

    void HeroInputDone()
    {
        PerformList.Add(heroChoice);
        EnemySelectPanel.SetActive(false);
        HerosToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HerosToManage.RemoveAt(0);
        HeroInput = HeroGUI.ACTIVATE;
    }
}
