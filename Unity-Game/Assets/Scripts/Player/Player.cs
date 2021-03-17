using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public HealthBar playerHealthBar;
    public ItemsHub itemsHub;

    public int HP = 50;
    public int ATK = 3;
    public int MANA = 3;
    public int DEF = 3;
    
    public int currentHealth;
    public float speed = 6.0f;

    public float attackRange = 1.5f;
    public float attackRate = 2f;

    public string weaponName = "Long Sword";
    public int actualWeaponDamage = 6;
    public MeshRenderer sword;
    public MeshRenderer shield;

    private string actualPotion;

    private void Start() {

        currentHealth = HP - 15;
        playerHealthBar.SetMaxHealth(HP);
        playerHealthBar.SetHealth(currentHealth);
    }

    private void Update() {

        if (actualPotion != null && Input.GetAxis("Drink") > 0) {
            DrinkPotion();
        }
    }

    public void SetWeapon(string weaponName, int bonusATK, Material material) {
        this.weaponName = weaponName;
        this.actualWeaponDamage += bonusATK;

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
            if (currentHealth > HP) {
                currentHealth = HP;
            }
            playerHealthBar.SetHealth(currentHealth);
        }

        actualPotion = null;

        itemsHub.DestroyActualPotion();

    }
}
