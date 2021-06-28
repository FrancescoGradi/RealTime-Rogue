using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthPotion : Item {
    private TMP_Text bonusText;
    
    private void Start() {

        itemName = "Health Potion";

        bonusATK = 0;
        bonusHP = 10;
        bonusMANA = 0;
        bonusDEF = 0;

        bonusText = this.GetComponentInChildren<TMP_Text>();
        bonusText.text = "+" + bonusHP.ToString();
    }
}
