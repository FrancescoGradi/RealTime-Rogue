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
    public List<float> angles = new List<float>() {0f};
    public float raycastMaxDistance = 10f;
    public float rayMinDistance = 1f;

    void Start() {
        enemyMovement = GetComponent<EnemyMovement>();
        // AgentReset();
    }

    public override void AgentReset() {

        if ((int) realTimeAcademy.resetParameters["agent_fixed"] == 1){
            enemy.gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        } else {
            enemy.gameObject.transform.SetPositionAndRotation(realTimeAcademy.GetRandomPosInRange(target, 0), Quaternion.identity);
        }

        enemyMovement.updateRate = (int) realTimeAcademy.resetParameters["agent_update_rate"];

        enemyMovement.SetTargetReached(false);
        enemyMovement.AddMovement(0, 0);
    }

    public override void CollectObservations() {
        
        List<float> obs = new List<float>();

        obs.Add(enemy.gameObject.transform.position.x);
        obs.Add(enemy.gameObject.transform.position.z);
        
        obs.Add(target.transform.position.x);
        obs.Add(target.transform.position.z);

        /*
        // Primo modo: restituisce tutte le posizioni globali degli ostacoli presenti sulla mappa
        List<GameObject> envObjects = objectsGenerator.GetActiveEnvObjects();

        for (int i = 0; i < objectsGenerator.max_objects; i++) {
            obs.Add(envObjects[i].transform.position.x);
            obs.Add(envObjects[i].transform.position.z);
        }
        */

        // Secondo modo: raggi di lunghezza massima che intersecano oggetti env e restituiscono la distanza
        for (int i = 0; i < angles.Count; i++) {
            float raycastDistance = enemyMovement.GetRayCastDistance(raycastMaxDistance, angles[i]);

            // Scoraggiamo il movimento verso un ostacolo
            if (raycastDistance < rayMinDistance) {
                AddReward(-1f);
            }

            obs.Add(raycastDistance);
        }

        AddVectorObs(obs);
    }

    /*

    // Caso vettore delle azioni CONTINUO

    public override void AgentAction(float[] vectorAction, string textAction) {

        float horizontal = vectorAction[0];
        float vertical = vectorAction[1];

        enemyMovement.AddMovement(horizontal, vertical);

        AddReward(-0.1f);
    }

    */

    // Caso vettore delle azioni DISCRETO

    public override void AgentAction(float[] vectorAction, string textAction) {

        float horizontal = 0f;
        float vertical = 0f;

        // Nessun movimento e movimento in ciascuna delle 8 direzioni cardinali

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

    public void TargetReached() {
        AddReward(50f);
        Done();
    }

}
