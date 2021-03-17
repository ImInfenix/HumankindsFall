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
                    def = "Extrem brutality\n<b>Spell</b>\nOrc's attack ignore defense but they lose accuracy\n (2) 5 secondes, -10% accuracy";
                }
               if(lvl == 1)
                {
                    def = "Extrem brutality\n<b>Spell</b>\nOrc's attack ignore defense but they lose accuracy\n <b>(2) 5 secondes, -10% accuracy</b>";
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
                    def = "Titanic impact\n<b>Spell</b>\nChoose a giant unit, his next attack will be powerful and stun the enemy for 2 seconds\n (2) + 15 damages";
                }
                if (lvl == 1)
                {
                    def = "Titanic impact\n<b>Spell</b>\nChoose a giant unit, his next attack will be powerful and stun the enemy for 2 seconds\n <b>(2) + 15 damages</b>";
                }                
                break;
        }
        
    }
}
