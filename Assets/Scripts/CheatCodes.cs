using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodes : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneLoader.ReloadScene();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SavingSystem.SaveData();
        }
    }
}
