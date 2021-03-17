using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GemUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 startPosition;
    Transform startParent;
    GemSlot gemSlot;
    Gem currentGem;

    bool isDragging;

    Inventory inventory;

    public GemSlot GemSlot { get => gemSlot; set => gemSlot = value; }

    private void Start()
    {
        inventory = Player.instance.Inventory;
        currentGem = transform.parent.gameObject.GetComponent<Gem>();
        isDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            startPosition = gameObject.transform.position;
            startParent = transform.parent.parent;
            gameObject.transform.parent.SetParent(GameObject.Find("InventoryGems").transform);
            Tooltip.HideTooltip_Static();
            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GemSlot == null)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, startPosition.z);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GemSlot == null && eventData.button == PointerEventData.InputButton.Left)
        {
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
                gameObject.transform.position = startPosition;

            //if there is a correct gem slot
            else
            {
                gameObject.transform.position = slotUnderMouse.transform.position;
                slotUnderMouse.Gem = currentGem;

                if (GemSlot != null)
                    GemSlot.Gem = null;

                GemSlot = slotUnderMouse;
                gameObject.transform.parent = GemSlot.transform;

                Gem gem = GemSlot.Gem;

                //get selected unit description, then add this gem to its gem list
                GameObject.Find("CurrentUnitDescription").GetComponent<UnitDescriptionDisplay>().
                    GetActualSlot().
                    GetCurrentUnitDescription().
                    AddGem(gem.GetType().ToString());

                //remove the gem from the inventory
                inventory.RemoveGem(gem);
            }

            transform.parent.SetParent(startParent);
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
}
