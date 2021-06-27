using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sprite : MonoBehaviour {
    
    public TMP_Text spriteValue;

    public void SetSpriteValue(int value, string category) {
        spriteValue.text = "+" + value.ToString() + " " + category;
    }

}
