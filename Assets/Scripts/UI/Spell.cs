using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Spell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    private string definition;
    private bool activated;
    private bool onCooldown;
    private float cooldown;
    private Cell currentCell;
    private Cell targetCell;
    public Board board;
    public Image cooldownImage;
    private RaceCount elemental;

    [Header ("Select Spell Race")]
    public Race race;
    [Header("Select Spell Range")]
    [Range(0, 5)]
    public int range;

    // Start is called before the first frame update
    void Start()
    {
        cooldownImage.fillAmount = 0;
        activated = false;
        onCooldown = false;
        board = FindObjectOfType<Board>();

        //Initialize tooltips def
        switch(race)
        {
            case (Race.Orc):
                definition = "For 5 seconds, orc in spell area ignore enemy defense but they lose 10% accuracy";
                cooldown = 5;
                break;
            case (Race.Skeleton):
                definition = "Ennemis in spell area loose 25% armor for 5 seconds";
                cooldown = 5;
                break;
            case (Race.Octopus):
                definition = "Stun the target for 5 seconds";
                cooldown = 5;
                break;
            case (Race.Elemental):
                elemental = SynergyHandler.instance.getElementals();
                definition = "Deal "+ elemental.getNumber()*10+" damage to enemy target";
                cooldown = 5;
                break;
            case (Race.Giant):
                definition = "Choose a giant unit, his next attack will deal 15% more damage and stun the enemy for 2 seconds";
                cooldown = 5;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(activated == true)
        {            
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3Int tileCoordinate = board.GetTilemap().WorldToCell(mousePos);
            
            targetCell = board.GetCell(tileCoordinate);

            if(targetCell != currentCell)
            {
                DesactivateArea(currentCell);
                ActivateArea(targetCell);
                currentCell = targetCell;
            } 
            
            //Press Left Click = launch
            if(Input.GetMouseButtonDown(0))
            {
                DesactivateArea(currentCell);
                if (currentCell != null)
                {                   
                    ActivateSpell();
                }
                else
                {
                    activated = false;
                }                
            }

            //Press Right Click = cancel
            if (Input.GetMouseButtonDown(1))
            {
                DesactivateArea(currentCell);
                activated = false;
            }
        }
        if(onCooldown == true)
        {
            cooldownImage.fillAmount -= 1 / cooldown * Time.deltaTime;
            if (cooldownImage.fillAmount <= 0 )
            {
                cooldownImage.fillAmount = 0;
                onCooldown = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowTooltip_Static(definition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip_Static();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onCooldown == false && GameManager.instance.gamestate == GameManager.GameState.Combat)
        {
            if (activated == true)
            {
                DesactivateArea(currentCell);
                activated = false;
            }
            else
            {
                activated = true;
            }
        }                 
    }

    private void ActivateArea(Cell center)
    {
        if (center != null)
        {
            List<Cell> affectedCells = PathfindingTool.cellsInRadius(center, range);
            foreach (Cell cell in affectedCells)
            {
                if (cell != null)
                {
                    cell.SetColor(Color.red); //new Color(Color.red.r, Color.red.g, Color.red.b,0.9f)
                }
            }
        }
    }

    private void DesactivateArea(Cell center)
    {
        if (center != null)
        {
            List<Cell> affectedCells = PathfindingTool.cellsInRadius(center, range);
            foreach (Cell cell in affectedCells)
            {
                if (cell != null)
                {
                    cell.SetColor(Color.white);
                }
            }
        }
    }

    private void ActivateSpell()
    {
        List<Unit> affectedAllyUnit = PathfindingTool.unitsInRadius(currentCell, range, "UnitAlly");
        List<Unit> affectedEnemyUnit = PathfindingTool.unitsInRadius(currentCell, range, "UnitEnemy");
        bool launched = false;

        switch (race)
        {
            case (Race.Orc):
                foreach(Unit unit in affectedAllyUnit)
                {
                    if(unit != null)
                    {
                        if (unit.getRace() == Race.Orc)
                        {
                            unit.activateOrcSpell(10, 5);
                            launched = true;
                        }
                    }                   
                }
                if(launched == true)
                {
                    onCooldown = true;
                    cooldownImage.fillAmount = 1;
                    activated = false;
                }
                break;

            case (Race.Skeleton):
                foreach (Unit unit in affectedEnemyUnit)
                {
                    if(unit != null)
                    {
                        unit.activateSkeletonSpell(0.25f, 5);
                        launched = true;
                    }                   
                }
                if (launched == true)
                {
                    onCooldown = true;
                    cooldownImage.fillAmount = 1;
                    activated = false;
                }
                break;

            case (Race.Octopus):
                foreach (Unit unit in affectedEnemyUnit)
                {
                    if(unit != null)
                    {                       
                        unit.activateStun(5);
                        launched = true;
                    }
                }
                if (launched == true)
                {
                    onCooldown = true;
                    cooldownImage.fillAmount = 1;
                    activated = false;
                }
                break;

            case (Race.Elemental):
                foreach (Unit unit in affectedEnemyUnit)
                {
                    if (unit != null)
                    {
                        unit.activateElementalSpell(elemental.getNumber() * 10);
                        launched = true;
                    }
                }
                if (launched == true)
                {
                    onCooldown = true;
                    cooldownImage.fillAmount = 1;
                    activated = false;
                }
                break;

            case (Race.Giant):
                foreach (Unit unit in affectedAllyUnit)
                {
                    if (unit != null)
                    {
                        if (unit.getRace() == Race.Giant)
                        {
                            unit.activateGiantSpell();
                            launched = true;
                        }
                    }
                }
                if (launched == true)
                {
                    onCooldown = true;
                    cooldownImage.fillAmount = 1;
                    activated = false;
                }
                break;
        }
    }
}
