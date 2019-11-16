using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectButton : MonoBehaviour
{
    public GameObject EnemyPrefab; 

    public void SelectEnemy()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine> ().Input2(EnemyPrefab); //save input of the enemy prefab
    }

    public void hideSelector()
    {
        EnemyPrefab.transform.Find("Selector").gameObject.SetActive(false); 
    }

    public void showSelector()
    {
        EnemyPrefab.transform.Find("Selector").gameObject.SetActive(true);
    }
}
