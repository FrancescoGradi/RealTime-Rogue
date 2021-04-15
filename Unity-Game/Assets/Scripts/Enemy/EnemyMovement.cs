using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MLAgents;

public class EnemyMovement : MonoBehaviour {

    private CharacterController characterController;
    private Animator animator;
    private EnemyAgent agent;
    private Enemy enemy;

    private float horizontal;
    private float vertical;

    public float epsilon = 1f;

    public GameObject target;
    public int envObjectsLayer = 12;
    public int itemsLayer = 10;


    public float speed = 4f;
    public float turnSmoothTime = 0.2f;

    public float fieldOfViewTargetAngle = 60f;

    private Vector3 direction = new Vector3(0, 0, 0);
    private int count = 0;
    public int updateRate = 20;
    private float turnSmoothVelocity;

    private bool targetReached = false;


    void Start() {

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<EnemyAgent>();
        enemy = GetComponent<Enemy>();
    }

    private void FixedUpdate() {
        
        if (!targetReached && !animator.GetBool("attacking")) {

            count += 1;
            if (count == updateRate || count > 1000) {
                agent.RequestDecision();
                count = 0;
            }
            
            if (direction.magnitude >= 0.05f) {
                animator.SetInteger("running", 1);
                Movement();
            } else {
                animator.SetInteger("running", 0);
            }
            /*
            if (!targetReached) {
                if (System.Math.Abs(this.gameObject.transform.position.z - target.transform.position.z) < epsilon && 
                    System.Math.Abs(this.gameObject.transform.position.x - target.transform.position.x) < epsilon) {
                    targetReached = true;
                    agent.TargetReached();
                }
            }
            */
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

    private void OnControllerColliderHit(ControllerColliderHit hit) {

        Item actualItem = hit.gameObject.GetComponent<Item>();
        if (actualItem != null) {
            CollectItem(actualItem);
        }
    }

    private void CollectItem(Item actualItem) {

        if (actualItem.itemName == "Health Potion") {

            enemy.SetActualPotion(actualItem.itemName);

            Destroy(actualItem.gameObject);

            enemy.DrinkPotion();
        }
    }

    public List<float> GetRayCastDistance(float maxDistance, float angle) {

        RaycastHit hit;
        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
        Vector3 pos = this.gameObject.transform.position;
        pos.y += 1.5f;

        if (Physics.Raycast(pos, direction, out hit, maxDistance)) {

            if (hit.transform.gameObject.layer == envObjectsLayer) {
                Debug.DrawRay(pos, direction * hit.distance, Color.yellow, Time.fixedDeltaTime);
                return new List<float> {hit.distance / maxDistance, 0, 1, 0};
            } else if (hit.transform.gameObject.layer == itemsLayer) {
                Debug.DrawRay(pos, direction * hit.distance, Color.red, Time.fixedDeltaTime);
                return new List<float> {hit.distance / maxDistance, 0, 0, 1};
            } else {
                Debug.DrawRay(pos, direction * maxDistance, Color.white, Time.fixedDeltaTime);
                return new List<float> {1, 1, 0, 0};
            }     
        } else {
            Debug.DrawRay(pos, direction * maxDistance, Color.white, Time.fixedDeltaTime);
            return new List<float> {1, 1, 0, 0};
        }
    }

    public float IsInRange() {

        // Verifico che il target si trovi davanti rispetto al mio agente

        Vector3 targetDir = target.gameObject.transform.position - this.gameObject.transform.position;
        float angle = Vector3.SignedAngle(targetDir, this.gameObject.transform.forward, Vector3.up);

        // Se il target si trova in quel determinato angolo rispetto all'agente ed e' sufficientemente vicino,
        // allora si trova nel range per l'attacco

        if (angle < fieldOfViewTargetAngle && angle > -fieldOfViewTargetAngle &&
            System.Math.Abs(this.gameObject.transform.position.z - target.transform.position.z) < epsilon && 
            System.Math.Abs(this.gameObject.transform.position.x - target.transform.position.x) < epsilon) {
            return 1f;
        } else {
            return 0f;
        }
    }
}
