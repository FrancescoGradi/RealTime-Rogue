using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialRoom : MonoBehaviour {

    public GameObject stairs;
    public GameObject healthPot;
    public GameObject magicTable;
    public GameObject weaponRack;

    public LayerMask playerLayer;

    private float epsilon = 3f;

    private void Update() {

        if (Input.GetButton("BaseAction")) {
            
            if (SearchNearbyPlayers(stairs).Length > 0) {
                FindObjectOfType<GameManager>().CreateNewRoom("T");
            }

            if (SearchNearbyPlayers(healthPot).Length > 0) {
                Debug.Log("HealthPot");
            }

            if (SearchNearbyPlayers(magicTable).Length > 0) {
                Debug.Log("Magic Table");
            }

            if (SearchNearbyPlayers(weaponRack).Length > 0) {
                Debug.Log("Weapon Rack");
            }
        }
    }

    private Collider[] SearchNearbyPlayers(GameObject obj) {
        return Physics.OverlapSphere(obj.transform.position, epsilon, playerLayer);
    }

}
