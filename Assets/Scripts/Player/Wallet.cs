using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Player))]
public class Wallet : MonoBehaviour
{
    private TMP_Text amountDisplay;

    private int amount;

    public void Initialize(int initialAmount = 0)
    {
        amount = initialAmount;
        UpdateGUI();
    }

    public void FillFields()
    {
        GameObject wallet = GameObject.Find("Wallet_GUI");
        if (wallet == null)
            return;
        amountDisplay = wallet.GetComponent<TMP_Text>();
        UpdateGUI();
    }

    public void Earn(int amount)
    {
        this.amount += amount;
        UpdateGUI();
    }

    public bool Pay(int amount)
    {
        if (this.amount < amount)
            return false;

        this.amount -= amount;
        UpdateGUI();
        return true;
    }

    public int GetAmount()
    {
        return amount;
    }

    public void UpdateGUI()
    {
        if (amountDisplay == null)
            return;
        amountDisplay.text = amount.ToString();
    }
}
