﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodes : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Inventory inventory = Player.instance.Inventory;
            inventory.GenerateGems();
            GameObject.Find("GemsObjects").GetComponent<GemsInventory>().UpdateDisplay();
        }
    }
}
