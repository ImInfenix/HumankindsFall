using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorOrc : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        race = Race.Orc;
        clas = Class.Warrior;
        maxLife = Stats.WarriorOrc_Stat.MaxLife;
        life = maxLife;
        maxMana = Stats.WarriorOrc_Stat.MaxMana;
        mana = 0;
        armor = Stats.WarriorOrc_Stat.Armor;
        moveSpeed = Stats.WarriorOrc_Stat.MoveSpeed;
        damage = Stats.WarriorOrc_Stat.Damage;
        attackSpeed = Stats.WarriorOrc_Stat.AttackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
