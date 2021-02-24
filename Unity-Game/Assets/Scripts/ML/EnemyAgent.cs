using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using MLAgents;

public class EnemyAgent : Agent
{
    public Enemy enemy;
    public float epsilon = 0.5f;

    void Start() {
        AgentReset();
    }

    public override void AgentReset() { }

    public override void CollectObservations() {
        
        List<float> obs = new List<float>();

        obs.Add(enemy.gameObject.transform.position.x);
        obs.Add(enemy.gameObject.transform.position.z);

        AddVectorObs(obs);
    }

    public override void AgentAction(float[] vectorAction, string textAction) {

        float horizontal = vectorAction[0];
        float vertical = vectorAction[1];

        AddReward(-0.01f);

        if (System.Math.Abs(enemy.gameObject.transform.position.z - 9) < epsilon) {
            AddReward(100f);
            Done();
        }

    }

}
