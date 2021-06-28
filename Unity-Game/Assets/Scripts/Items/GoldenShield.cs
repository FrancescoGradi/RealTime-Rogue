using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldenShield : Item {
    public Material material;
    private TMP_Text bonusText;

    private void Start() {
        itemName = "Golden Shield";

        bonusATK = 0;
        bonusHP = 0;
        bonusMANA = 0;
        // bonusDEF = Utility.GetRandomInt(3, 6);
        bonusDEF = 6;

        bonusText = this.GetComponentInChildren<TMP_Text>();
        bonusText.text = "+" + bonusDEF.ToString();
    }
    
    private void Update() {
        gameObject.transform.Rotate(0, 0.7f, 0, Space.Self);
    }

}
