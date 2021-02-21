using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    public enum SlotState { Empty, Used, Locked }
    public SlotState Status { get { return _status; } }
    private SlotState _status;

    public InventorySlot(bool isLocked)
    {
        _status = isLocked ? SlotState.Locked : SlotState.Empty;
    }

    public void Unlock()
    {
        _status = SlotState.Empty;
    }
}
