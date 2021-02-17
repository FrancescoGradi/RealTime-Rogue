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
    private float horizontal;
    private float vertical;
    private Vector3 direction;
    private float nextSprintTime = 0f;


    private void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update() {

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
            characterController.Move(new Vector3(0, - gravity * Time.deltaTime, 0));

            if (characterController.velocity.y < - 25 * gravity * Time.deltaTime)
                FindObjectOfType<GameManager>().GameOver();
        }
    }

    private void FixedUpdate() {
        CollectItems();
    }

    private void Movement() {

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        characterController.Move(moveDir.normalized * player.speed * Time.deltaTime);
    }

    private void CollectItems() {
        
        Collider[] items = Physics.OverlapBox(gameObject.transform.position, transform.localScale, Quaternion.identity, itemsLayer);

        foreach (Collider item in items) {
            
            Item actualItem = item.GetComponent<Item>();
            itemsHub.CollectedItem(actualItem.itemName);

            if (actualItem.itemName == "Bastard Sword") {
                itemsHub.SetBastardSword();
                BastardSword weapon = item.GetComponent<BastardSword>();
                player.SetWeapon(item.name, weapon.damage, weapon.attackRange, weapon.attackRate, weapon.material);
            } else if (actualItem.itemName == "Golden Shield") {
                itemsHub.SetGoldShield();
                GoldenShield shield = item.GetComponent<GoldenShield>();
                player.SetShield(item.name, shield.material);
            } else if (actualItem.itemName == "Health Potion") {
                itemsHub.SetHealthPotion();
                player.SetActualPotion(actualItem.itemName);
            } else if (actualItem.itemName == "Bonus Potion") {
                itemsHub.SetBonusPotion();
                player.SetActualPotion(actualItem.itemName);
            }

            Destroy(item.gameObject);
        }
    }

    private void Sprint(float distance) {
        this.gameObject.transform.Translate(new Vector3(0, 0, distance));
    }
    
}