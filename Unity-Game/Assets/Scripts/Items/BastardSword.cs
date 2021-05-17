using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BastardSword : Item {
    public Material material;

    private void start() {
        itemName = "Bastard Sword";

        bonusATK = 10;
        bonusHP = 0;
        bonusMANA = 0;
        bonusDEF = 0;
    }

    private void Update() {
        gameObject.transform.Rotate(0, 0.7f, 0, Space.Self);
    }

}
