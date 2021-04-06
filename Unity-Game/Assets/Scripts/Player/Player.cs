using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    #region Singleton

    public static Player instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one Player instance found!");
            return;
        }
        instance = this;
    }

    #endregion

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
    public Spikes spikes;

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
        this.actualWeaponDamage = bonusATK;

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

            AddHealth(5);
        } else if (actualPotion == "Bonus Potion") {

            itemsHub.DrinkPotionCanvasFeedback("+5 All Stats");

            ATK += 5;
            MANA += 5;
            DEF += 5;
            speed += 2f;

            Spikes tmp = Instantiate(spikes, this.gameObject.transform.position, Quaternion.identity);
            tmp.gameObject.transform.SetParent(this.gameObject.transform);

            StartCoroutine(WaitBonusPotion(5f));
        }

        actualPotion = null;

        itemsHub.DestroyActualPotion();

    }

    public void AddHealth(int health) {

        itemsHub.DrinkPotionCanvasFeedback("+" + health.ToString() + " HP");

        currentHealth += health;
        if (currentHealth > HP) {
            currentHealth = HP;
        }
        playerHealthBar.SetHealth(currentHealth);
    }

    private IEnumerator WaitBonusPotion(float seconds) {

        yield return new WaitForSecondsRealtime(seconds);

        ATK -= 5;
        MANA -= 5;
        DEF -= 5;
        speed -= 2f;

        itemsHub.DrinkPotionCanvasFeedback("Bonus Potion Over!");

    }
}
