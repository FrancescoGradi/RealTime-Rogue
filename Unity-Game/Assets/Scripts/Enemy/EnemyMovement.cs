using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MLAgents;

public class EnemyMovement : MonoBehaviour
{

    private CharacterController characterController;
    private Animator animator;
    private EnemyAgent agent;

    private float horizontal;
    private float vertical;

    public float speed = 4f;

    private Vector3 direction;


    void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<EnemyAgent>();
    }

    void Update() {
        agent.RequestDecision();
    }

    public void Movement(float horizontal, float vertical) {

        direction = new Vector3(horizontal, 0, vertical);
        characterController.Move(direction * speed * Time.deltaTime);
    }

}
