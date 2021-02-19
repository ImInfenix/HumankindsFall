using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to create a hashmap<Class, int>
public class ClassCount
{
    private Class c;
    private int number;
    private string str;

    public ClassCount(Class classe, int n, string s)
    {
        c = classe;
        number = n;
        str = s;
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
        return str;
    }

}
