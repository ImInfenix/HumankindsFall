using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to create a hashmap<Class, int>
public class ClassCount
{
    private Class c;
    private int number;
    private string def;

    public ClassCount(Class classe, int n)
    {
        c = classe;
        number = n;
        initDefinition();
    }

    public int getNumber()
    {
        return number;
    }

    public void setNumber(int n)
    {
        number = n;
    }

    public Class getClass()
    {
        return c;
    }

    public string getString()
    {
        return def;
    }

    private void initDefinition()
    {
        switch (c)
        {
            case Class.Mage:
                def = "Mages start the combat with more stamina\n (2) 25% of their max stamina\n (4) 50% of their max stamina";
                break;

            case Class.Warrior:
                def = "Warriors up their attack\n (2) +10% attack\n (4) +20% attack";
                break;

            case Class.Tank:
                def = "Tanks increase their max health\n (2) +25% max health";
                break;

            case Class.Bowman:
                def = "Bowmans increase their attack speed\n (2) +25% attack speed";
                break;

            case Class.Healer:
                def = "Healers heals the ally with the lowest health for 15% of his max health\n (1) Every 5 attacks\n (2) Every 3 attacks\n (3) Every 2 attacks";
                break;

            case Class.Support:
                def = "Units near supports gain 20% armor\n (2) 1 case radius\n (3) 2 cases radius\n (3) 3 cases radius";
                break;

            case Class.Berserker:
                def = "Berserkers gain attack and armor when their is no unit around\n (2) +15% attack, +10% armor";
                break;

            case Class.Assassin:
                def = "At start of combat, assassins jump on the farthest unit\n (1) They gain invisibility for 5 seconds at start of combat";
                break;
        }

    }

}
