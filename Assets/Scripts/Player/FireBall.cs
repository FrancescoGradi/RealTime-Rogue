using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public LayerMask enemyLayers;
    public float fireballRadius = 1.5f;
    public float fireballSpeed = 0.15f;
    private float nextHitTime = 0f;
    private float hitTime = 0.2f;
    private List<string> enemyHit = new List<string>();

    void Start() {
        
        Object.Destroy(this.gameObject, 6f);
    }

    void FixedUpdate() {

        this.gameObject.transform.Translate(0, 0, fireballSpeed);

        if (Time.time >= nextHitTime) {
            Collider[] hitEnemies = Physics.OverlapSphere(this.gameObject.transform.position, 1.5f, enemyLayers);

            if (hitEnemies != null) {
                foreach(Collider enemy in hitEnemies) {
                    if (!enemyHit.Contains(enemy.name)) {
                        Debug.Log("Hit " + enemy.name);
                        enemy.GetComponent<Enemy>().TakeDamage(5);
                        enemyHit.Add(enemy.name);
                    }
                }
            }
            nextHitTime = Time.time + hitTime;
        }
    }
}
