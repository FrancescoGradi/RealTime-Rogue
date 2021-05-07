using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RealTimeAcademy : Academy {

	public GameObject target;
	public GameObject agent;
	public ObjectsGenerator objectsGenerator;

	public float range = 10;
	private float epsilon = 4f;

	private void Start() {
				
		AcademyReset();
	}

  	public override void AcademyReset() {
		
		objectsGenerator.ResetPositions();
		
		range = this.resetParameters["spawn_range"];

		AgentReset();
		TargetReset();
	}

	public Vector3 GetRandomPosInRange(GameObject otherObject, float posY) {

		Vector3 pos = new Vector3(Random.Range(-range, range), posY, Random.Range(-range, range));

		while (Vector3.Distance(pos, otherObject.gameObject.transform.position) < epsilon) {
			pos = new Vector3(Random.Range(-range, range), posY, Random.Range(-range, range));
		}

		return pos;
	}

	public bool IsTargetDown() {

		if (target.GetComponent<Enemy>().currentHealth <= 0) {
			return true;
		} else {
			return false;
		}
	}

	private void AgentReset() {

		agent.transform.SetPositionAndRotation(this.GetRandomPosInRange(target, 0), Quaternion.identity);

		agent.GetComponent<EnemyMovement>().updateRate = (int) this.resetParameters["agent_update_rate"];
        agent.GetComponent<EnemyMovement>().epsilon = this.resetParameters["attack_range_epsilon"];

        agent.GetComponent<Enemy>().ResetStatsAndItems(true, 0, 6f);

        agent.GetComponent<EnemyMovement>().AddMovement(0, 0);
    }

	private void TargetReset() {

		target.transform.SetPositionAndRotation(this.GetRandomPosInRange(agent, 0), Quaternion.identity);

		target.GetComponent<EnemyMovement>().updateRate = (int) this.resetParameters["target_update_rate"];
        target.GetComponent<EnemyMovement>().epsilon = this.resetParameters["attack_range_epsilon"];

        target.GetComponent<Enemy>().ResetStatsAndItems(false, (int) this.resetParameters["target_HP"], this.resetParameters["target_speed"]);

        target.GetComponent<EnemyMovement>().AddMovement(0, 0);

	}

}
