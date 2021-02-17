using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public Wallet Wallet { get; private set; }

    private void Awake()
    {
        //Singleton creation
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        Wallet = GetComponent<Wallet>();
    }
}
