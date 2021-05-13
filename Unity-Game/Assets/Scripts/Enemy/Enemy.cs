﻿using System.Collections;
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
    public int actualWeaponDamage = 6;

    private string actualPotion;

    public HealthPotionCircleEffect healthPotionCircleEffect;

    private List<int> initialRandomHP = new List<int>{5, 10, 15, 20};

    void Start() {
        currentHealth = HP;
        healthBar.SetMaxHealth(HP);
    }

    public void TakeDamage(int damage, float delay) {

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

        actualPotion = null;

        animator.SetBool("attacking", false);
    }

    private void Die() {

        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        FindObjectOfType<EnemyGenerator>().EnemyDown();

        Destroy(gameObject, 4f);
    }
    
    public void SetActualPotion(string actualPotion) {
        this.actualPotion = actualPotion;
    }

    public bool HasActualPotion() {
        if (actualPotion != null) {
            return true;
        } else {
            return false;
        }
    }

    public void DrinkPotion() {
        
        if (actualPotion == "Health Potion") {
            
            // L'idea è vedere se l'agente riesce a capire se gli conviene perdere tempo per prendere una pozione, oppure no

            if (currentHealth >= HP) {
                currentHealth = HP;
            } else {
                currentHealth += 5;
                if (currentHealth > HP)
                    currentHealth = HP;
                healthBar.SetHealth(currentHealth);
            }
            
            HealthPotionAnimation();
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
        }
    }
}
