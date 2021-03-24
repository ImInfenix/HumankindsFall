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
    private Vector3 barsOffset = new Vector3(0, 0.3f, 0);

    [SerializeField] private GameObject classIconGameObject;
    private Vector3 classIconOffset = new Vector3(0.3f, 0.14f, 0);

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Image Fill;

    private void Awake()
    {
        if (healthbarHandlers == null)
            healthbarHandlers = new List<HealthbarHandler>();

        healthbarHandlers.Add(this);

        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        healthbarHandlers.Remove(this);
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

    public static void ShowBars()
    {
        foreach (HealthbarHandler handler in healthbarHandlers)
            handler.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public static void HideBars()
    {
        foreach (HealthbarHandler handler in healthbarHandlers)
            handler.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void HideStaminaBar()
    {
        staminaSlider.gameObject.SetActive(false);
    }

    public void HideClassIcon()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void OnDragNDropStarts()
    {
        canvas.sortingOrder = 10;
    }

    public void OnDragNDropEnds()
    {
        canvas.sortingOrder = 4;
    }

    public void SetHealthColor(Color color)
    {
        Fill.color = color;
    }
}
