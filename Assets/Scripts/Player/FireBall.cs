using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public LayerMask enemyLayers;
    public float fireballRadius = 1.5f;
    public float fireballSpeed = 0.15f;
    public int damages = 8;

    void Start() {
        
        Object.Destroy(this.gameObject, 6f);
    }

    void FixedUpdate() {

        this.gameObject.transform.Translate(0, 0, fireballSpeed);

        
        Collider[] hitEnemies = Physics.OverlapSphere(this.gameObject.transform.position, 1.5f, enemyLayers);

        if (hitEnemies != null) {
            foreach(Collider enemy in hitEnemies) {
                Debug.Log("Hit " + enemy.name);
                enemy.GetComponent<Enemy>().TakeDamage(damages);
                Object.Destroy(this.gameObject);
                break;
            }
        }
    }
}
