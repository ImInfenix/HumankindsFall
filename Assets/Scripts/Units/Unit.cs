using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Unit : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public RaceStat raceStats;
    public ClassStat classStat;

    private Transform thisTransform;

    private Race race;
    private Class clas;

    private int maxLife;
    private int life;
    private int maxMana;
    private int mana;
    private int armor;
    private int moveSpeed;
    private float attackSpeed;
    private int damage;
    private int range;


    // Start is called before the first frame update
    void Start()
    {
        race = raceStats.race;
        clas = classStat.clas;
        
        maxLife = raceStats.maxLife + classStat.maxLife;
        life = maxLife;
        maxMana = raceStats.maxMana + classStat.maxMana;
        mana = raceStats.mana + classStat.mana;
        armor = raceStats.armor + classStat.armor;
        moveSpeed = raceStats.moveSpeed + classStat.moveSpeed;
        attackSpeed = raceStats.attackSpeed + classStat.attackSpeed;
        damage = raceStats.damage + classStat.damage;
        range = classStat.range;

        thisTransform = transform;
    }

    protected virtual void move()
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
