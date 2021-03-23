﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MLAgents;

public class Target : MonoBehaviour {

    public int updateMovement = 100;
    public float translateFactor = 2f;
    public float speed = 2f;
    public int HP = 5;
    public int currentHealth = 5;

    private int count = 0;
    private CharacterController characterController;
    private Vector3 direction = new Vector3(0, 0, 0);
    private bool moving = false;
    private bool isDown = false;



    private void Start() {
        characterController = GetComponent<CharacterController>();
        currentHealth = HP;
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

        currentHealth = HP;
        direction = new Vector3(0, 0, 0);
        isDown = false;
        moving = false;
        this.gameObject.transform.position = pos;
    }

    private void Move() {
        characterController.Move(direction * speed * Time.fixedDeltaTime);
    }

    private void ChangeRandomMovement() {
        direction = new Vector3((int) UnityEngine.Random.Range(-translateFactor, translateFactor), 0f, (int) UnityEngine.Random.Range(-translateFactor, translateFactor));
    }

    public void TakeDamage(int damage, float delay) {

        StartCoroutine(DamageWaiter(damage, delay));
    }

    private IEnumerator DamageWaiter(int damage, float seconds) {
        
        yield return new WaitForSecondsRealtime(seconds);

        currentHealth -= damage;

        if (!isDown && currentHealth <= 0) {
            isDown = true;
            StartCoroutine(PlayerDownWaiter(0.3f));
        }
    }

    private IEnumerator PlayerDownWaiter(float seconds) {

        yield return new WaitForSecondsRealtime(seconds);

        FindObjectOfType<EnemyAgent>().PlayerDown();
    }
}
