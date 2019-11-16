/*
 * FitWarriorsBattleProject
 * @Author: Dakota Ruhl
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack: MonoBehaviour
{
    public string attackName;
    public float attackDamage;      //Base Damage
    public float attackCost;        //Mana Cost
}
