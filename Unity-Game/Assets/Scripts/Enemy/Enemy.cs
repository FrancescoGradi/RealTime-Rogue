using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public EnemyHealthBar healthBar;

    public int HP = 20;
    public int ATK = 3;
    public int MANA = 3;
    public int DEF = 3;
    
    public int currentHealth;
    public float speed = 6.0f;

    public float attackRange = 1.5f;
    public float attackRate = 2f;

    public string weaponName = "Long Sword";
    public int actualWeaponDamage = 6;

    void Start()
    {
        currentHealth = HP;
        healthBar.SetMaxHealth(HP);
    }

    public void TakeDamage(int damage, float delay) {

        StartCoroutine(DamageWaiter(damage, delay));
    }

    private void Die() {
        Debug.Log("Enemy died");
        animator.SetTrigger("dead");

        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        FindObjectOfType<EnemyGenerator>().EnemyDown();

        Destroy(gameObject, 4f);
    }

    
    private IEnumerator DamageWaiter(int damage, float seconds) {
        
        yield return new WaitForSecondsRealtime(seconds);

        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) {
            Die();
        } else {
            animator.SetTrigger("hit");
        }
    }
}
