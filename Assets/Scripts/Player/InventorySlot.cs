using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [Header("UI References"), SerializeField]
    private Image lockedImage;

    [SerializeField]
    private Image selectedImage;

    [SerializeField]
    private UnitDescriptionDisplay unitDescriptionDisplay;

    public enum SlotType { Inventory, Shop, None }
    [SerializeField]
    private SlotType slotType = SlotType.Inventory;

    [Header("Prefab"), SerializeField]
    private GameObject unitPrefab;

    public enum SlotState { Empty, Used, Locked }
    public SlotState Status { get { return _status; } }
    private SlotState _status;

    public bool startsLocked;

    public uint id;

    private UnitDescription unitDescription;
    private GameObject child;
    private GameObject classDisplay;

    private Camera attachedCamera;

    private void Awake()
    {
        if (startsLocked)
        {
            _status = SlotState.Locked;
            lockedImage.gameObject.SetActive(true);
            return;
        }

        Unlock();
    }

    private void Start()
    {
        attachedCamera = Camera.main;
        if (unitDescriptionDisplay == null)
            unitDescriptionDisplay = FindObjectOfType<UnitDescriptionDisplay>();
    }

    public void Unlock()
    {
        _status = SlotState.Empty;
        lockedImage.gameObject.SetActive(false);
    }

    public void Select()
    {
        selectedImage.gameObject.SetActive(true);
    }

    public void Unselect()
    {
        selectedImage.gameObject.SetActive(false);
    }

    public void PutInSlot(Unit unit)
    {
        if (_status != SlotState.Empty)
            return;

        PutInSlot(new UnitDescription(unit));

        unit.board.GetCell(unit.currentPosition).SetIsOccupied(false);

        Destroy(unit.gameObject);
    }

    public void PutInSlot(UnitDescription unitDescription)
    {
        if (_status != SlotState.Empty)
            return;

        _status = SlotState.Used;

        child = new GameObject(unitDescription.GetUnitName());
        child.transform.SetParent(transform, false);

        {
            Image i = child.AddComponent<Image>();
            Sprite sprite = unitDescription.GetSprite();
            i.sprite = sprite;

            Rect spriteRect = unitDescription.GetSprite().rect;
            float aspectRatio = spriteRect.width / spriteRect.height;
            RectTransform rt = child.GetComponent<RectTransform>();

            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100 * aspectRatio);
        }

        this.unitDescription = unitDescription;

        {
            classDisplay = new GameObject("Class display");
            classDisplay.transform.SetParent(transform, false);
            RectTransform rt = classDisplay.AddComponent<RectTransform>();
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 25);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 25);
            classDisplay.AddComponent<Image>().sprite = unitDescription.GetClass().classIconSprite;
        }
    }

    public static InventorySlot GetSlotUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        InventorySlot slotUnderMouse = null;

        foreach (RaycastResult r in results)
        {
            slotUnderMouse = r.gameObject.GetComponent<InventorySlot>();
            if (slotUnderMouse != null)
                break;
        }
        return slotUnderMouse;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (_status != SlotState.Used || GameManager.instance.gamestate != GameManager.GameState.Placement)
            return;

        List<Cell> authorizedCells = Board.CurrentBoard.GetAuthorizedAllyCells();

        List<Cell> avalaibleCells = new List<Cell>();

        foreach (Cell cell in authorizedCells)
            if (!cell.GetIsOccupied())
                avalaibleCells.Add(cell);

        if (avalaibleCells.Count == 0)
            return;

        Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();
        newUnit.SetName(unitDescription.GetUnitName());
        newUnit.SetSprite(unitDescription.GetSprite());
        newUnit.raceStats = unitDescription.GetRace();
        newUnit.classStat = unitDescription.GetClass();
        newUnit.SetAbilityName(unitDescription.GetAbilityName());
        newUnit.SetGems(unitDescription.GetGems());
        newUnit.AttachBoard();
        newUnit.transform.SetPositionAndRotation(attachedCamera.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

        Cell destinatinCell = avalaibleCells[0];
        newUnit.initialPos = destinatinCell.TileMapPosition;

        newUnit.tag = Unit.allyTag;
        newUnit.isRandomUnit = false;
        newUnit.id = unitDescription.GetId();

        newUnit.PrepareForDragNDrop();
        ClearSlot();
    }

    public void ClearSlot()
    {
        _status = SlotState.Empty;
        unitDescription = null;
        Destroy(child);
        Destroy(classDisplay);
        unitDescriptionDisplay.UnselectActualSlot();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Status != SlotState.Used)
        {
            if (unitDescriptionDisplay != null)
                unitDescriptionDisplay.UnselectActualSlot();
            return;
        }

        unitDescriptionDisplay.ChangeActualSlot(this);
    }

    public UnitDescription GetCurrentUnitDescription()
    {
        return unitDescription;
    }

    public SlotType GetSlotType()
    {
        return slotType;
    }
}
