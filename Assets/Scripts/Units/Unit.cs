﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public const string ennemyTag = "UnitEnemy";
    public const string allyTag = "UnitAlly";

    public RaceStat raceStats;
    public ClassStat classStat;

    private int maxLife;
    [SerializeField] private int currentLife;
    private int incrementStamina;
    private int armor;
    private float moveSpeed;
    private float attackSpeed;
    private int damage;
    private int range;
    private int power;
    private string unitName;

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
    //private Vector3Int targetPos;
    private Unit targetUnit = null;
    //private int targetDistance = PathfindingTool.infVal;
    //private bool hasTarget = false;
    private bool isActing = false;
    private bool isMoving = false;

    private SpriteRenderer spriteRenderer;
    private Color baseColor, damageColor, healColor;
    private int takingDamageCount = 0;
    private bool isAbilityActivated = false;
    private string targetTag;

    public bool isRandomUnit = true;

    [SerializeField] private string abilityName;

    private Ability ability;

    public uint id;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    [SerializeField] private HealthbarHandler healthBar;
    [SerializeField] private Image classIcon;
    [SerializeField] private SpriteRenderer circleSprite;
    private GameObject projectileGameObject;

    private List<Gem> gems = new List<Gem>();

    public int MaxLife { get => maxLife; set => maxLife = value; }
    public int CurrentLife { get => currentLife; set => currentLife = value; }
    public int IncrementStamina { get => incrementStamina; set => incrementStamina = value; }
    public int Armor { get => armor; set => armor = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Damage { get => damage; set => damage = value; }
    public int Range { get => range; set => range = value; }
    public int Power { get => power; set => power = value; }

    // Start is called before the first frame update
    void Start()
    {
        if(isRandomUnit)
            GenerateRaceAndClass();

        InitializeUnit();

        GameManager.instance.AddUnit(this);
    }

    public void InitializeUnit()
    {
        //if the ability name exists
        if (abilityName != null && abilityName != "")
        {
            //load the ability class and add a component to the unit
            var abilityType = System.Type.GetType(abilityName);
            gameObject.AddComponent(abilityType);
            ability = gameObject.GetComponent<Ability>();
            ability.setUnit(this);
        }

        else
            healthBar.HideStaminaBar();

        if (classStat.classIconSprite == null)
            healthBar.HideClassIcon();

        MaxLife = raceStats.maxLife + classStat.maxLife;
        IncrementStamina = classStat.incrementStamina;
        Armor = raceStats.armor + classStat.armor;
        MoveSpeed = raceStats.moveSpeed + classStat.moveSpeed;
        AttackSpeed = raceStats.attackSpeed + classStat.attackSpeed;
        Damage = raceStats.damage + classStat.damage;
        Range = classStat.range;

        GetUnitGems();
        ApplyInitGemsEffect();

        CurrentLife = MaxLife;

        projectileGameObject = classStat.projectile;

        healthBar.SetHealth(CurrentLife, MaxLife);

        classIcon.sprite = classStat.classIconSprite;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (raceStats.race != Race.Human)
            spriteRenderer.sprite = raceStats.unitSprite;

        else
            spriteRenderer.sprite = Resources.Load<Sprite>("Textures/Unit Sprites/Humans/" + classStat.clas.ToString());

        //canAttack = true;
        //hasTarget = false;
        isActing = false;

        if (currentCell == null)
        {
            if (isRandomUnit && false)
            {
                Cell startCell = board.GetCell(new Vector3Int(initialPos.x, initialPos.y, 0));
                occupyNewCell(startCell);
                updatePosition();
            }

            else
            {
                Vector3Int tileCoordinate = board.GetTilemap().WorldToCell(transform.position);
                Cell newCell = board.GetCell(tileCoordinate);
                occupyNewCell(newCell);
                updatePosition();
            }
        }

        //if the unit is an ally unit
        if (CompareTag(allyTag))
        {
            //it should target enemy units
            targetTag = ennemyTag;
            circleSprite.color = new Color(0, 0, 1);
        }

        else
        {
            targetTag = allyTag;
            circleSprite.color = new Color(1, 0, 0);
        }

        baseColor = spriteRenderer.color;
        damageColor = new Color(1, 0.6f, 0.6f);
        healColor = new Color(0.4f, 1, 0.4f);
    }

    private void GetUnitGems()
    {
        foreach (Transform gemTransform in gameObject.transform.GetChild(2).transform)
        {
            Gem gemProv = gemTransform.GetComponent<Gem>();
            gems.Add(gemProv);
            gemProv.setUnit(this);
        }        
    }

    private void ApplyInitGemsEffect()
    {
        foreach (Gem gem in gems)
        {
            gem.InitGemEffect();
        }
    }

    private void GenerateRaceAndClass()
    {
        UnitDescription newDescription = UnitGenerator.GenerateUnit(tag);
        unitName = newDescription.GetUnitName();
        name = unitName;
        raceStats = newDescription.GetRace();
        classStat = newDescription.GetClass();
        abilityName = newDescription.GetAbilityName();
        id = newDescription.GetId();
    }

    public void UpdateUnit()
    {
        canMove = false;

        //if the unit is not following a path yet and has a target cell different from their current cell
        if (isAbilityActivated && abilityName != null && abilityName != "")
        {
            ability.castAbility();
        }

        if (!isActing)
        {
            List<Unit> listAvailableTarget = PathfindingTool.unitsInRadius(currentCell, Range, targetTag);
            
            //if the target is not yet in range of attack
            if (listAvailableTarget.Count > 0)
            {
                targetUnit = listAvailableTarget[0];
                StartCoroutine(AttackTarget());
            }

            else if (path != null && path.Count > 0)
            {
                //start the coroutine to move to the next cell of the path
                StartCoroutine(MoveToCell(path[0]));
            }

            else if (path == null)
            {
                StartCoroutine(UnitWaitForSeconds(moveSpeed));
            }

            path = PathfindingTool.createPathTarget(this);
        }
    }

    private void ApplyAttackGemsEffect()
    {
        foreach (Gem gem in gems)
        {
            gem.AttackGemEffect();
        }
    }

    IEnumerator UnitWaitForSeconds(float seconds)
    {
        isActing = true;
        yield return new WaitForSeconds(seconds);
        isActing = false;
        //path = PathfindingTool.findTarget(board, currentCell, targetTag);
    }

    //follow a path represented by a list of cells to cross
    IEnumerator MoveFollowingPath(List<Cell> path)
    {
        isActing = true;
        foreach (Cell cell in path)
        {
            yield return new WaitForSeconds(MoveSpeed);
            updatePosition();
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
            yield return new WaitForSeconds(MoveSpeed);
            //ensure that the unit is at the center of the current cell before it can start acting again
            updatePosition();
            isActing = false;
        }

        else
        {
            path = null;
            yield return new WaitForSeconds(MoveSpeed);
        }
    }

    IEnumerator MoveAnimation(Cell cell)
    {
        //set the speed of the animation (distance at each iteration of while loop)
        float speed = 0.1f;

        //if the unit is moving too fast, inscrease the animation speed
        if (MoveSpeed <= 0.2)
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

        updatePosition();

        yield return null;
    }

    //attack the current target
    IEnumerator AttackTarget()
    {
        isActing = true;

        StartCoroutine(AttackAnimation());

        targetUnit.takeDamage(Damage);

        ApplyAttackGemsEffect();

        if (Range > 1)
            StartCoroutine(ProjectileAnimation());

        if (abilityName != null && abilityName != "")
        {
            ability.updateAbility(IncrementStamina);
        }

        yield return new WaitForSeconds(AttackSpeed);
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
        if (AttackSpeed <= 0.3f)
        {
            animationTime = 2 * AttackSpeed / 3;
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
        if (AttackSpeed <= 0.2f)
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

    public void occupyNewCell(Cell newCell)
    {
        if (currentCell != null)
        {
            currentCell.DecreaseNumberOfUnits();
            currentCell.SetCurrentUnit(null);
        }

        currentCell = newCell;
        currentCell.IncreaseNumberOfUnits();
        currentCell.SetCurrentUnit(this);
    }

    //set the position of the unit in a cell
    public void updatePosition()
    {
        currentPosition = currentCell.TileMapPosition;
        worldPosition = new Vector3(currentCell.WorldPosition.x, currentCell.WorldPosition.y, 0);
        transform.position = worldPosition;
        //print(currentCell);
    }

    public Vector3Int getPosition()
    {
        return (currentPosition);
    }

    public Race getRace()
    {
        return raceStats.race;
    }

    public Class getClass()
    {
        return classStat.clas;
    }

    public Cell getCell()
    {
        return currentCell;
    }

    private void checkDeath()
    {
        if (CurrentLife <= 0)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    public void takeDamage(int damage)
    {
        CurrentLife -= damage;
        checkDeath();
        healthBar.SetHealth(CurrentLife);
        StartCoroutine(ChangeColorAnimation(damageColor, 0.4f));
    }

    public void heal(int heal)
    {
        CurrentLife += heal;
        if (CurrentLife > MaxLife)
        {
            CurrentLife = MaxLife;
        }
        healthBar.SetHealth(CurrentLife);
        StartCoroutine(ChangeColorAnimation(healColor, 0.7f));
    }

    IEnumerator ChangeColorAnimation(Color color, float durationSeconds)
    {
        spriteRenderer.color = color;
        takingDamageCount++;

        yield return new WaitForSeconds(durationSeconds);

        //if this is the last animation playing, set the color back to normal
        takingDamageCount--;
        if (takingDamageCount == 0)
            spriteRenderer.color = baseColor;
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            if (targetUnit == null || currentCell == null /*|| hasTarget == false*/)
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

            this.gameObject.transform.position = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);

            if (Input.GetMouseButtonUp(0))
                OnMouseUp();
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.gamestate != GameManager.GameState.Placement)
            return;

        if (CompareTag(allyTag))
        {
            if (Input.GetMouseButtonDown(0) && canMove)
            {
                PrepareForDragNDrop();
            }
        }

    }

    public void PrepareForDragNDrop()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        startPosX = mousePos.x - transform.position.x;
        startPosY = mousePos.y - transform.position.y;

        moving = true;

        spriteRenderer.sortingOrder = 10;
    }

    private void OnMouseUp()
    {
        if (GameManager.instance.gamestate != GameManager.GameState.Placement)
            return;


        if (CompareTag(allyTag))
        {
            if (canMove)
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                Vector3Int tileCoordinate = board.GetTilemap().WorldToCell(mousePos);

                spriteRenderer.sortingOrder = 0;

                List<Cell> authorizedCells = board.GetAuthorizedAllyCells();
                Cell targetCell = board.GetCell(tileCoordinate);

                if (targetCell == null || targetCell.GetIsOccupied() == true || !authorizedCells.Contains(targetCell))
                    updatePosition();

                else
                {
                    Cell newCell = board.GetCell(tileCoordinate);
                    occupyNewCell(newCell);
                    updatePosition();
                }

                moving = false;

                InventorySlot slotUnderMouse = InventorySlot.GetSlotUnderMouse();
                if (slotUnderMouse != null)
                    slotUnderMouse.PutInSlot(this);
            }
        }

    }

    private void OnDestroy()
    {

        currentCell.DecreaseNumberOfUnits();
        GameManager.instance?.RemoveUnit(this);
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

    public string GetAbilityName()
    {
        return abilityName;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetAbilityName(string abilityName)
    {
        this.abilityName = abilityName;
    }

    public void AttachBoard()
    {
        board = Board.CurrentBoard;
    }

    public HealthbarHandler getHealthbar()
    {
        return healthBar;
    }

    public void setBoard(Board board)
    {
        this.board = board;
    }

    public string GetName()
    {
        return unitName;
    }

    public void SetName(string name)
    {
        unitName = name;
    }

    public void setTargetUnit(Unit unit)
    {
        targetUnit = unit;
    }
}
