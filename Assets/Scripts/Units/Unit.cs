using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Unit : MonoBehaviour
{
    public RaceStat raceStats;
    public ClassStat classStat;

    private static int infVal = 1000;
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
    [SerializeField] private string unitName;

    private bool moving;
    private bool canMove;
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
    private bool isMoving = false;

    private SpriteRenderer spriteRenderer;
    private Color baseColor, damageColor;
    private int takingDamageCount = 0;
    private bool isAbilityActivated = false;
    private string targetTag = "UnitAlly";

    [SerializeField] private string abilityName;

    private Ability ability;

    public HealthbarHandler healthBar;

    public GameObject projectileGameObject;

    // Start is called before the first frame update
    void Start()
    {
        //get all RaceStat and ClassStat scriptableObject
        RaceStat[] races = (RaceStat[]) Resources.FindObjectsOfTypeAll(typeof(RaceStat));
        ClassStat[] classes = (ClassStat[]) Resources.FindObjectsOfTypeAll(typeof(ClassStat));


        //select a random RaceStat
        int randomRaceIndex = Random.Range(0, races.Length);
        raceStats = races[randomRaceIndex];

        //select a random ClassStat
        int randomClassIndex = Random.Range(0, classes.Length);
        classStat = classes[randomClassIndex];

        string[] possibleNames = raceStats.unitNames;
        int randomNameIndex = Random.Range(0, possibleNames.Length);
        unitName = possibleNames[randomNameIndex];

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

        healthBar.SetHealth(currentLife, maxLife);
        healthBar.SetStamina(currentStamina, maxStamina);

        //canAttack = true;
        hasTarget = false;
        isActing = false;

        Cell startCell = board.GetCell(new Vector3Int(initialPos.x, initialPos.y, 0));
        occupyNewCell(startCell);
        setPosition(startCell);

        //if the unit is an ally unit
        if (CompareTag("UnitAlly"))
            //it should target enemy units
            targetTag = "UnitEnemy";

        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
        damageColor = new Color(1, 0.6f, 0.6f);

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
        canMove = false;

        if (path == null && !isActing)
        {
            StartCoroutine(UnitWaitForSeconds(moveSpeed));
        }

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

                print(targetUnit);
            }
        }
    }

    IEnumerator UnitWaitForSeconds(float seconds)
    {
        isActing = true;
        yield return new WaitForSeconds(seconds);
        isActing = false;
        path = PathfindingTool.findTarget(board, currentCell, targetTag);        
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
        if (!cell.GetIsOccupied())
        {
            isActing = true;
            //change the occupied tile then start the animation
            occupyNewCell(cell);
            //StopCoroutine(MoveAnimation(cell));
            StartCoroutine(MoveAnimation(cell));
            yield return new WaitForSeconds(moveSpeed);
            //ensure that the unit is at the center of the current cell before it can start acting again
            setPosition(cell);
            isActing = false;
        }

        else
        {
            path = null;
            yield return new WaitForSeconds(moveSpeed);
        }
    }

    IEnumerator MoveAnimation(Cell cell)
    {
        //set the speed of the animation (distance at each iteration of while loop)
        float speed = 0.1f;

        //if the unit is moving too fast, inscrease the animation speed
        if (moveSpeed <= 0.2)
            speed = 0.2f;

        //set the maximum number of movement in the animation
        float maxRefresh = 1 / speed;

        //time to wait between every movement
        float refreshRate = 0.01f;

        int numberOfRefresh = 0;

        Vector3 distance = cell.WorldPosition - transform.position;

        //while the unit is not arrived at the target position and the max number of movement is not reached
        while (cell == currentCell && transform.position != cell.WorldPosition && numberOfRefresh <= maxRefresh)
        {
            transform.position += distance * speed;
            numberOfRefresh++;
            yield return new WaitForSeconds(refreshRate);
        }

        setPosition(cell);

        yield return null;
    }

    //attack the current target
    IEnumerator AttackTarget()
    {
        isActing = true;

        StartCoroutine(AttackAnimation());

        targetUnit.takeDamage(damage);

        if (range > 1)
            StartCoroutine(ProjectileAnimation());

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
        float animationTime = 0.3f;
        if (attackSpeed <= 0.3f)
        {
            animationTime = 2 * attackSpeed / 3;
        }

        yield return new WaitForSeconds(0.2f);

        //place the sprite back to its original position
        transform.position = worldPosition;
    }

    IEnumerator ProjectileAnimation()
    {
        GameObject projectile = Instantiate(projectileGameObject, worldPosition, Quaternion.identity, transform);

        //set the speed of the animation (distance at each iteration of while loop)
        float speed = 0.06f;

        //if the unit is attacking too fast, inscrease the animation speed
        if (attackSpeed <= 0.2f)
            speed = 0.15f;

        //set the maximum number of refresh of the projectile animation
        float maxRefresh = 1 / speed;

        //time to wait between every movement
        float refreshRate = 0.01f;

        int numberOfRefresh = 0;

        Vector3 distance = targetUnit.transform.position - projectile.transform.position;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //while there is still a target, the projectile is not arrived at the target position and the max number of refresh is not reached
        while (targetUnit != null && projectile.transform.position != targetUnit.transform.position && numberOfRefresh <= maxRefresh)
        {
            projectile.transform.position += distance * speed;
            numberOfRefresh++;
            yield return new WaitForSeconds(refreshRate);
        }

        Destroy(projectile);

        yield return null;
    }

    private void occupyNewCell(Cell newCell)
    {
        if (currentCell != null)
        {
            currentCell.DecreaseNumberOfUnits();
        }

        currentCell = newCell;
        currentCell.IncreaseNumberOfUnits();
        currentCell.SetCurrentUnit(this);
    }

    //set the position of the unit in a cell
    public void setPosition(Cell cell)
    {
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
            currentCell.DecreaseNumberOfUnits();
            Destroy(this.gameObject);
        }
    }

    public void takeDamage(int damage)
    {
        currentLife -= damage;
        checkDeath();
        healthBar.SetHealth(currentLife);
        StartCoroutine(TakeDamageAnimation());
    }

    public void heal(int heal)
    {
        currentLife += heal;
        if(currentLife > maxLife)
        {
            currentLife = maxLife;
        }
        healthBar.SetHealth(currentLife);        
    }

    IEnumerator TakeDamageAnimation()
    {
        spriteRenderer.color = damageColor;
        takingDamageCount++;

        yield return new WaitForSeconds(0.4f);

        //if this is the last animation playing, set the color back to normal
        takingDamageCount--;
        if(takingDamageCount == 0)
            spriteRenderer.color = baseColor;
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
        canMove = true;
        if (moving)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);
        }
    }

    private void OnMouseDown()
    {
        if(canMove /*&& CompareTag("UnitAlly")*/)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                startPosX = mousePos.x - this.transform.localPosition.x;
                startPosY = mousePos.y - this.transform.localPosition.y;

                moving = true;
            }
        }
    }

    private void OnMouseUp()
    {
        if(canMove /*&& CompareTag("UnitAlly")*/)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3Int tileCoordinate = board.GetTilemap().WorldToCell(mousePos);

            if (board.GetCell(tileCoordinate) == null || board.GetCell(tileCoordinate).GetIsOccupied() == true)
                setPosition(board.GetCell(currentPosition));
            else
            {
                Cell newCell = board.GetCell(tileCoordinate);
                occupyNewCell(newCell);
                setPosition(newCell);
            }

            moving = false;
        }

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

    public int getRange()
    {
        return range;
    }
    public void setRange(int range)
    {
        this.range = range;
    }

    public int getCurrentLife()
    {
        return currentLife;
    }
}
