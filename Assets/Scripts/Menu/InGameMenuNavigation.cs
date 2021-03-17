using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuNavigation : MonoBehaviour
{
    public void GoToMap()
    {
        GameManager.instance.EnterMap();
    }
}
