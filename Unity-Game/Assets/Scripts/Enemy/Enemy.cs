using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public EnemyHealthBar healthBar;

    public int maxHealth = 20;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage) {

        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) {
            Die();
        } else {
            animator.SetTrigger("hit");
        }
    }

    private void Die() {
        Debug.Log("Enemy died");
        animator.SetTrigger("dead");

        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        FindObjectOfType<EnemyGenerator>().EnemyDown();

        Destroy(gameObject, 4f);
    }

}
