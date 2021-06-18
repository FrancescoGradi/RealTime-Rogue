using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBall : MonoBehaviour {
    public LayerMask enemyLayers;
    public float radius = 0.1f;
    public float speed = 0.3f;
    public int damages = 8;

    public GameObject explosion;

    void Start() {
        
        Object.Destroy(this.gameObject, 6f);
    }

    void FixedUpdate() {

        this.gameObject.transform.Translate(0, 0, speed);
        
        Collider[] hitPlayers = Physics.OverlapSphere(this.gameObject.transform.position, radius, enemyLayers);

        if (hitPlayers != null) {
            foreach(Collider player in hitPlayers) {
                if (player.GetComponent<Enemy>() != null) {
                    player.GetComponent<Enemy>().TakeDamage(damages, 0.1f);
                } else if (player.GetComponent<Player>() != null) {
                    player.GetComponent<Player>().TakeDamage(damages, 0.1f);
                }
                if (explosion != null) {
                    Vector3 explosionPos = player.gameObject.transform.position;
                    Instantiate(explosion, explosionPos, Quaternion.identity);
                }
                Object.Destroy(this.gameObject);
                break;
            }
        }
    }
}
