using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public enum SlotState { Empty, Used, Locked }
    public SlotState Status { get { return _status; } }
    private SlotState _status;

    public bool startsLocked;

    private void Awake()
    {
        _status = startsLocked ? SlotState.Locked : SlotState.Empty;
    }

    public void Unlock()
    {
        _status = SlotState.Empty;
    }
}
