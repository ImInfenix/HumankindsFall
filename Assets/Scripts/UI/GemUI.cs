using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GemUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 startPosition;
    private Transform startParent;
    private GemSlot gemSlot;
    private Gem currentGem;

    private bool isDragging;
    private bool canDrag;

    private Inventory inventory;

    public GemSlot GemSlot { get => gemSlot; set => gemSlot = value; }

    private void Awake()
    {
        currentGem = transform.parent.gameObject.GetComponent<Gem>();
        isDragging = false;
        canDrag = true;
    }

    private void Start()
    {
        inventory = Player.instance.Inventory;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && GemSlot == null && canDrag)
        {
            startPosition = gameObject.transform.position;
            startParent = transform.parent.parent;
            gameObject.transform.parent.SetParent(GameObject.Find("InventoryGems").transform);
            Tooltip.HideTooltip_Static();
            isDragging = true;
        }

        else if (!canDrag)
            SelectSlot();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GemSlot == null && canDrag)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, startPosition.z);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GemSlot == null && eventData.button == PointerEventData.InputButton.Left && canDrag)
        {
            isDragging = false;

            //get the gem slot under the mouse pointer
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            GemSlot slotUnderMouse = null;

            foreach (RaycastResult r in results)
            {
                slotUnderMouse = r.gameObject.GetComponent<GemSlot>();
                if (slotUnderMouse != null)
                    break;
            }

            //if there is no correct gem slot, reset the gem position
            if (slotUnderMouse == null || slotUnderMouse.Gem != null || !slotUnderMouse.IsUnitSlot)
            {
                transform.position = startPosition;
                transform.parent.SetParent(startParent);
            }

            //if there is a correct gem slot
            else
            {
                transform.position = slotUnderMouse.transform.position;
                slotUnderMouse.Gem = currentGem;

                if (GemSlot != null)
                    GemSlot.Gem = null;

                GemSlot = slotUnderMouse;
                transform.parent.SetParent(GemSlot.transform);

                Gem gem = GemSlot.Gem;

                //get selected unit description, then add this gem to its gem list
                UnitDescriptionDisplay unitDescriptionDisplay = GameObject.Find("CurrentUnitDescription").GetComponent<UnitDescriptionDisplay>();
                unitDescriptionDisplay.AddGem(transform.parent.gameObject);
                unitDescriptionDisplay.UpdateDescription();

                //remove the gem from the inventory
                inventory.RemoveGem(gem);
            }            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDragging)
            Tooltip.ShowTooltip_Static(currentGem.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip_Static();
    }

    public void DisableDrag()
    {
        canDrag = false;
    }

    public void SelectSlot()
    {
        if (GemSlot.selectedGemSlot != null && GemSlot.selectedGemSlot == gemSlot)
        {
            gemSlot.UnselectSlot();
            return;
        }

        if (GemSlot.selectedGemSlot)
            GemSlot.selectedGemSlot.UnselectSlot();

        gemSlot.SelectSlot();
    }
}
