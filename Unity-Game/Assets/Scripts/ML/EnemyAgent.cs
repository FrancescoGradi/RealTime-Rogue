using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using MLAgents;

public class EnemyAgent : Agent
{
    public Enemy enemy;
    private EnemyMovement enemyMovement;

    void Start() {
        AgentReset();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public override void AgentReset() {
        enemy.gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
    }

    public override void CollectObservations() {
        
        List<float> obs = new List<float>();

        obs.Add(enemy.gameObject.transform.position.x);
        obs.Add(enemy.gameObject.transform.position.z);

        // Bounds del pavimento
        if (obs[0] < -5f || obs[0] > 5f || obs[1] < -5 || obs[1] > 25)
            Done();

        AddVectorObs(obs);
    }

    public override void AgentAction(float[] vectorAction, string textAction) {

        float horizontal = vectorAction[0];
        float vertical = vectorAction[1];

        enemyMovement.AddMovement(horizontal, vertical);

        AddReward(-0.01f);
    }

    public void TargetReached() {
        AddReward(50f);
        Done();
    }

}
