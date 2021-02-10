using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarHandler : MonoBehaviour
{

    public Slider slider;
    private Vector3 offset = new Vector3(0,0.3f,0);

    public Camera attachedCamera;

    void Start()
    {
        attachedCamera = Camera.main;
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        slider.transform.position = attachedCamera.WorldToScreenPoint(transform.parent.position + offset);
    }

    public void SetHealth(int health, int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
