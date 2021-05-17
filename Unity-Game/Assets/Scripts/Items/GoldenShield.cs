using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenShield : Item
{
    public Material material;

    private void Start() {
        itemName = "Golden Shield";

        bonusATK = 0;
        bonusHP = 0;
        bonusMANA = 0;
        bonusDEF = 6;
    }
    
    private void Update() {
        gameObject.transform.Rotate(0, 0.7f, 0, Space.Self);
    }

}
