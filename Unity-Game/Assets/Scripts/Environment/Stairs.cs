using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {

    public GameObject stairsText;
    public LayerMask playerLayer;

    private float stairsRange = 4.5f;
    private bool roomChanged = false;


    private void Start() {
        stairsText.SetActive(false);
    }

    private void Update() {

        if (Vector3.Distance(Player.instance.transform.position, this.gameObject.transform.position) < stairsRange) {
            stairsText.SetActive(true);

            if (Input.GetButton("BaseAction") && !roomChanged) {
                roomChanged = true;
                GameManager.instance.CreateNewRoom("T");
            }            
        } else {
            stairsText.SetActive(false);
        }
    }
}
