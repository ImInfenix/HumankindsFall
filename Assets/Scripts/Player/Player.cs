using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public int walletStartAmount = 0;

    public Wallet Wallet { get { return _wallet; } }
    private Wallet _wallet;

    public Inventory Inventory { get { return _inventory; } }
    private Inventory _inventory;

    private void Awake()
    {
        //Singleton creation
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        _wallet = GetComponent<Wallet>();
        _inventory = GetComponent<Inventory>();
        InitiateForNewScene();
    }

    public void InitiateForNewScene()
    {
        Inventory.FillFields();
        Wallet.FillFields();
    }

    private void Start()
    {
        SaveFile saveFile = SavingSystem.RetrieveData();

        Inventory.Initialize(saveFile?.GetAllUnits());
        if (saveFile != null)
        {
            Wallet.Initialize(saveFile.walletAmount);
            UnitDescription.currentId = saveFile.unitGeneratorId;
            Marker.finishedLevels = saveFile.finishedLevels;
            return;
        }
        Wallet.Initialize(walletStartAmount);
    }
}
