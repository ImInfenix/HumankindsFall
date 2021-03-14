using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GemUI : MonoBehaviour
{
    Vector3 startPosition;
    GemSlot gemSlot;

    public GemSlot GemSlot { get => gemSlot; set => gemSlot = value; }

    public void OnMouseDown()
    {
        startPosition = gameObject.transform.position;
    }

    public void OnMouseDrag()
    {
        if (GemSlot == null)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, startPosition.z);
        }
    }

    public void OnMouseUp()
    {
        if (GemSlot == null)
        {
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

            if (slotUnderMouse == null || slotUnderMouse.Gem != null || !slotUnderMouse.IsUnitSlot)
                gameObject.transform.position = startPosition;

            else
            {
                gameObject.transform.position = slotUnderMouse.transform.position;
                slotUnderMouse.Gem = transform.parent.gameObject.GetComponent<Gem>();
                print(slotUnderMouse.Gem);
                if (GemSlot != null)
                    GemSlot.Gem = null;
                GemSlot = slotUnderMouse;
                gameObject.transform.parent = GemSlot.transform;

                //get selected unit description, then add this gem to its gem list
                GameObject.Find("CurrentUnitDescription").GetComponent<UnitDescriptionDisplay>().
                    GetActualSlot().
                    GetCurrentUnitDescription().
                    AddGem(GemSlot.Gem.GetType().ToString());
            }
        }
    }
}
