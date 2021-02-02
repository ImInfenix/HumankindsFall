using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class WarriorOrc_Stat : MonoBehaviour
    {
        private static int 
            maxLife = 100,
            maxMana = 100,
            armor = 100,
            moveSpeed = 25,
            damage = 10;

       private static float attackSpeed;

        public static int MaxLife { get => maxLife; set => maxLife = value; }
        public static int MaxMana { get => maxMana; set => maxMana = value; }
        public static int Armor { get => armor; set => armor = value; }
        public static int MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public static int Damage { get => damage; set => damage = value; }


        public static float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }

    }
}

