using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour {

    public Animator animator;
    public Transform attackPoint;
    public LayerMask playerLayer;
    public GameObject hitEffect;

    private Enemy enemy;
    private float nextAttackTime = 0f;

    private void Start() {
        enemy = GetComponent<Enemy>();
    }

    public void NormalAttack() {

        animator.SetTrigger("attack");
        animator.SetBool("attacking", true);

        StartCoroutine(AttackWaiter(0.5f));

        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, enemy.attackRange, playerLayer);

        foreach(Collider player in hitPlayers) {
            int damage = enemy.actualWeaponDamage + enemy.ATK;
            if (player.GetComponent<Enemy>() != null)
                player.GetComponent<Enemy>().TakeDamage(damage, 0.1f);
            else if (player.GetComponent<Player>() != null)
                player.GetComponent<Player>().TakeDamage(damage, 0.1f);
            StartCoroutine(HitEffect(player.transform.position, 0.1f));
            break;
        }
    }

    private IEnumerator AttackWaiter(float seconds) {
        
        yield return new WaitForSeconds(seconds);
        animator.SetBool("attacking", false);
    }
    private IEnumerator HitEffect(Vector3 pos, float seconds) {

        yield return new WaitForSeconds(seconds);

        GameObject hit = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(hit, 0.5f);
    }
}
