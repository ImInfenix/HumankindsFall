using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class to create a hashmap<Race, int>
public class RaceCount
{
    private Race r;
    private int number;
    private string def;

    public RaceCount(Race race, int n)
    {
        r = race;
        number = n;
        initDefinition(0);
    }

    public int getNumber()
    {
        return number;
    }

    public Race getRace()
    {
        return r;
    }

    public string getString()
    {
        return def;
    }

    public void setNumber(int n)
    {
        number = n;
    }

    public void initDefinition(int lvl)
    {
        switch(r)
        {
            case Race.Orc:
                if(lvl == 0)
                {
                    def = "Extrem brutality\n<b>Spell</b>\nOrc's attack ignore 30% of defense but they lose accuracy\n (2) 5 secondes, -10% accuracy";
                }
               if(lvl == 1)
                {
                    def = "Extrem brutality\n<b>Spell</b>\nOrc's attack ignore 30% of defense but they lose accuracy\n <b>(2) 5 secondes, -10% accuracy</b>";
                }
                break;

            case Race.Skeleton:
                if (lvl == 0)
                {
                    def = "Deads world\n<b>Spell</b>\nEnnemis loose armor for 5 seconds\n (2) -25% armor";
                }
                if (lvl == 1)
                {
                    def = "Deads world\n<b>Spell</b>\nEnnemis loose armor for 5 seconds\n <b>(2) -25% armor</b>";
                }                
                break;

            case Race.Octopus:
                if (lvl == 0)
                {
                    def = "Sprawling cage\n<b>Spell</b>\nStun the target\n (2) 5 seconds";
                }
                if (lvl == 1)
                {
                    def = "Sprawling cage\n<b>Spell</b>\nStun the target\n <b>(2) 5 seconds</b>";
                }                
                break;

            case Race.Elemental:
                if (lvl == 0)
                {
                    def = "Fusion of elements\n<b>Spell</b>\nDeal damage on enemy target by number of elemental on board\n (2) 10 attack per elemental";
                }
                if (lvl == 1)
                {
                    def = "Fusion of elements\n<b>Spell</b>\nDeal damage on enemy target by number of elemental on board\n <b>(2) 10 attack per elemental</b>";
                }
                break;

            case Race.Giant:
                if (lvl == 0)
                {
                    def = "Titanic impact\n<b>Spell</b>\nChoose a giant unit, his next attack will be powerful and stun the enemy for 2 seconds\n (2) + 15% damages";
                }
                if (lvl == 1)
                {
                    def = "Titanic impact\n<b>Spell</b>\nChoose a giant unit, his next attack will be powerful and stun the enemy for 2 seconds\n <b>(2) + 15% damages</b>";
                }                
                break;

            case Race.Ratman:
                if (lvl == 0)
                {
                    def = "Poisonous bite\n<b>Spell</b>\nThe next attack of all ratmen poisons the enemy for 5 seconds\n (2) 2 damage /seconde";
                }
                if (lvl == 1)
                {
                    def = "Poisonous bite\n<b>Spell</b>\nThe next attack of all ratmen poisons the enemy for 5 seconds\n <b>(2) 2 damage /seconde</b>";
                }
                break;

            case Race.Demon:
                if (lvl == 0)
                {
                    def = "Finally I'm complete\n<b>Spell</b>\nSummon the demon king on the target cell, he has less life but more damage\n (3) -50% hp, + 30% damages";
                }
                if (lvl == 1)
                {
                    def = "Finally I'm complete\n<b>Spell</b>\nSummon the demon king on the target cell, he has less life but more damage\n <b>(3) -50% hp, + 30% damages</b>";
                }
                break;
        }
        
    }
}
