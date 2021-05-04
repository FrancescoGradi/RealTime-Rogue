using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MLAgents;

public class Target : MonoBehaviour {

    public int updateMovement = 100;
    public float translateFactor = 1f;
    public float speed = 6f;
    public int HP = 10;
    public int currentHealth = 5;

    private int count = 0;
    private CharacterController characterController;
    private Animator animator;
    private Vector3 direction = new Vector3(0, 0, 0);
    private float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;
    private bool moving = false;
    private bool isDown = false;
    public EnemyHealthBar healthBar;



    private void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentHealth = HP;
        healthBar.SetMaxHealth(HP);
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

        if (moving && !animator.GetBool("attacking")) {
            animator.SetInteger("running", 1);
            Movement();
        } else {
            animator.SetInteger("running", 0);
        }
            
    }

    public void ResetPosition(Vector3 pos, int maxHP) {

        HP = maxHP;
        currentHealth = HP;
        healthBar.SetMaxHealth(currentHealth);
        healthBar.SetHealth(currentHealth);

        direction = new Vector3(0, 0, 0);
        isDown = false;
        moving = false;
        this.gameObject.transform.position = pos;

    }

    public void Movement() {

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        characterController.Move(moveDir.normalized * speed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage, float delay) {

        StartCoroutine(DamageWaiter(damage, delay));
    }

    private void ChangeRandomMovement() {

        float horizontal = 0f;
        float vertical = 0f;

        // Nessun movimento e movimento in ciascuna delle 16 direzioni cardinali + fermo

        switch ((int)(UnityEngine.Random.Range(0, 17))) {

            case 0:
                horizontal = 0f;
                vertical = 0f;
                break;
            case 1:
                horizontal = 1f;
                vertical = 0f;
                break;
            case 2:
                horizontal = 0.924f;
                vertical = 0.382f;
                break;
            case 3:
                horizontal = 0.707f;
                vertical = 0.707f;
                break;
            case 4:
                horizontal = 0.382f;
                vertical = 0.924f;
                break;
            case 5:
                horizontal = 0f;
                vertical = 1f;
                break;
            case 6:
                horizontal = -0.382f;
                vertical = 0.924f;
                break;
            case 7:
                horizontal = -0.707f;
                vertical = 0.707f;
                break;
            case 8:
                horizontal = -0.924f;
                vertical = 0.382f;
                break;
            case 9:
                horizontal = -1f;
                vertical = 0f;
                break;
            case 10:
                horizontal = -0.924f;
                vertical = -0.382f;
                break;
            case 11:
                horizontal = -0.707f;
                vertical = -0.707f;
                break;
            case 12:
                horizontal = -0.382f;
                vertical = -0.924f;
                break;
            case 13:
                horizontal = 0f;
                vertical = -1f;
                break;
            case 14:
                horizontal = 0.382f;
                vertical = -0.924f;
                break;
            case 15:
                horizontal = 0.707f;
                vertical = -0.707f;
                break;
            case 16:
                horizontal = 0.924f;
                vertical = -0.382f;
                break;
        }

        direction = new Vector3(horizontal, 0, vertical);
    }

    private IEnumerator DamageWaiter(int damage, float seconds) {
        
        yield return new WaitForSeconds(seconds);

        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (!isDown && currentHealth <= 0) {
            isDown = true;
            StartCoroutine(PlayerDownWaiter(0f));
        }
    }

    private IEnumerator PlayerDownWaiter(float seconds) {

        yield return new WaitForSeconds(seconds);

        FindObjectOfType<EnemyAgent>().PlayerDown();
    }
}
