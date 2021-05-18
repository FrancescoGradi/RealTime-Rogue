using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponRack : MonoBehaviour {
    
    public GameObject weaponRackUI;
    public TMP_Text weaponRackText;
    public LayerMask playerLayer;

    private float weaponRackRange = 2f;

    private void Start() {
        weaponRackUI.SetActive(false);

        weaponRackText.text = "Overpassed Rooms: " + GameManager.instance.GetRoomNumber().ToString() + " Killed Enemies: " + 
                                GameManager.instance.GetKilledEnemies().ToString();
    }

    private void Update() {
        if (Vector3.Distance(Player.instance.transform.position, this.gameObject.transform.position) < weaponRackRange) {
            weaponRackUI.SetActive(true);
        } else {
            weaponRackUI.SetActive(false);
        }
    }



}
