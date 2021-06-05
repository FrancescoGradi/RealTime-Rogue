using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour {
    
    private CharacterController characterController;
    private Animator animator;

    public Player player;
    public LayerMask itemsLayer;
    public Transform cam;
    public ItemsHub itemsHub;
    public Smoke smokeEffect;

    public float turnSmoothTime = 0.2f;
    public float gravity = 20.0f;

    private float turnSmoothVelocity;
    private float horizontal = 0f;
    private float vertical = 0f;
    private Vector3 direction = new Vector3(0, 0, 0);
    private float nextSprintTime = 0f;


    private void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate() {

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f && !animator.GetBool("attacking")) {
            
            if (Input.GetButton("Sprint") && Time.time >= nextSprintTime && direction.magnitude >= 0.1f) {
                player.speed *= 10;

                player.GetComponent<BoxCollider>().enabled = false;

                StartCoroutine(SprintWaiter(0.03f));

                if (smokeEffect != null)
                    Instantiate(smokeEffect, this.gameObject.transform.position, Quaternion.identity);

                nextSprintTime = Time.time + 1f;
            }

            animator.SetInteger("running", 1);
            
            Movement();

        }  else {
            animator.SetInteger("running", 0);
        }
    }

    private void Movement() {

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        characterController.Move(moveDir.normalized * player.speed * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        
        Item actualItem = hit.gameObject.GetComponent<Item>();
        if (actualItem != null) {
            CollectItem(actualItem);
        }
    }

    private void CollectItem(Item actualItem) {

        if (actualItem.GetComponent<BastardSword>() != null) {

            itemsHub.CollectedItem(actualItem.itemName);
            itemsHub.SetSword(actualItem.itemName);

            BastardSword weapon = actualItem.GetComponent<BastardSword>();
            player.SetWeapon(actualItem.name, weapon.bonusATK, weapon.material);
            
            actualItem.gameObject.SetActive(false);

        } else if (actualItem.GetComponent<GoldenShield>() != null) {

            itemsHub.CollectedItem(actualItem.itemName);
            itemsHub.SetShield(actualItem.itemName);

            GoldenShield shield = actualItem.GetComponent<GoldenShield>();
            player.SetShield(actualItem.name, shield.bonusDEF, shield.material);

            actualItem.gameObject.SetActive(false);

        } else if (actualItem.GetComponent<HealthPotion>() != null || actualItem.GetComponent<BonusPotion>() != null) {
            
            itemsHub.CollectedItem(actualItem.itemName);
            itemsHub.SetPotion(actualItem.itemName);

            player.SetActualPotion(actualItem);
            actualItem.gameObject.SetActive(false);
        } 
    }

    private IEnumerator SprintWaiter(float seconds) {
        
        yield return new WaitForSecondsRealtime(seconds);

        player.GetComponent<BoxCollider>().enabled = true;
        player.speed /= 10;
    }
    
}