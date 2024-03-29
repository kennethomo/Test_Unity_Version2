﻿/*
 * FitWarriorsBattleProject
 * @Author: Dakota Ruhl
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurnHandler {

    public string Attacker;                     //name of attacker
    public string Type; 
    public GameObject AttacksGameObject;        //who is attacking
    public GameObject AttackersTarget;          //who is targeted for attack


    //which attack is performed
    public BaseAttack chosenAttack;
}
