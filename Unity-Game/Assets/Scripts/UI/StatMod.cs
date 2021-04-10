using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StatMod : MonoBehaviour {
    
    public int baseValue;
    public int actualValue;

    public TMP_Text actualValueText;
    public GameObject button;

    public GameObject leftArrow;
    public GameObject rightArrow;

    public void SetBaseValue(int value) {

        baseValue = value;
        actualValue = baseValue;
        actualValueText.text = actualValue.ToString(); 

        if (actualValue != baseValue) {
            leftArrow.SetActive(true);
            actualValueText.color = new Color32(0, 100, 14, 255);
        } else {
            leftArrow.SetActive(false);
            actualValueText.color = new Color32(132, 132, 132, 255);
        }

    }

    public void SetActualValue(int value) {

        actualValue += value;
        actualValueText.text = actualValue.ToString(); 

        if (actualValue != baseValue) {
            leftArrow.SetActive(true);
            actualValueText.color = new Color32(0, 100, 14, 255);
        } else {
            leftArrow.SetActive(false);
            actualValueText.color = new Color32(132, 132, 132, 255);
        }
    }

    public void AddOnePoint() {
        actualValue += 1;
        actualValueText.text = actualValue.ToString();
        actualValueText.color = new Color32(0, 100, 14, 255); 

        leftArrow.SetActive(true);
    }

    public void SubtractOnePoint() {
        actualValue -= 1;
        actualValueText.text = actualValue.ToString();

        if (actualValue == baseValue) {
            leftArrow.SetActive(false);
            actualValueText.color = new Color32(132, 132, 132, 255);           
        }
    }

}
