using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    public Player player;
    public LayerMask itemsLayer;
    public Transform cam;
    public ItemsHub itemsHub;

    public float turnSmoothTime = 0.2f;
    public float gravity = 20.0f;

    private float turnSmoothVelocity;
    private float horizontal = 0f;
    private float vertical = 0f;
    private Vector3 direction = new Vector3(0, 0, 0);
    private float nextSprintTime = 0f;

    private int count = 0;
    private int updateRate = 20;


    private void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate() {

        if (Input.GetKey(KeyCode.Q) && Time.time >= nextSprintTime) {
            Sprint(4f);
            nextSprintTime = Time.time + 1f;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f && !animator.GetBool("attacking")) {
            animator.SetBool("running", true);
            animator.SetInteger("condition", 1);
            
            Movement();

        }  else {
            animator.SetBool("running", false);
            animator.SetInteger("condition", 0);
        }

        if (!characterController.isGrounded) {
            characterController.Move(new Vector3(0, - gravity * Time.fixedDeltaTime, 0));

            if (characterController.velocity.y < - 25 * gravity * Time.fixedDeltaTime)
                FindObjectOfType<GameManager>().GameOver();
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
        
        if (actualItem.itemName == "Bastard Sword") {

            itemsHub.CollectedItem(actualItem.itemName);
            itemsHub.SetBastardSword();

            BastardSword weapon = actualItem.GetComponent<BastardSword>();
            player.SetWeapon(actualItem.name, weapon.bonusATK, weapon.material);
            
            Destroy(actualItem.gameObject);

        } else if (actualItem.itemName == "Golden Shield") {

            itemsHub.CollectedItem(actualItem.itemName);
            itemsHub.SetGoldShield();

            GoldenShield shield = actualItem.GetComponent<GoldenShield>();
            player.SetShield(actualItem.name, shield.material);

            Destroy(actualItem.gameObject);

        } else if (actualItem.itemName == "Health Potion") {

            itemsHub.CollectedItem(actualItem.itemName);
            itemsHub.SetHealthPotion();

            player.SetActualPotion(actualItem.itemName);

            Destroy(actualItem.gameObject);

        } else if (actualItem.itemName == "Bonus Potion") {
            
            itemsHub.CollectedItem(actualItem.itemName);
            itemsHub.SetBonusPotion();

            player.SetActualPotion(actualItem.itemName);

            Destroy(actualItem.gameObject);
        }
    }

    private void Sprint(float distance) {
        this.gameObject.transform.Translate(new Vector3(0, 0, distance));
    }
    
}