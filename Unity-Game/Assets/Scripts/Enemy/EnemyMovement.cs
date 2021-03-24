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

    public float epsilon = 1f;

    public GameObject target;
    public LayerMask envObjectsLayer;


    public float speed = 4f;
    public float turnSmoothTime = 0.2f;

    private Vector3 direction = new Vector3(0, 0, 0);
    private int count = 0;
    public int updateRate = 20;
    private float turnSmoothVelocity;

    private bool targetReached = false;


    void Start() {

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<EnemyAgent>();
    }

    private void FixedUpdate() {

        count += 1;
        if (count == updateRate) {
            agent.RequestDecision();
            count = 0;
        }

        if (!targetReached && !animator.GetBool("attacking")) {
            
            if (direction.magnitude >= 0.05f) {
                animator.SetInteger("condition", 1);
                Movement();
            } else {
                animator.SetInteger("condition", 0);
            }
            
            if (!targetReached) {
                if (System.Math.Abs(this.gameObject.transform.position.z - target.transform.position.z) < epsilon && 
                    System.Math.Abs(this.gameObject.transform.position.x - target.transform.position.x) < epsilon) {
                    targetReached = true;
                    agent.TargetReached();
                }
            }
        }
    }

    public void AddMovement(float horizontal, float vertical) {

        this.horizontal = horizontal;
        this.vertical = vertical;
        direction = new Vector3(horizontal, 0, vertical);

    }

    public void Movement() {

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        characterController.Move(moveDir.normalized * speed * Time.fixedDeltaTime);
    }

    public void SetTargetReached(bool targetReached) {
        this.targetReached = targetReached;
    }

    public float GetRayCastDistance(float maxDistance, float angle) {

        RaycastHit hit;
        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
        Vector3 pos = this.gameObject.transform.position;
        pos.y += 1.5f;

        if (Physics.Raycast(pos, direction, out hit, maxDistance, envObjectsLayer)) {
            Debug.DrawRay(pos, direction * hit.distance, Color.yellow, Time.fixedDeltaTime);
            return hit.distance;
        }
        else {
            Debug.DrawRay(pos, direction * maxDistance, Color.white, Time.fixedDeltaTime);
            return maxDistance;
        }
    }
}
