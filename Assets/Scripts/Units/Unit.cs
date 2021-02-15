﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Unit : MonoBehaviour
{
    public RaceStat raceStats;
    public ClassStat classStat;

    private Race race;
    private Class clas;

    private int maxLife;
    [SerializeField] private int currentLife;
    private int maxStamina;
    private int currentStamina;
    private int incrementStamina;
    private int armor;
    private float moveSpeed;
    private float attackSpeed;
    private int damage;
    private int range;

    private bool moving;
    public Board board;
    private Vector3 worldPosition;
    public Vector3Int currentPosition;
    public Cell currentCell = null;
    private List<Cell> path = null;

    private float startPosX;
    private float startPosY;


    public Vector3Int initialPos;
    private Vector3Int targetPos;
    private Unit targetUnit = null;
    private int targetDistance = PathfindingTool.infVal;
    private bool hasTarget = false;
    private bool isActing = false;
    private bool isAbilityActivated = false;
    private string targetTag = "UnitAlly";

    [SerializeField] private string abilityName;

    private Ability ability;

    public HealthbarHandler healthBar;


    // Start is called before the first frame update
    void Start()
    {
        race = raceStats.race;
        clas = classStat.clas;

        maxLife = raceStats.maxLife + classStat.maxLife;
        currentLife = maxLife;
        maxStamina = 10/*raceStats.maxMana + classStat.maxMana*/;
        currentStamina = 0 /*raceStats.mana + classStat.mana*/;
        incrementStamina = 1;
        armor = raceStats.armor + classStat.armor;
        moveSpeed = raceStats.moveSpeed + classStat.moveSpeed;
        attackSpeed = raceStats.attackSpeed + classStat.attackSpeed;
        damage = raceStats.damage + classStat.damage;
        range = classStat.range;

        healthBar.SetHealth(maxLife, currentLife);

        //canAttack = true;
        hasTarget = false;
        isActing = false;

        setPosition(board.GetCell(new Vector3Int(initialPos.x, initialPos.y, 0)));

        //if the unit is an ally unit
        if (CompareTag("UnitAlly"))
            //it should target enemy units
            targetTag = "UnitEnemy";

        if(abilityName != null && abilityName != "")
        {
            var abilityType = System.Type.GetType(abilityName);
            gameObject.AddComponent(abilityType);
            ability = gameObject.GetComponent<Ability>();
            ability.setUnit(this);
        }

        GameManager.instance.AddUnit(this);

    }

    public void UpdateUnit()
    {
        //findTarget();
        checkDeath();

        /*if (!hasTarget)
        {
            findTarget();
        }*/

        //if the unit is not following a path yet and has a target cell different from their current cell
        if(isAbilityActivated && abilityName != null && abilityName != "")
        {
            ability.castAbility();
        }

        if (!isActing)
        {
            //if the target is not yet in range of attack
            if (targetDistance > range && targetUnit != null && path != null)
            {
                //start the coroutine to move to the next cell of the path
                StartCoroutine(MoveToCell(path[0]));
            }
            else if(targetDistance <= range && targetUnit != null && path != null)
            {
                //start the coroutine to attack the target
                StartCoroutine(AttackTarget());
            }

            //check if a better target can be selected
            path = PathfindingTool.findTarget(board, currentCell, targetTag);
            if(path != null)
            {
                targetDistance = path.Count;
                hasTarget = true;
                targetPos = path[targetDistance-1].TileMapPosition;
                targetUnit = path[targetDistance-1].GetCurrentUnit();

            }
        }
    }

    //follow a path represented by a list of cells to cross
    IEnumerator MoveFollowingPath(List<Cell> path)
    {
        isActing = true;
        foreach (Cell cell in path)
        {
            yield return new WaitForSeconds(moveSpeed);
            setPosition(cell);
        }
        isActing = false;
    }

    //move to a given cell
    IEnumerator MoveToCell(Cell cell)
    {
        if (cell.GetIsOccupied() == false)
        {
            isActing = true;
            setPosition(cell);
            yield return new WaitForSeconds(moveSpeed);
            isActing = false;
        }
    }

    //attack the current target
    IEnumerator AttackTarget()
    {
        isActing = true;

        StartCoroutine(AttackAnimation());

        targetUnit.takeDamage(damage);

        if (abilityName != null && abilityName != "")
        {
            ability.updateAbility(incrementStamina);
        }

        yield return new WaitForSeconds(attackSpeed);
        isActing = false;
    }

    //move the sprite toward the target for a short time
    IEnumerator AttackAnimation()
    {
        //get the position where the sprite will be placed during the attack
        Vector3 targetWorldPosition = targetUnit.getCell().WorldPosition;
        Vector3 attackDirection = targetWorldPosition - worldPosition;
        attackDirection.Normalize();
        attackDirection /= 8;

        transform.position = worldPosition + attackDirection;

        //set the animation time to be fast, and faster than the attack speed
        float animationTime = 0.2f;
        if (attackSpeed <= 0.2f)
        {
            animationTime = attackSpeed / 2;
        }

        yield return new WaitForSeconds(0.2f);

        //place the sprite back to its original position
        transform.position = worldPosition;
    }

    //set the position of the unit in a cell
    public void setPosition(Cell cell)
    {
        if (currentCell != null)
        {
            currentCell.SetIsOccupied(false);
            currentCell.SetCurrentUnit(null);
        }

        currentCell = cell;

        currentCell.SetIsOccupied(true);
        currentCell.SetCurrentUnit(this);

        currentPosition = currentCell.TileMapPosition;
        worldPosition = new Vector3(currentCell.WorldPosition.x, currentCell.WorldPosition.y, 0);
        transform.position = worldPosition;
    }

    public Vector3Int getPosition()
    {
        return (currentPosition);
    }

    public Cell getCell()
    {
        return currentCell;
    }

    private void checkDeath()
    {
        if(currentLife <= 0)
        {
            currentCell.SetIsOccupied(false);
            Destroy(this.gameObject);
        }
    }

    public void takeDamage(int damage)
    {
        currentLife -= damage;
        healthBar.SetHealth(currentLife);
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            if (targetUnit == null || currentCell == null || hasTarget == false)
                return;

            Gizmos.DrawLine(transform.position, targetUnit.transform.position);
        }
    }
    public void UpdateDragDrop()
    {
        if(moving)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);
        }
    }

    private void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;

            moving = true;
        }
    }

    private void OnMouseUp()
    {
        moving = false;
    }

    private void OnDestroy()
    {
        GameManager.instance.RemoveUnit(this);
    }

    public string getTargetTag()
    {
        return targetTag;
    }

    public void setIsAbilityActivated(bool b)
    {
        isAbilityActivated = b;
    }

    public bool getIsAbilityActivated()
    {
        return isAbilityActivated;
    }
}
