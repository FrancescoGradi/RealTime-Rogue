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
    public float assistedFireBallAngle = 45;
    public float assistedRange = 15f;

    private float nextAttackTime = 0f;
    public event System.Action OnAttack;

    private void FixedUpdate() {

        if (Time.time >= nextAttackTime) {

            if (Input.GetButton("Fire1")) {
                Attack();
                nextAttackTime = Time.time + 1f / player.attackRate;
            }

            if (Input.GetButton("Fire2")) {
                SpecialAttack();
                nextAttackTime = Time.time + 4f / player.attackRate;
            }

            if (Input.GetButton("Fire3")) {
                MagicAttack();
                nextAttackTime = Time.time + 4f / player.attackRate;
            }
        }
    }

    private void Attack() {
        animator.SetTrigger("attack");
        animator.SetBool("attacking", true);

        StartCoroutine(AttackWaiter(2f));

        if (OnAttack != null)
            OnAttack();

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, player.attackRange, enemyLayers);

        foreach(Collider enemy in hitEnemies) {
            enemy.GetComponent<Enemy>().TakeDamage(player.actualWeaponDamage + player.ATK, 0.3f);
        }
    }
    private void SpecialAttack() {
        animator.SetTrigger("greatAttack");
        animator.SetBool("attacking", true);

        StartCoroutine(AttackWaiter(1f));

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, player.attackRange * 3, enemyLayers);

        foreach(Collider enemy in hitEnemies) {
            enemy.GetComponent<Enemy>().TakeDamage((player.actualWeaponDamage + player.ATK) * 2, 0.9f);
        }
    }

    private void MagicAttack() {

        Collider[] hitEnemies = Physics.OverlapSphere(this.gameObject.transform.position, assistedRange, enemyLayers);

        foreach (Collider enemy in hitEnemies) {
            if (EnemyInFieldOfView(enemy.gameObject)) {
                Vector3 targetDir = enemy.gameObject.transform.position - this.gameObject.transform.position;
                Vector3 forward = this.gameObject.transform.forward;

                float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
                this.gameObject.transform.Rotate(0, -angle, 0);
                break;
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
        new_fireball.GetComponent<FireBall>().damages = player.MANA;
    }

    private void OnDrawGizmosSelected() {
        
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, player.attackRange);
    }

    private bool EnemyInFieldOfView(GameObject enemy) {

        Vector3 targetDir = enemy.gameObject.transform.position - this.gameObject.transform.position;
        float angle = Vector3.SignedAngle(targetDir, this.gameObject.transform.forward, Vector3.up);

        if (angle < assistedFireBallAngle && angle > - assistedFireBallAngle) {
            return true;
        } else {
            return false;
        }
    }

}
