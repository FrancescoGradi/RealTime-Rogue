using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public Player player;
    public GameObject fireball;
    public float assistedFireBallAngle = 60;
    public float assistedRange = 15f;

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime) {

            if (Input.GetKey(KeyCode.Space)) {
                Attack();
                nextAttackTime = Time.time + 1f / player.attackRate;
            }

            if (Input.GetKey(KeyCode.F)) {
                GreatAttack();
                nextAttackTime = Time.time + 4f / player.attackRate;
            }

            if (Input.GetKey(KeyCode.V)) {
                MagicAttack();
                nextAttackTime = Time.time + 4f / player.attackRate;
            }
        }
    }

    private void Attack() {
        animator.SetTrigger("attack");
        animator.SetBool("attacking", true);

        StartCoroutine(AttackWaiter(1f));

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, player.attackRange, enemyLayers);

        foreach(Collider enemy in hitEnemies) {
            Debug.Log("Hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(player.damage);
        }
    }
    private void GreatAttack() {
        animator.SetTrigger("greatAttack");
        animator.SetBool("attacking", true);

        StartCoroutine(AttackWaiter(1f));

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, player.attackRange, enemyLayers);

        foreach(Collider enemy in hitEnemies) {
            Debug.Log("Hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(player.damage * 2);
        }
    }

    private void MagicAttack() {

        Collider[] hitEnemies = Physics.OverlapSphere(this.gameObject.transform.position, assistedRange, enemyLayers);

        foreach (Collider enemy in hitEnemies) {
            if (EnemyInFieldOfView(enemy.gameObject)) {
                Vector3 targetDir = enemy.gameObject.transform.position - this.gameObject.transform.position;
                float angle = Vector3.Angle(targetDir, this.gameObject.transform.forward);
                this.gameObject.transform.Rotate(0, angle, 0);
            }
        }

        animator.SetTrigger("magic");
        animator.SetBool("attacking", true);

        StartCoroutine(MagicWaiter(1f));
    }

    private IEnumerator AttackWaiter(float seconds) {
        
        yield return new WaitForSecondsRealtime(seconds);
        animator.SetBool("attacking", false);
    }

    private IEnumerator MagicWaiter(float seconds) {
        
        yield return new WaitForSecondsRealtime(seconds);
        animator.SetBool("attacking", false);
        GameObject new_fireball = Instantiate(fireball, new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z + 1f), player.transform.rotation);
        new_fireball.GetComponent<FireBall>().enemyLayers = enemyLayers;
    }

    private void OnDrawGizmosSelected() {
        
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, player.attackRange);
    }

    private bool EnemyInFieldOfView(GameObject enemy) {
        Vector3 targetDir = enemy.gameObject.transform.position - this.gameObject.transform.position;

        float angle = Vector3.Angle(targetDir, this.gameObject.transform.forward);

        if (angle < assistedFireBallAngle) {
            return true;
        } else {
            return false;
        }
    }

}
