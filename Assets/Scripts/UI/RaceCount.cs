using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class to create a hashmap<Race, int>
public class RaceCount
{
    private Race r;
    private int number;
    private string str;

    public RaceCount(Race race, int n, string s)
    {
        r = race;
        number = n;
        str = s;
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
        return str;
    }

    public void setNumber(int n)
    {
        number = n;
    }
}
