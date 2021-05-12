using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using MLAgents;

public class EnemyAgent : Agent {
    public GameObject target;
    public RealTimeAcademy realTimeAcademy;
    public LocalCellView localCellView;
    public LocalCellView globalCellView;
    
    public List<float> angles = new List<float>() {0f};
    public float raycastMaxDistance = 8f;
    public float rayMinDistance = 1f;
    public float healthPotionReward = 16f;

    private Enemy enemy;
    private EnemyMovement enemyMovement;
    private EnemyCombat enemyCombat;

    void Start() {
        enemy = GetComponent<Enemy>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyCombat = GetComponent<EnemyCombat>();
    }

    public override void AgentReset() { }

    public override void CollectObservations() {
        
        List<float> obs = new List<float>();

        // Posizioni normalizzate secondo la grandezza massima della mappa

        obs.Add(enemy.gameObject.transform.position.x / 15f);
        obs.Add(enemy.gameObject.transform.position.z / 15f);

        // Valore compreso tra [-1, 1] che indica la direzione verso cui l'agente è rivolto

        obs.Add((Vector3.SignedAngle(enemy.gameObject.transform.forward, new Vector3(0, 0, 1), Vector3.up)) / 180f);

        obs.Add(target.transform.position.x / 15f);
        obs.Add(target.transform.position.z / 15f);

        // Differenze tra le posizioni tra target e agent

        obs.Add((target.transform.position.x / 15f) - (enemy.gameObject.transform.position.x / 15f));
        obs.Add((target.transform.position.z / 15f) - (enemy.gameObject.transform.position.z / 15f));

        /*

        // Global Cell View: osservazioni globali sullo stato -> mappa globale di dimensione 19x19

        List<float> globalCells = globalCellView.GetLocalCellView();

        foreach (float cellValue in globalCells) {
            obs.Add(cellValue);
        }

        */

        // Local Cell View: modo per ottenere delle osservazioni locali sullo stato

        localCellView.SetPosition(enemy.gameObject.transform.position.x, enemy.gameObject.transform.position.z);

        List<float> cells = localCellView.GetLocalCellView();

        foreach (float cellValue in cells) {
            obs.Add(cellValue);
        }

        // Reward negativa con gli ostacoli 12->5x5, 17, 24-> 7x7

        if (cells[17] == 1f)
            AddReward(-1f);
        
        if (cells[24] == 1f)
            AddReward(-1f);

        /*
        // Secondo modo: raggi di lunghezza massima che intersecano oggetti env-item e restituiscono la distanza
        
        for (int i = 0; i < angles.Count; i++) {
            List<float> raycastVector = enemyMovement.GetRayCastDistance(raycastMaxDistance, angles[i]);

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
        */

        // Booleano: se il target si trova nel range dell'agente, allora restituisce 1. Serve per aiutare l'agente
        // ad attaccare

        obs.Add(enemyMovement.IsInRange());

        // 1 se l'agente ha una pozione o 0 se non ce l'ha
        if (enemy.HasActualPotion()) {
            obs.Add(1);
        } else {
            obs.Add(0);
        }

        // STATS

        obs.Add((float) enemy.currentHealth / (float) enemy.HP);
        obs.Add((float) target.GetComponent<Enemy>().currentHealth / (float) target.GetComponent<Enemy>().HP);
        
        AddVectorObs(obs);
    }

    // Caso vettore delle azioni CONTINUO
    public override void AgentAction(float[] vectorAction, string textAction) {

        float horizontal = vectorAction[0];
        float vertical = vectorAction[1];
        float attack = vectorAction[2];
        float drink = vectorAction[3];

        /*

        if (enemyMovement.playerLayer == 11) {
            Debug.Log("Agent action " + horizontal + "   " + vertical + "   " + attack + "   " + drink);
        } else if (enemyMovement.playerLayer == 9) {
            Debug.Log("Target action " + horizontal + "   " + vertical + "   " + attack + "   " + drink);
        }

        */
        
        if (attack > 0) {
            enemyCombat.NormalAttack();
            enemyMovement.AddMovement(0, 0);
        } else {
            enemyMovement.AddMovement(horizontal, vertical);
        }

        if (drink > 0) {
            enemy.DrinkPotion();
        }

        AddReward(-0.1f);
    }

    public void PlayerDown() {
        // Bisogna stabilire chi è morto, tra player e target, passando dalla realtime Academy (che sa chi è chi)
        if (realTimeAcademy.IsTargetDown()) {
            // La reward finale dipende anche dagli HP rimasti dell'agente
            if (enemy.currentHealth > 0) {
                AddReward(5f * (float) enemy.currentHealth);
                // Debug.Log("Reward " + (5f * (float) enemy.currentHealth));
            } else {
                AddReward(5f);
            }
        }

        Done();
    }

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
            case 17:
                horizontal = 0f;
                vertical = 0f;
                enemyCombat.NormalAttack();
                break;
        }

        enemyMovement.AddMovement(horizontal, vertical);

        AddReward(-0.1f);
    }

    */

}
