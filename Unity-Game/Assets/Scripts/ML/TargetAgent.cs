using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using MLAgents;

public class TargetAgent : Agent {

    /*
    public GameObject enemy;
    public RealTimeAcademy realTimeAcademy;
    public LocalCellView localCellView;
    
    public List<float> angles = new List<float>() {0f};
    public float raycastMaxDistance = 8f;
    public float rayMinDistance = 1f;
    public float healthPotionReward = 16f;

    private Target target;
    private TargetCombat targetCombat;

    void Start() {
        target = GetComponent<Target>();
        targetCombat = GetComponent<TargetCombat>();
        // AgentReset();
    }

    public override void AgentReset() {

        
        target.gameObject.transform.SetPositionAndRotation(realTimeAcademy.GetRandomPosInRange(enemy, 0), Quaternion.identity);

        target.updateRate = (int) realTimeAcademy.resetParameters["agent_update_rate"];
        target.epsilon = realTimeAcademy.resetParameters["attack_range_epsilon"];
        healthPotionReward = realTimeAcademy.resetParameters["health_potion_reward"];

        target.ResetStatsAndItems();

        target.AddMovement(0, 0);
    }

    public override void CollectObservations() {
        
        List<float> obs = new List<float>();

        // Posizioni normalizzate secondo la grandezza massima della mappa

        obs.Add(target.gameObject.transform.position.x / 15f);
        obs.Add(target.gameObject.transform.position.z / 15f);

        // Valore compreso tra [-1, 1] che indica la direzione verso cui l'agente è rivolto

        obs.Add((Vector3.SignedAngle(target.gameObject.transform.forward, new Vector3(0, 0, 1), Vector3.up)) / 180f);

        obs.Add(enemy.transform.position.x / 15f);
        obs.Add(enemy.transform.position.z / 15f);

        // Local Cell View: modo per ottenere delle osservazioni locali sullo stato

        localCellView.SetPosition(target.gameObject.transform.position.x, target.gameObject.transform.position.z);

        List<float> cells = localCellView.GetLocalCellView();

        foreach (float cellValue in cells) {
            obs.Add(cellValue);
        }

        // Reward negativa con gli ostacoli

        if (cells[12] == 1f)
            AddReward(-1f); 

        /*

        // Secondo modo: raggi di lunghezza massima che intersecano oggetti env-item e restituiscono la distanza
        
        for (int i = 0; i < angles.Count; i++) {
            List<float> raycastVector = target.GetRayCastDistance(raycastMaxDistance, angles[i]);

            // Scoraggiamo il movimento verso un ostacolo
            if (raycastVector[2] == 1 && raycastVector[0] < (rayMinDistance / raycastMaxDistance)) {
                AddReward(-1f);
            }

            obs.Add(raycastVector[0]);
            obs.Add(raycastVector[1]);
            obs.Add(raycastVector[2]);
            obs.Add(raycastVector[3]);
            obs.Add(raycastVector[4]);
        }

        // Booleano: se il target si trova nel range dell'agente, allora restituisce 1. Serve per aiutare l'agente
        // ad attaccare

        obs.Add(target.IsInRange());

        // 1 se l'agente ha una pozione o 0 se non ce l'ha
        if (target.HasActualPotion()) {
            obs.Add(1);
        } else {
            obs.Add(0);
        }

        // STATS

        obs.Add(target.currentHealth / target.HP);
        
        AddVectorObs(obs);
    }

    // Caso vettore delle azioni CONTINUO
    public override void AgentAction(float[] vectorAction, string textAction) {

        float horizontal = vectorAction[0];
        float vertical = vectorAction[1];
        float attack = vectorAction[2];
        float drink = vectorAction[3];

        // Debug.Log(horizontal + "   " + vertical + "   " + attack + "   " + drink);
        // Debug.Log("Drink --> " + drink);

        if (attack > 0) {
            targetCombat.NormalAttack();
            target.AddMovement(0, 0);
        } else {
            target.AddMovement(horizontal, vertical);
        }

        if (drink > 0) {
            target.DrinkPotion();
        }

        AddReward(-0.1f);
    }

    public void HealthPotionCollectedReward() {
        AddReward(healthPotionReward);
    }

    public void EnemyDown() {
        AddReward(50f);
        Done();
    }

    */

}

