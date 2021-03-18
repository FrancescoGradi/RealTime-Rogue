using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public int updateMovement = 100;
    public float translateFactor = 2f;
    public float speed = 2f;

    private int count = 0;
    private CharacterController characterController;
    private Vector3 direction = new Vector3(0, 0, 0);
    private bool moving = false;


    private void Start() {
        characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate() {

        count += 1;

        if (!moving && count == 1) {
            moving = true;
        }

        if (count == updateMovement) {
            count = 0;
            ChangeRandomMovement();
        } 

        if (moving)
            Move();
    }

    public void ResetPosition(Vector3 pos) {

        direction = new Vector3(0, 0, 0);
        moving = false;
        this.gameObject.transform.position = pos;
    }

    private void Move() {
        characterController.Move(direction * speed * Time.fixedDeltaTime);
    }

    private void ChangeRandomMovement() {
        direction = new Vector3((int) UnityEngine.Random.Range(-translateFactor, translateFactor), 0f, (int) UnityEngine.Random.Range(-translateFactor, translateFactor));
    }
}
