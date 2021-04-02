﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using MLAgents;

public class EnemyAgent : Agent
{
    public Enemy enemy;
    public GameObject target;
    public RealTimeAcademy realTimeAcademy;

    private EnemyMovement enemyMovement;
    private EnemyCombat enemyCombat;
    public List<float> angles = new List<float>() {0f};
    public float raycastMaxDistance = 10f;
    public float rayMinDistance = 1f;

    void Start() {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyCombat = GetComponent<EnemyCombat>();
        // AgentReset();
    }

    public override void AgentReset() {

        if ((int) realTimeAcademy.resetParameters["agent_fixed"] == 1){
            enemy.gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        } else {
            enemy.gameObject.transform.SetPositionAndRotation(realTimeAcademy.GetRandomPosInRange(target, 0), Quaternion.identity);
        }

        enemyMovement.updateRate = (int) realTimeAcademy.resetParameters["agent_update_rate"];
        enemyMovement.epsilon = realTimeAcademy.resetParameters["attack_range_epsilon"];

        enemyMovement.SetTargetReached(false);
        enemyMovement.AddMovement(0, 0);
    }

    public override void CollectObservations() {
        
        List<float> obs = new List<float>();

        obs.Add(enemy.gameObject.transform.position.x);
        obs.Add(enemy.gameObject.transform.position.z);
        
        obs.Add(target.transform.position.x);
        obs.Add(target.transform.position.z);

        // Secondo modo: raggi di lunghezza massima che intersecano oggetti env e restituiscono la distanza
        
        for (int i = 0; i < angles.Count; i++) {
            float raycastDistance = enemyMovement.GetRayCastDistance(raycastMaxDistance, angles[i]);

            // Scoraggiamo il movimento verso un ostacolo
            if (raycastDistance < rayMinDistance) {
                AddReward(-1f);
            }

            obs.Add(raycastDistance);
        }

        // Booleano: se il target si trova nel range dell'agente, allora restituisce 1. Serve per aiutare l'agente
        // ad attaccare

        obs.Add(enemyMovement.IsInRange());

        AddVectorObs(obs);
    }


    // Caso vettore delle azioni CONTINUO
    /*

    public override void AgentAction(float[] vectorAction, string textAction) {

        float horizontal = vectorAction[0];
        float vertical = vectorAction[1];
        float attack = vectorAction[2];

        if (attack > 0) {
            enemyCombat.NormalAttack();
            enemyMovement.AddMovement(0, 0);
        } else {
            enemyMovement.AddMovement(horizontal, vertical);
        }

        AddReward(-0.1f);
    }

    */

    /*
    // Caso vettore delle azioni DISCRETO 8 + 1

    public override void AgentAction(float[] vectorAction, string textAction) {

        float horizontal = 0f;
        float vertical = 0f;

        // Nessun movimento e movimento in ciascuna delle 8 direzioni cardinali + attacco

        switch ((int) vectorAction[0]) {

            case 0:
                horizontal = 0f;
                vertical = 0f;
                break;
            case 1:
                horizontal = 1f;
                vertical = 0f;
                break;
            case 2:
                horizontal = 0f;
                vertical = -1f;
                break;
            case 3:
                horizontal = -1f;
                vertical = 0f;
                break;
            case 4:
                horizontal = 1f;
                vertical = 1f;
                break;
            case 5:
                horizontal = 1f;
                vertical = -1f;
                break;
            case 6:
                horizontal = -1f;
                vertical = -1f;
                break;
            case 7:
                horizontal = -1f;
                vertical = 1f;
                break;
            case 8:
                horizontal = 0f;
                vertical = 1f;
                break;
        }

        enemyMovement.AddMovement(horizontal, vertical);

        AddReward(-0.1f);
    }

    */

    // Caso vettore delle azioni DISCRETO 16 + 1
    public override void AgentAction(float[] vectorAction, string textAction) {

        float horizontal = 0f;
        float vertical = 0f;

        // Nessun movimento e movimento in ciascuna delle 8 direzioni cardinali + attacco

        switch ((int) vectorAction[0]) {

            case 0:
                horizontal = 0f;
                vertical = 0f;
                break;
            case 1:
                horizontal = 1f;
                vertical = 0f;
                break;
            case 2:
                horizontal = 0.924f;
                vertical = 0.382f;
                break;
            case 3:
                horizontal = 0.707f;
                vertical = 0.707f;
                break;
            case 4:
                horizontal = 0.382f;
                vertical = 0.924f;
                break;
            case 5:
                horizontal = 0f;
                vertical = 1f;
                break;
            case 6:
                horizontal = -0.382f;
                vertical = 0.924f;
                break;
            case 7:
                horizontal = -0.707f;
                vertical = 0.707f;
                break;
            case 8:
                horizontal = -0.924f;
                vertical = 0.382f;
                break;
            case 9:
                horizontal = -1f;
                vertical = 0f;
                break;
            case 10:
                horizontal = -0.924f;
                vertical = -0.382f;
                break;
            case 11:
                horizontal = -0.707f;
                vertical = -0.707f;
                break;
            case 12:
                horizontal = -0.382f;
                vertical = -0.924f;
                break;
            case 13:
                horizontal = 0f;
                vertical = -1f;
                break;
            case 14:
                horizontal = 0.382f;
                vertical = -0.924f;
                break;
            case 15:
                horizontal = 0.707f;
                vertical = -0.707f;
                break;
            case 16:
                horizontal = 0.924f;
                vertical = -0.382f;
                break;
        }

        enemyMovement.AddMovement(horizontal, vertical);

        AddReward(-0.1f);
    }


    public void TargetReached() {
        AddReward(50f);
        Done();
    }

    public void PlayerDown() {
        AddReward(50f);
        Done();
    }

}
