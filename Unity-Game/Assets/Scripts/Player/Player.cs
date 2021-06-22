﻿using UnityEngine;
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

    public float attackRange = 2f;
    public float attackRate = 2f;

    public string weaponName = "Long Sword";
    public MeshRenderer sword;
    public int actualWeaponDamage = 6;

    public string shieldName = "Wood Shield";
    public MeshRenderer shield;
    public int actualShieldDef = 2;

    public Spikes spikes;
    public HealthPotionCircleEffect healthPotionCircleEffect;

    private Item actualPotion;
    private bool actualPotionActive;

    private void Start() {

        currentHealth = HP;
        actualPotionActive = false;

        if (playerHealthBar != null) {
            playerHealthBar.SetMaxHealth(HP);
            playerHealthBar.SetHealth(currentHealth);
        }
    }

    private void Update() {

        if (actualPotion != null && Input.GetAxis("Drink") > 0 && !actualPotionActive) {
            DrinkPotion();
        }
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
    }

    private void DrinkPotion() {

        if (actualPotion != null) {

            if (actualPotion.GetComponent<HealthPotion>() != null) {
            
                AddHealth(actualPotion.bonusHP);
                Destroy(actualPotion.gameObject);

            } else if (actualPotion.GetComponent<BonusPotion>() != null) {

                itemsHub.DrinkPotionCanvasFeedback("+5 All Stats");

                actualPotionActive = true;

                ATK += actualPotion.bonusATK;
                MANA += actualPotion.bonusMANA;
                DEF += actualPotion.bonusDEF;
                speed += 2f;

                Spikes tmp = Instantiate(spikes, this.gameObject.transform.position, Quaternion.identity);
                tmp.gameObject.transform.SetParent(this.gameObject.transform);

                StartCoroutine(WaitBonusPotion(5f));
            }
        }

        itemsHub.DestroyActualPotion();
    }

    public void TakeDamage(int damage, float delay) {

        // Riduzione del danno per lo scudo
        damage -= (DEF + actualShieldDef);

        if (damage < 0)
            damage = 0;

        StartCoroutine(DamageWaiter(damage, delay));
    }

    public void AddHealth(int health) {

        itemsHub.DrinkPotionCanvasFeedback("+" + health.ToString() + " HP");

        currentHealth += health;
        if (currentHealth > HP) {
            currentHealth = HP;
        }
        playerHealthBar.SetHealth(currentHealth);

        HealthPotionCircleEffect tmp = Instantiate(healthPotionCircleEffect, this.gameObject.transform.position, healthPotionCircleEffect.gameObject.transform.rotation);
        Vector3 pos = tmp.gameObject.transform.position;
        pos.y = 0.2f;
        tmp.gameObject.transform.position = pos;
        tmp.gameObject.transform.SetParent(this.gameObject.transform);
    }

    private IEnumerator WaitBonusPotion(float seconds) {

        yield return new WaitForSeconds(seconds);

        ATK -= actualPotion.bonusATK;
        MANA -= actualPotion.bonusMANA;
        DEF -= actualPotion.bonusDEF;;
        speed -= 2f;

        Destroy(actualPotion.gameObject);
        actualPotionActive = false;
        itemsHub.DrinkPotionCanvasFeedback("Bonus Potion Over!");
    }
    
    private IEnumerator DamageWaiter(int damage, float seconds) {
        
        yield return new WaitForSeconds(seconds);

        currentHealth -= damage;

        playerHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) {
            // TO-DO Gestire la morte del player con schermata di GAME-OVER
            Debug.Log("Player ko!");
        }
    }
}
