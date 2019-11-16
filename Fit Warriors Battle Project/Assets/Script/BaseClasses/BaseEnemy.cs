/*
 * FitWarriorsBattleProject
 * @Author: Dakota Ruhl
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy: BaseClass
{

    public int level;
    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        LEGENDARY
    }

    public Rarity rarity; 
}
