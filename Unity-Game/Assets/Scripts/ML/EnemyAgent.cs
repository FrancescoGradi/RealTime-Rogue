using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using MLAgents;

public class EnemyAgent : Agent
{
    public Enemy enemy;
    public GameObject target;
    public RealTimeAcademy realTimeAcademy;
    public ObjectsGenerator objectsGenerator;

    private EnemyMovement enemyMovement;

    void Start() {
        enemyMovement = GetComponent<EnemyMovement>();
        // AgentReset();
    }

    public override void AgentReset() {
        // enemy.gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        enemy.gameObject.transform.SetPositionAndRotation(realTimeAcademy.GetRandomPosInRange(target, 0), Quaternion.identity);
        enemyMovement.SetTargetReached(false);
        enemyMovement.AddMovement(0, 0);
    }

    public override void CollectObservations() {
        
        List<float> obs = new List<float>();

        obs.Add(enemy.gameObject.transform.position.x);
        obs.Add(enemy.gameObject.transform.position.z);
        
        obs.Add(target.transform.position.x);
        obs.Add(target.transform.position.z);

        List<GameObject> envObjects = objectsGenerator.GetActiveEnvObjects();

        for (int i = 0; i < objectsGenerator.max_objects; i++) {
            obs.Add(envObjects[i].transform.position.x);
            obs.Add(envObjects[i].transform.position.z);
        }

        AddVectorObs(obs);
    }

    public override void AgentAction(float[] vectorAction, string textAction) {

        float horizontal = vectorAction[0];
        float vertical = vectorAction[1];

        enemyMovement.AddMovement(horizontal, vertical);

        AddReward(-0.1f);
    }

    public void TargetReached() {
        AddReward(50f);
        Done();
    }

}
