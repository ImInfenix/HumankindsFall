using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSprite : MonoBehaviour
{
    float angle = 20.0f;

    void FixedUpdate()
    {

        gameObject.transform.Rotate(0.0f, 0.0f, angle, Space.Self);
    }
}