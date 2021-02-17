using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TMP_Text healthText;

    private void Awake() {
    }

    public void SetMaxHealth(int maxValue) {
        
        slider.maxValue = maxValue;
        slider.value = maxValue;

        healthText.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
    }

    public void SetHealth(int health) {

        slider.value = health;
        healthText.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
    }
}