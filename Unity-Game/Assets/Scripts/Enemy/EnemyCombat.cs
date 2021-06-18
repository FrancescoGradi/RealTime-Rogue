using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour {

    public Animator animator;
    public Transform attackPoint;
    public LayerMask playerLayer;
    public GameObject electricBall;
    public GameObject hitEffect;
    public float assistedAngle = 175;
    public float assistedDistance = 3f;
    public float assistedRange = 15f;

    private Enemy enemy;
    private EnemyMovement enemyMovement;

    private void Start() {
        enemy = GetComponent<Enemy>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void NormalAttack() {

        animator.SetTrigger("attack");
        animator.SetBool("attacking", true);

        StartCoroutine(AttackWaiter(0.5f));

        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, enemy.attackRange, playerLayer);

        foreach(Collider player in hitPlayers) {
            
            // Mira assistita per i colpi in mischia
            if (Vector3.Distance(enemy.gameObject.transform.position, player.gameObject.transform.position) < assistedDistance) {
                Vector3 targetDir = player.gameObject.transform.position - this.gameObject.transform.position;
                Vector3 forward = this.gameObject.transform.forward;

                float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
                this.gameObject.transform.Rotate(0, -angle, 0);
            }

            int damage = enemy.actualWeaponDamage + enemy.ATK;
            if (player.GetComponent<Enemy>() != null)
                player.GetComponent<Enemy>().TakeDamage(damage, 0.1f);
            else if (player.GetComponent<Player>() != null)
                player.GetComponent<Player>().TakeDamage(damage, 0.1f);
            StartCoroutine(HitEffect(player.transform.position, 0.1f));
            break;
        }
    }

    public void MagicAttack() {
        
        if (Vector3.Distance(enemy.gameObject.transform.position, enemyMovement.target.gameObject.transform.position) < assistedRange) {
            Vector3 targetDir = enemyMovement.target.gameObject.transform.position - this.gameObject.transform.position;
            Vector3 forward = this.gameObject.transform.forward;

            float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
            this.gameObject.transform.Rotate(0, -angle, 0);
        }

        animator.SetTrigger("magic");
        animator.SetBool("attacking", true);

        StartCoroutine(MagicWaiter(0.5f));
    }

    private IEnumerator AttackWaiter(float seconds) {
        
        yield return new WaitForSeconds(seconds);
        animator.SetBool("attacking", false);
    }

    private IEnumerator MagicWaiter(float seconds) {
        
        yield return new WaitForSeconds(seconds);
        
        if (enemyMovement.playerLayer == 11) {
            GameObject new_electriball = Instantiate(electricBall, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1, enemy.transform.position.z), enemy.transform.rotation);
            new_electriball.GetComponent<ElectricBall>().damages = enemy.MANA + 7;
        } else if (enemyMovement.playerLayer == 9) {
            GameObject new_fireball = Instantiate(electricBall, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1, enemy.transform.position.z), enemy.transform.rotation);
            new_fireball.GetComponent<FireBall>().damages = enemy.MANA + 7;
        }

        animator.SetBool("attacking", false);
    }

    private IEnumerator HitEffect(Vector3 pos, float seconds) {

        yield return new WaitForSeconds(seconds);

        GameObject hit = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(hit, 0.5f);
    }

    private bool EnemyInFieldOfView(GameObject enemy) {

        Vector3 targetDir = enemy.gameObject.transform.position - this.gameObject.transform.position;
        float angle = Vector3.SignedAngle(targetDir, this.gameObject.transform.forward, Vector3.up);

        if (angle < assistedAngle && angle > - assistedAngle) {
            return true;
        } else {
            return false;
        }
    }
}
