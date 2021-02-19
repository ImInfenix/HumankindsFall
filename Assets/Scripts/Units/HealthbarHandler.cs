using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarHandler : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;
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
        transform.GetChild(0).transform.position = attachedCamera.WorldToScreenPoint(transform.parent.position + offset);
    }

    public void SetHealth(int health, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }

    public void SetStamina(int stamina, int maxStamina)
    {
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = stamina;
    }

    public void SetStamina(int stamina)
    {
        staminaSlider.value = stamina;
    }
}
