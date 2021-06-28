using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusPotion : Item {

    private TMP_Text bonusText;

    private void Start() {
        itemName = "Bonus Potion";

        bonusATK = 5;
        bonusHP = 0;
        bonusMANA = 5;
        bonusDEF = 5;

        bonusText = this.GetComponentInChildren<TMP_Text>();
        bonusText.text = "+" + bonusATK.ToString();
    }

    private void Update() {
        gameObject.transform.Rotate(0, 0.7f, 0, Space.Self);
    }
}
