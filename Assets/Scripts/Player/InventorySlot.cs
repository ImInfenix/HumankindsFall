using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private Image lockedImage;

    [Header("Prefab"), SerializeField]
    private GameObject unitPrefab;

    public enum SlotState { Empty, Used, Locked }
    public SlotState Status { get { return _status; } }
    private SlotState _status;

    public bool startsLocked;

    public uint Id { get; private set; }

    private UnitDescription unitDescription;
    private GameObject child;

    private Camera attachedCamera;

    private void Awake()
    {
        Id = GetNewId();

        if (startsLocked)
        {
            _status = SlotState.Locked;
            lockedImage.gameObject.SetActive(true);
            return;
        }

        Unlock();
        attachedCamera = Camera.main;
    }

    public void Unlock()
    {
        _status = SlotState.Empty;
        lockedImage.gameObject.SetActive(false);
    }

    public void PutInSlot(Unit unit)
    {
        if (_status != SlotState.Empty)
            return;

        _status = SlotState.Used;

        child = new GameObject(unit.name);
        child.transform.SetParent(transform, false);

        Image i = child.AddComponent<Image>();
        Sprite sprite = unit.GetSprite();
        i.sprite = sprite;

        Rect spriteRect = unit.GetSprite().rect;
        float aspectRatio = spriteRect.width / spriteRect.height;
        RectTransform rt = child.GetComponent<RectTransform>();
        float size = spriteRect.height * aspectRatio * 2;
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, (rt.rect.width - size) / 2, size);

        unitDescription = new UnitDescription(sprite, unit.raceStats, unit.classStat, unit.GetAbilityName());

        Destroy(unit.gameObject);
    }

    private static uint currentId = 0;

    private static uint GetNewId()
    {
        uint newId = currentId;
        currentId++;
        return newId;
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
        if (_status != SlotState.Used)
            return;

        Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();
        newUnit.SetSprite(unitDescription.GetSprite());
        newUnit.raceStats = unitDescription.GetRace();
        newUnit.classStat = unitDescription.GetClass();
        newUnit.SetAbilityName(unitDescription.GetAbilityName());
        newUnit.AttachBoard();
        newUnit.transform.SetPositionAndRotation(attachedCamera.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

        newUnit.tag = "UnitAlly";
        newUnit.PrepareForDragNDrop();

        _status = SlotState.Empty;
        unitDescription = null;
        Destroy(child);
    }
}
