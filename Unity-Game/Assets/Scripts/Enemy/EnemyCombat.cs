﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour {

    public Animator animator;
    public Transform attackPoint;
    public LayerMask playerLayer;


    private Enemy enemy;
    private float nextAttackTime = 0f;

    private void Start() {
        enemy = GetComponent<Enemy>();
    }

    public void NormalAttack() {

        animator.SetTrigger("attack");
        animator.SetBool("attacking", true);

        StartCoroutine(AttackWaiter(0));

        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, enemy.attackRange, playerLayer);

        foreach(Collider player in hitPlayers) {
            player.GetComponent<Target>().TakeDamage(enemy.actualWeaponDamage + enemy.ATK, 0f);
        }
    }

    private IEnumerator AttackWaiter(float seconds) {
        
        yield return new WaitForSecondsRealtime(seconds);
        animator.SetBool("attacking", false);
    }
}
