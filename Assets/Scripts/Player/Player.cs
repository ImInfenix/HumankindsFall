using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

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
    }

    private void Start()
    {
        _inventory.FillFields();
    }
}
