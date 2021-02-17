using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public HealthBar playerHealthBar;
    public ItemsHub itemsHub;

    public int maxHealth;
    public int currentHealth;
    public float speed = 6.0f;
    public int damage = 5;
    public float attackRange = 1f;
    public float attackRate = 2f;
    public string weaponName = "Long Sword";
    public MeshRenderer sword;
    public MeshRenderer shield;

    private string actualPotion;

    private void Start() {

        currentHealth = maxHealth - 15;
        playerHealthBar.SetMaxHealth(maxHealth);
        playerHealthBar.SetHealth(currentHealth);
    }

    private void Update() {

        if (Input.GetKey(KeyCode.E) && actualPotion != null) {
            DrinkPotion();
        }
    }

    public void SetWeapon(string weaponName, int damage, float attackRange, float attackRate, Material material) {
        this.weaponName = weaponName;
        this.damage = damage;
        this.attackRange = attackRange;
        this.attackRate = attackRate;
        sword.material = material;
    }

    internal void SetShield(string name, Material material) {
        shield.material = material;
    }

    public void SetActualPotion(string actualPotion) {
        this.actualPotion = actualPotion;
    }

    private void DrinkPotion() {

        if (actualPotion == "Health Potion") {
            currentHealth += 10;
            if (currentHealth > maxHealth) {
                currentHealth = maxHealth;
            }
            playerHealthBar.SetHealth(currentHealth);
        }

        actualPotion = null;

        itemsHub.DestroyActualPotion();

    }
}
