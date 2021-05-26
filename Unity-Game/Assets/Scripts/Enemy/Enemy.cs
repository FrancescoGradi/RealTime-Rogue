using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    public Animator animator;
    public EnemyHealthBar healthBar;
    public EnemyAgent agent;

    public int HP = 20;
    public int ATK = 3;
    public int MANA = 3;
    public int DEF = 3;
    
    public int currentHealth;
    public float speed = 6.0f;

    public float attackRange = 2f;
    public float attackRate = 3f;

    public string weaponName = "Long Sword";
    public MeshRenderer sword;
    public Material initialSwordMaterial;
    public int actualWeaponDamage = 6;

    public string shieldName = "Wood Shield";
    public MeshRenderer shield;
    public Material initialShieldMaterial;
    public int actualShieldDef = 2;


    private Item actualPotion;

    public GameObject healthPotionSprite;
    public HealthPotionCircleEffect healthPotionCircleEffect;

    private List<int> initialRandomHP = new List<int>{5, 10, 15, 20};

    void Start() {
        currentHealth = HP;
        healthBar.SetMaxHealth(HP);
    }

    public void TakeDamage(int damage, float delay) {
        
        // Riduzione del danno per lo scudo
        damage -= (DEF + actualShieldDef);

        if (damage < 0)
            damage = 0;

        StartCoroutine(DamageWaiter(damage, delay));
    }

    public void ResetStatsAndItems(bool randomHealth, int health, float speed) {

        // HP Iniziali con valore casuale: 5, 10, 15, 20
        if (randomHealth)
            currentHealth = initialRandomHP[(int)(UnityEngine.Random.Range(0, initialRandomHP.Count))];
        else
            currentHealth = health;

        if (healthBar.isActiveAndEnabled)
            healthBar.SetHealth(currentHealth);

        ATK = 3;
        MANA = 3;
        DEF = 3;

        this.speed = speed;

        weaponName = "Long Sword";
        actualWeaponDamage = 6;
        sword.material = initialSwordMaterial;

        shieldName = "Wood Shield";
        actualShieldDef = 2;
        shield.material = initialShieldMaterial;

        actualPotion = null;
        healthPotionSprite.SetActive(false);

        animator.SetBool("attacking", false);
    }

    private void Die() {

        animator.SetBool("dead", true);
        healthBar.gameObject.SetActive(false);

        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        FindObjectOfType<EnemyGenerator>().EnemyDown();

        Destroy(gameObject, 4f);
    }

    public void SetWeapon(string weaponName, int bonusATK, Material material) {
        this.weaponName = weaponName;
        this.actualWeaponDamage = bonusATK;

        sword.material = material;
    }

    public void SetShield(string shieldName, int bonusDEF, Material material) {
        this.shieldName = shieldName;
        this.actualShieldDef = bonusDEF;

        shield.material = material;
    }

    public void SetActualPotion(Item actualPotion) {

        if (this.actualPotion != null)
            Destroy(this.actualPotion.gameObject);
        
        this.actualPotion = actualPotion;

        if (this.actualPotion.GetComponent<HealthPotion>() != null) {
            healthPotionSprite.SetActive(true);
        }
    }

    public bool HasActualPotion() {
        if (actualPotion != null) {
            return true;
        } else {
            return false;
        }
    }

    public void DrinkPotion() {

        if (actualPotion != null) {

            if (actualPotion.GetComponent<HealthPotion>() != null) {

                agent.HealthPotionReward();
            
                if (currentHealth >= HP) {
                    currentHealth = HP;
                } else {
                    currentHealth += actualPotion.bonusHP;
                    if (currentHealth > HP)
                        currentHealth = HP;
                    healthBar.SetHealth(currentHealth);
                }

                Destroy(actualPotion.gameObject);
                
                healthPotionSprite.SetActive(false);
                HealthPotionAnimation();
            }

        }
        
        actualPotion = null;
    }

    private void HealthPotionAnimation() {

        HealthPotionCircleEffect tmp = Instantiate(healthPotionCircleEffect, this.gameObject.transform.position, healthPotionCircleEffect.gameObject.transform.rotation);
        Vector3 pos = tmp.gameObject.transform.position;
        pos.y = 0.2f;
        tmp.gameObject.transform.position = pos;
        tmp.gameObject.transform.SetParent(this.gameObject.transform);
    }

    
    private IEnumerator DamageWaiter(int damage, float seconds) {
        
        yield return new WaitForSeconds(seconds);

        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) {
            FindObjectOfType<EnemyAgent>().PlayerDown();
            // Die();
        }
    }
}
