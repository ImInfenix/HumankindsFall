using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    public enum Race
    {
        Orc,
        Humans
    }

    public enum Class
    {
        Warrior,
        Mage
    }

    protected Race race;
    protected Class clas;
    // protected Object attack; A voir comment on implémente les attaques

    protected int maxLife;
    protected int life;
    protected int maxMana;
    protected int mana;
    protected int armor;
    protected int moveSpeed;
    protected float attackSpeed;
    protected int damage;

    private Transform thisTransform;


    // Start is called before the first frame update
    void Start()
    {
        thisTransform = transform;
    }

    protected virtual void move()
    {

    }

}
