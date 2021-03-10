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
        initDefinition();
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

    private void initDefinition()
    {
        switch(r)
        {
            case Race.Orc:
                def = "Extrem brutality\nSpell\nOrc's attack ignore defense but they lose accuracy\n (2) 5 secondes, -10% accuracy";
                break;

            case Race.Skeleton:
                def = "Deads world\nSpell\nEnnemis loose moovespeed for 5 seconds\n (2) -10% moovespeed";
                break;

            case Race.Octopus:
                def = "Sprawling cage\nSpell\nStun the target\n (2) 5 seconds";
                break;

            case Race.Elemental:
                def = "Fusion of elements\nSpell\nIncrease the target's attack by number of elemental on board\n (2) 2 attack per elemental";
                break;

            case Race.Giant:
                def = "Titanic impact\nSpell\nChoose a giant unit, his next attack will be powerful and stun the enemy for 2 seconds\n (2) + 15 damages";
                break;
        }
        
    }
}
