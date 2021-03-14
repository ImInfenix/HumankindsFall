using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GemSlot : MonoBehaviour
{
    Gem gem;
    bool isLocked;
    bool isUnitSlot = false;

    public Gem Gem { get => gem; set => gem = value; }
    public bool IsUnitSlot { get => isUnitSlot; set => isUnitSlot = value; }
}
