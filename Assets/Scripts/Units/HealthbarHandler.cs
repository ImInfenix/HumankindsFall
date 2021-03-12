using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarHandler : MonoBehaviour
{
    public static List<HealthbarHandler> healthbarHandlers;

    public Slider slider;
    public Slider healthSlider;
    public Slider staminaSlider;
    private Vector3 offset = new Vector3(0,0.3f,0);

    public Camera attachedCamera;

    private void Awake()
    {
        if (healthbarHandlers == null)
            healthbarHandlers = new List<HealthbarHandler>();

        healthbarHandlers.Add(this);

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        attachedCamera = Camera.main;
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void OnDestroy()
    {
        healthbarHandlers.Remove(this);
    }

    private void UpdatePosition()
    {
        transform.GetChild(0).transform.position = attachedCamera.WorldToScreenPoint(transform.parent.position + offset);
    }

    public void SetHealth(float health, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }

    public void SetStamina(float stamina, int maxStamina)
    {
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = stamina;
    }

    public void SetStamina(int stamina)
    {
        staminaSlider.value = stamina;
    }

    public static void ShowAll()
    {
        foreach (HealthbarHandler handler in healthbarHandlers)
            handler.gameObject.SetActive(true);
    }
    
    public static void HideAll()
    {
        foreach (HealthbarHandler handler in healthbarHandlers)
            handler.gameObject.SetActive(false);
    }
}
