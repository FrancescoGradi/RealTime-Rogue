using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPotion : Item
{
    public int statsBonus = 5;

    private void Start() {
        itemName = "Bonus Potion";

        bonusATK = 5;
        bonusHP = 0;
        bonusMANA = 5;
        bonusDEF = 5;
    }

    private void Update() {
        gameObject.transform.Rotate(0, 0.7f, 0, Space.Self);
    }
}
