using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenShield : Item
{
    // Start is called before the first frame update
    public Material material;

    private void Start() {
        itemName = "Golden Shield";
    }
    
    private void Update() {
        gameObject.transform.Rotate(0, 0.7f, 0, Space.Self);
    }

}
