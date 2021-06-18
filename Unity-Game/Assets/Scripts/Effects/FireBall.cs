using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {
    public LayerMask enemyLayers;
    public float radius = 0.1f;
    public float speed = 0.3f;
    public int damages = 8;

    public GameObject explosion;

    void Start() {
        
        Object.Destroy(this.gameObject, 5f);
    }

    void FixedUpdate() {

        this.gameObject.transform.Translate(0, 0, speed);
        
        Collider[] hitEnemies = Physics.OverlapSphere(this.gameObject.transform.position, radius, enemyLayers);

        if (hitEnemies != null) {
            foreach(Collider enemy in hitEnemies) {
                if (enemy.GetComponent<Enemy>() != null) {
                    enemy.GetComponent<Enemy>().TakeDamage(damages, 0.1f);
                }
                if (explosion != null) {
                    Vector3 explosionPos = enemy.gameObject.transform.position;
                    Instantiate(explosion, explosionPos, Quaternion.identity);
                }
                Object.Destroy(this.gameObject);
                break;
            }
        }
    }
}
