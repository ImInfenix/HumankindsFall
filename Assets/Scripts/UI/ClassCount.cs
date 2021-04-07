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
        initDefinition(0);
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

    public void initDefinition(int lvl)
    {
        switch (c)
        {
            case Class.Mage:
                if(lvl==0)
                {
                    def = "Mages start the combat with more stamina\n (2) 25% of their max stamina\n (4) 50% of their max stamina";
                }
                if(lvl==1)
                {
                    def = "Mages start the combat with more stamina\n <b>(2) 25% of their max stamina</b>\n (4) 50% of their max stamina";
                }
                if(lvl==2)
                {
                    def = "Mages start the combat with more stamina\n (2) 25% of their max stamina\n <b>(4) 50% of their max stamina</b>";
                }               
                break;

            case Class.Warrior:
                if (lvl == 0)
                {
                    def = "Warriors up their attack\n (2) +15% attack\n (4) +30% attack";
                }
                if (lvl == 1)
                {
                    def = "Warriors up their attack\n <b>(2) +15% attack</b>\n (4) +30% attack";
                }
                if (lvl == 2)
                {
                    def = "Warriors up their attack\n (2) +15% attack\n <b>(4) +30% attack</b>";
                }                
                break;

            case Class.Tank:
                if (lvl == 0)
                {
                    def = "Tanks increase their max health\n (2) 40% max health";
                }
                if (lvl == 1)
                {
                    def = "Tanks increase their max health\n <b>(2) +40% max health</b>";
                }
                break;

            case Class.Bowman:
                if (lvl == 0)
                {
                    def = "Bowmans increase their attack speed\n (2) +20% attack speed";
                }
                if (lvl == 1)
                {
                    def = "Bowmans increase their attack speed\n <b>(2) +20% attack speed</b>";
                }
                break;

            case Class.Healer:
                if (lvl == 0)
                {
                    def = "Healers heals the ally with the lowest health for 10% of his max health\n (1) Every 5 attacks\n (2) Every 3 attacks\n (3) Every 2 attacks";
                }
                if (lvl == 1)
                {
                    def = "Healers heals the ally with the lowest health for 10% of his max health\n <b>(1) Every 5 attacks</b>\n (2) Every 3 attacks\n (3) Every 2 attacks";
                }
                if (lvl == 2)
                {
                    def = "Healers heals the ally with the lowest health for 10% of his max health\n (1) Every 5 attacks\n <b>(2) Every 3 attacks</b>\n (3) Every 2 attacks";
                }
                if (lvl == 3)
                {
                    def = "Healers heals the ally with the lowest health for 10% of his max health\n (1) Every 5 attacks\n (2) Every 3 attacks\n <b>(3) Every 2 attacks</b>";
                }                
                break;

            case Class.Support:
                if (lvl == 0)
                {
                    def = "Units near supports gain armor\n (2) 10% armor\n (3) 20% armor\n (4) 40% armor";
                }
                if (lvl == 1)
                {
                    def = "Units near supports gain armor\n <b>(2) 10% armor</b>\n (3) 20% armor\n (4) 40% armor";
                }
                if (lvl == 2)
                {
                    def = "Units near supports gain armor\n (2) 10% armor\n <b>(3) 20% armor</b>\n (4) 40% armor";
                }
                if (lvl == 3)
                {
                    def = "Units near supports gain armor\n (2) 10% armor\n (3) 20% armor\n <b>(4) 40% armor</b>";
                }                
                break;

            case Class.Berserker:
                if (lvl == 0)
                {
                    def = "Berserkers gain attack and armor when their is no unit around\n (2) +20% attack, +10% armor";
                }
                if (lvl == 1)
                {
                    def = "Berserkers gain attack and armor when their is no unit around\n <b>(2) +20% attack, +10% armor</b>";
                }                
                break;

            case Class.Assassin:
                if (lvl == 0)
                {
                    def = "At start of combat, assassins gain invisibility for 5 seconds\n (1) Their first attack is 30% stronger";
                }
                if (lvl == 1)
                {
                    def = "At start of combat, assassins gain invisibility for 5 seconds\n <b>(1) Their first attack is 30% stronger</b>";
                }               
                break;
        }

    }

}
