using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    public int healthBonus = 10;
    
    private void Start() {
        itemName = "Health Potion";
    }
}
