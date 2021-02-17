using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BastardSword : Item {
    public int damage = 10;
    public float attackRate = 1.5f;
    public float attackRange = 1.2f;
    public Material material;

    private void start() {
        itemName = "Bastard Sword";
    }

    private void Update() {
        gameObject.transform.Rotate(0, 0.7f, 0, Space.Self);
    }

}
