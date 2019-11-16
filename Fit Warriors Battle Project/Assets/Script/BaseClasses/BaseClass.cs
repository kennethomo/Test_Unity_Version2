/*
 * FitWarriorsBattleProject
 * @Author: Dakota Ruhl
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseClass
{
    public string theName;

    public float baseHP;        //hp
    public float curHP;

    public float baseMP;        //mp
    public float curMP; 

    public float baseATK;       //attack
    public float curATK;

    public float baseDEF;       //defence
    public float curDEF;

    public List<BaseAttack> attacks = new List<BaseAttack>();

}
