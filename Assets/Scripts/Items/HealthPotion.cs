using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    
    private void Start() {
        itemName = "Health Potion";

        bonusATK = 0;
        bonusHP = 10;
        bonusMANA = 0;
        bonusDEF = 0;

    }
}
