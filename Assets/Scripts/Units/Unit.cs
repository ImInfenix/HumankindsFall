using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [Header("BASE")]
    public const string ennemyTag = "UnitEnemy";
    public const string allyTag = "UnitAlly";

    public RaceStat raceStats;
    public ClassStat classStat;

    [Header ("STATS")]
    [SerializeField] private int maxLife;
    [SerializeField] private float currentLife;
    [SerializeField] private float incrementStamina;
    [SerializeField] private float armor;
    [SerializeField] private float initialArmor;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float accuracy;
    [SerializeField] private float damage;
    [SerializeField] private int range;
    [SerializeField] private float power;
    [SerializeField] private string unitName;
    [SerializeField] private bool isTargetable;
    [SerializeField] private bool stuned = false;
    [SerializeField] private bool poisonned = false;
    private uint level;

    private bool moving;
    private bool canMove;
    private bool supportBuff = false;
    private bool healer1 = false;
    private bool healer2 = false;
    private bool healer3 = false;
    private bool orcSpell = false;
    private bool giantSpell = false;
    private bool ratmanSpell = false;
    private int cptHealer = 0;
    private float saveArmor;
    private Unit healTarget = null;
    private bool canChangeColor = true;
    private float poisonTime;
    public float poisonDamageInterval = 1.0f;
    public int poisonDamage = 2;

    [Header("POSITION")]
    public Board board;
    [SerializeField] private Vector3 worldPosition;
    public Vector3Int currentPosition;
    public Cell currentCell = null;
    private List<Cell> path = null;
    public Vector3Int initialPos;

    private float startPosX;
    private float startPosY;


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
    [SerializeField] private bool isPlacedUnit;

    [Header("ABILITY")]
    [SerializeField] private string abilityName;

    [SerializeField] private Ability ability;

    [Header("UI")]
    public uint id;

    [SerializeField] private HealthbarHandler healthBar;
    [SerializeField] private StatusHandler status;
    [SerializeField] private Image classIcon;
    [SerializeField] private SpriteRenderer circleSprite;
    private GameObject projectileGameObject;

    private List<Gem> gems = new List<Gem>();

    [Header("Material")]
    public Material classic;
    public Material outline;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public int MaxLife { get => maxLife; set => maxLife = value; }
    public float CurrentLife { get => currentLife; set => currentLife = value; }
    public float IncrementStamina { get => incrementStamina; set => incrementStamina = value; }
    public float Armor { get => armor; set => armor = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Damage { get => damage; set => damage = value; }
    public int Range { get => range; set => range = value; }
    public float Power { get => power; set => power = value; }
    public Ability Ability { get => ability; set => ability = value; }
    public StatusHandler Status { get => status; set => status = value; }
    public bool Poisonned { get => poisonned; set => poisonned = value; }
    public uint Level { get => level; set => level = value; }

    // Start is called before the first frame update
    void Start()
    {
        if(isRandomUnit && classStat.clas != Class.DemonKing)
            GenerateRaceAndClass();

        InitializeUnit();

        GameManager.instance.AddUnit(this);
    }

    public void InitializeUnit()
    {
        board = FindObjectOfType<Board>();
        spriteRenderer.material = classic;
        //if the ability name exists
        if (abilityName != null && abilityName != "")
        {
            //load the ability class and add a component to the unit
            var abilityType = System.Type.GetType(abilityName);
            gameObject.AddComponent(abilityType);
            Ability = gameObject.GetComponent<Ability>();
            Ability.setUnit(this);
        }

        else
            healthBar.HideStaminaBar();

        if (classStat.classIconSprite == null)
            healthBar.HideClassIcon();

        MaxLife = raceStats.maxLife + classStat.maxLife + 10 * (int)(Level - 1);
        IncrementStamina = classStat.incrementStamina;
        Armor = raceStats.armor + classStat.armor + 0.05f * (Level - 1);
        initialArmor = armor;
        accuracy = 100;
        Power = 1 + 0.05f * (Level - 1);
          
        MoveSpeed = raceStats.moveSpeed + classStat.moveSpeed;
        AttackSpeed = raceStats.attackSpeed + classStat.attackSpeed + 0.05f * (Level - 1);
        Damage = raceStats.damage + classStat.damage + 1 * (Level - 1);
        Range = classStat.range;

        isTargetable = true;

        GetUnitGems();
        ApplyInitGemsEffect();

        CurrentLife = MaxLife;

        projectileGameObject = classStat.projectile;

        healthBar.SetHealth(CurrentLife, MaxLife);

        classIcon.sprite = classStat.classIconSprite;
        spriteRenderer = GetComponent<SpriteRenderer>();

        //if (raceStats.race != Race.Human)
        if(classStat.clas != Class.DemonKing)
            spriteRenderer.sprite = raceStats.getSprite(classStat.clas);//spriteRenderer.sprite = Resources.Load<Sprite>("Textures/Unit Sprites/" + raceStats.race.ToString() + "/" + classStat.clas.ToString());   // spriteRenderer.sprite = raceStats.unitSprite;

        // else
        //      spriteRenderer.sprite = Resources.Load<Sprite>("Textures/Unit Sprites/Humans/" + classStat.clas.ToString());

        //canAttack = true;
        //hasTarget = false;
        isActing = false;

        if (currentCell == null)
        {
            if (!isPlacedUnit && classStat.clas != Class.DemonKing)
            {
                Cell startCell = board.GetCell(new Vector3Int(initialPos.x, initialPos.y, 0));
                occupyNewCell(startCell);
                updatePosition();
            }
            else if(classStat.clas == Class.DemonKing)
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                Vector3Int tileCoordinate = board.GetTilemap().WorldToCell(mousePos);

                Cell newCell = board.GetCell(tileCoordinate);
                
                occupyNewCell(newCell);
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
            healthBar.SetHealthColor(new Color(0, 0, 1));
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
            gemProv.setUnit(this);
            if (isPlacedUnit)
                gems.Add(gemProv);
        }
    }

    public string[] GetGems()
    {
        string[] gemsArray = new string[gems.Count];
        for (int i = 0; i < gems.Count; i++)
        {
            gemsArray[i] = gems[i].GetType().ToString();
        }

        return gemsArray;
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
        level = newDescription.GetLevel();
    }

    public void UpdateUnit()
    {
        canMove = false;

        //if the unit has an ability and is currently following the ability's behavior
        //currently useless because no ability uses different behavior
        if (isAbilityActivated && abilityName != null && abilityName != "")
        {
            Ability.castAbility();
        }

        if (!isActing)
        {
            List<Unit> listAvailableTarget = PathfindingTool.unitsInRadius(currentCell, Range, targetTag);

            //if the target is not yet in range of attack
            if (listAvailableTarget.Count > 0)
            {
                if(listAvailableTarget[0].getTargetable())
                {
                    targetUnit = listAvailableTarget[0];
                    StartCoroutine(AttackTarget());
                }
            }

            else if (path != null && path.Count > 0)
            {
                //start the coroutine to move to the next cell of the path
                StartCoroutine(MoveToCell(path[0]));
            }

            else if (path == null)
            {
                StartCoroutine(UnitWaitForSeconds(1/moveSpeed));
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
        if (!cell.GetIsOccupied() && cell.GetIsObstacle() == false && stuned == false)
        {
            isActing = true;
            //change the occupied tile then start the animation
            occupyNewCell(cell);
            //StopCoroutine(MoveAnimation(cell));
            StartCoroutine(MoveAnimation(cell));
            yield return new WaitForSeconds(1/MoveSpeed);
            //ensure that the unit is at the center of the current cell before it can start acting again
            updatePosition();
            isActing = false;
        }

        else
        {
            path = null;
            yield return new WaitForSeconds(1/MoveSpeed);
        }
    }

    IEnumerator MoveAnimation(Cell cell)
    {
        //set the speed of the animation (distance at each iteration of while loop)
        float speed = 0.1f;

        //if the unit is moving too fast, inscrease the animation speed
        if (1/MoveSpeed <= 0.2)
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
        if(stuned == false)
        {
            isActing = true;

            StartCoroutine(AttackAnimation());

            if (orcSpell == false)
                if (giantSpell == false)
                    targetUnit.takeDamage(damage);
                else
                {
                    float damageGiant = damage*1.15f;
                    targetUnit.takeDamage((int)damageGiant);
                    targetUnit.activateStun(2);
                    giantSpell = false;
                }
            else
            {
                int rand = Random.Range(1, 101);
                if (rand <= accuracy)
                    targetUnit.takeOrcDamage(damage);
            }

            if(ratmanSpell == true)
            {
                ratmanSpell = false;
                targetUnit.activatePoison(5, poisonDamage);
            }

            ApplyAttackGemsEffect();

            if (classStat.clas == Class.Assassin && isTargetable == false)
                stopInvisibility();


            if (Range > 1)
                StartCoroutine(ProjectileAnimation());

            if (abilityName != null && abilityName != "")
            {
                Ability.updateAbility(IncrementStamina);
            }

            yield return new WaitForSeconds(1/AttackSpeed);
            isActing = false;

            cptHealer++;

            if (healer1 == true)
            {
                if (cptHealer == 5)
                {
                    Unit target = GameManager.instance.searchHealTarget();
                    if (target != null)
                        target.heal((target.getMaxlife() / 20) * 3);
                    cptHealer = 0;
                }
            }
            if (healer2 == true)
            {
                if (cptHealer == 3)
                {
                    Unit target = GameManager.instance.searchHealTarget();
                    if (target != null)
                        target.heal((target.getMaxlife() / 20) * 3);
                    cptHealer = 0;
                }
            }
            if (healer3 == true)
            {
                if (cptHealer == 2)
                {
                    Unit target = GameManager.instance.searchHealTarget();
                    if (target != null)
                        target.heal((target.getMaxlife() / 20) * 3);
                    cptHealer = 0;
                }
            }

            
        }
       
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
        if (1/AttackSpeed <= 0.3f)
        {
            animationTime = 2 * (1/AttackSpeed) / 3;
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
        if (1/AttackSpeed <= 0.2f)
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

    IEnumerator PoisonDamage(float poisonDamage)
    {
        Poisonned = true;
        float poisonCounter = 0;
        while (poisonCounter < poisonTime)
        {
            takeDamage(poisonDamage);
            yield return new WaitForSeconds(poisonDamageInterval);
            poisonCounter += poisonDamageInterval;
        }
        Poisonned = false;
        Status.setPoison(false);
    }

    public void occupyNewCell(Cell newCell)
    {
        if (currentCell != null)
        {
            currentCell.DecreaseNumberOfUnits();
            currentCell.RemoveCurrentUnit(this);
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

    public void takeDamage(float damage)
    {
        CurrentLife -= damage/armor ;
        checkDeath();
        healthBar.SetHealth(CurrentLife);
        StartCoroutine(ChangeColorAnimation(damageColor, 0.4f));
    }

    public void takeOrcDamage(float damage)
    {
        takeDamage(damage * armor);
    }

    public void heal(float heal)
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
        {
            if (stuned == false)
                spriteRenderer.color = baseColor;
            if(stuned == true)
                spriteRenderer.color = new Color(104f / 255f, 104f / 255f, 104f / 255f, 1f); 
        }
          
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
        if (PauseMenu.isGamePaused || EventSystem.current.IsPointerOverGameObject())
            return;

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
        healthBar.OnDragNDropStarts();
    }

    private void OnMouseUp()
    {
        if (PauseMenu.isGamePaused)
            return;

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

                if (targetCell == null || targetCell.GetIsOccupied() == true || !authorizedCells.Contains(targetCell) || targetCell.GetIsObstacle() == true)
                    updatePosition();

                else
                {
                    Cell newCell = board.GetCell(tileCoordinate);
                    occupyNewCell(newCell);
                    updatePosition();
                }

                moving = false;

                healthBar.OnDragNDropEnds();

                InventorySlot slotUnderMouse = InventorySlot.GetSlotUnderMouse();
                if (slotUnderMouse != null)
                    slotUnderMouse.PutInSlot(this);
            }
        }

    }

    private void OnDestroy()
    {

        currentCell.DecreaseNumberOfUnits();
        currentCell.RemoveCurrentUnit(this);
        GameManager.instance?.RemoveUnit(this);
    }

    public bool getTargetable()
    {
        return isTargetable;
    }

    public int getMaxlife()
    {
        return maxLife;
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

    public int getRange()
    {
        return range;
    }

    public bool getStuned()
    {
        return stuned;
    }

    public void setRange(int range)
    {
        this.range = range;
    }

    public float getCurrentLife()
    {
        return currentLife;
    }

    public void SetGems(string[] gems)
    {
        this.gems = new List<Gem>();

        if (gems != null)
        {
            foreach (string gem in gems)
            {
                GameObject gemGameObject = Resources.Load("Gems/" + gem) as GameObject;
                GameObject newGem = Instantiate(gemGameObject, transform.GetChild(2));
                this.gems.Add(newGem.GetComponent<Gem>());
            }
        }
    }

    public void ApplyAbilityGemsEffects()
    {
        foreach (Gem gem in gems)
        {
            gem.AbilityGemEffect();
        }
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

    public Unit getTargetUnit()
    {
        return targetUnit;
    }

    public void ActivateClass(Class c, int nb)
    {
        switch (c)
        {
            case Class.Mage:
                if(nb >= 2 && nb < 4)
                {
                    Ability.mageSynergy(1);
                }
                if(nb >= 4)
                {
                    Ability.mageSynergy(2);
                }
                break;

            case Class.Warrior:
                if (nb >= 2 && nb < 4)
                {
                    damage += damage/4;
                }
                if (nb >= 4)
                {
                    damage += damage/2;
                }
                break;

            case Class.Tank:
                if(nb >= 2)
                {
                    maxLife += maxLife/2;
                    currentLife = maxLife;
                    healthBar.SetHealth(currentLife, maxLife);
                }
                break;

            case Class.Bowman:
                if (nb >= 2)
                {
                    attackSpeed += attackSpeed/4;
                }
                break;

            case Class.Healer:
                if(nb == 1)
                {
                    healer1 = true;
                }
                if(nb == 2)
                {
                    healer2 = true;
                }
                if(nb >= 3)
                {
                    healer3 = true;
                }
                break;

            case Class.Support:
                List<Unit> unitInRange = PathfindingTool.unitsInRadius(currentCell, 1, allyTag);
                if (nb >= 2)
                {
                    foreach(Unit unit in unitInRange)
                    {

                        if (nb == 2)
                        {
                            unit.supportBoost(1);
                        }
                        if (nb == 3)
                        {
                            unit.supportBoost(2);
                        }
                        if (nb >= 4)
                        {
                            unit.supportBoost(3);
                        }
                    }
                }
                break;

            case Class.Berserker:
                if (nb >= 2)
                {
                    if(board.isOccupiedNeighbour(currentCell) == false)
                    {
                        damage += damage / 2;
                        armor += initialArmor / 4;
                    }
                }
                break;

            case Class.Assassin:
                if(nb >= 1)
                {
                    startInvisibility();

                    Invoke("stopInvisibility", 5);
                }
                break;
        }
    }

    public void supportBoost(int lvl)
    {
        if(supportBuff == false)
        {
            if (lvl == 1)
            {
                armor += initialArmor / 5;
            }
            if (lvl == 2)
            {
                armor += (initialArmor / 10) * 3;
            }
            if (lvl == 3)
            {
                armor += initialArmor / 2;
            }
            supportBuff = true;
        }
    }

    public void startInvisibility()
    {
        isTargetable = false;
        baseColor.a = 0.5f;
        spriteRenderer.color = baseColor;
    }
    public void stopInvisibility()
    {
        if(isTargetable == false)
        {
            isTargetable = true;
            baseColor.a = 1;
            spriteRenderer.color = baseColor;
        }
    }

    public void activateOrcSpell(float accuracyLost, float time)
    {
        Status.setOrcUp(true);
        orcSpell = true;
        accuracy -= accuracyLost;
        Invoke("endOrcSpell", time);
    }

    private void endOrcSpell()
    {
        Status.setOrcUp(false);
        orcSpell = false;
        accuracy = 100;
    }

    public void activateSkeletonSpell(float armorLost, float time)
    {
        Status.setBreakShield(true);
        saveArmor = armor;
        armor -= (armorLost*armor);
        Invoke("endSkeletonSpell", time);
    }

    private void endSkeletonSpell()
    {
        Status.setBreakShield(false);
        armor = saveArmor;
    }

    public void activateStun(float time)
    {
        stuned = true;
        spriteRenderer.color = new Color(104f / 255f, 104f / 255f, 104f / 255f,1f);
        Invoke("endStun", time);
    }

    private void endStun()
    {
        stuned = false;
        spriteRenderer.color = Color.white;
    }


    public void activateGiantSpell()
    {
        giantSpell = true;
    }
    public void activateElementalSpell(int damage)
    {
        takeDamage(damage);

    }
    public void activateRatmanSpell()
    {
        ratmanSpell = true;
    }

    public void activatePoison(float time, float damage)
    {
        Status.setPoison(true);
        Poisonned = true;
        poisonTime = time;
        StartCoroutine(PoisonDamage(damage));
    }

    public void activateOutline()
    {
        spriteRenderer.material = outline;
    }

    public void desactivateOutline()
    {
        spriteRenderer.material = classic;
    }
}
