using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarHandler : MonoBehaviour
{

    public Slider slider;
    private Vector3 offset = new Vector3(0,0.3f,0);

    // Start is called before the first frame update
    public void SetHealth(int health, int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}
