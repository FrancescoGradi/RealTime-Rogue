using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public TMP_Text damages;

    public void SetMaxHealth(int maxValue) {
        
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

    public void SetHealth(int health) {
        
        damages.gameObject.SetActive(true);
        damages.text = (- (slider.value - health)).ToString();
        StartCoroutine(RemoveAfterSeconds(2, damages.gameObject));

        slider.value = health;
    }

    private IEnumerator RemoveAfterSeconds(int seconds, GameObject gameObject) {

        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}