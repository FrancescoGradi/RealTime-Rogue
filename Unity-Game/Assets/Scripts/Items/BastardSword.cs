using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BastardSword : Item {
    public Material material;
    private TMP_Text bonusText;

    private void Start() {
        itemName = "Bastard Sword";

        // bonusATK = Utility.GetRandomInt(7, 10);
        bonusATK = 10;
        bonusHP = 0;
        bonusMANA = 0;
        bonusDEF = 0;

        bonusText = this.GetComponentInChildren<TMP_Text>();
        bonusText.text = "+" + bonusATK.ToString();
    }

    private void Update() {
        gameObject.transform.Rotate(0, 0.7f, 0, Space.Self);
    }

}
