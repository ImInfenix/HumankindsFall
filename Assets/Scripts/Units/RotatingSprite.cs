using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSprite : MonoBehaviour
{
    float angle = 10.0f;

    void Update()
    {

        gameObject.transform.Rotate(0.0f, 0.0f, angle, Space.Self);
    }
}
