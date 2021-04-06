using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPot : MonoBehaviour {

    public GameObject healthPotText;
    public LayerMask playerLayer;
    public GameObject redPotion;
    public int health = 30;

    private bool isHealthPotFull = true;
    private float healthPotRange = 2.5f;

    private void Start() {
        healthPotText.SetActive(false);
    }

    private void Update() {
        
        if (isHealthPotFull && Vector3.Distance(Player.instance.transform.position, this.gameObject.transform.position) < healthPotRange) {
            healthPotText.SetActive(true);
            if (Input.GetButton("BaseAction")) {
                isHealthPotFull = false;
                healthPotText.SetActive(false);
                redPotion.SetActive(false);
                Player.instance.AddHealth(health);
            }

        } else {
            healthPotText.SetActive(false);
        }
    }
}
