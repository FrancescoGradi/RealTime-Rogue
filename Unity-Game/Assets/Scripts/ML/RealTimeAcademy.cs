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
		
		List<float> itemsWeights = new List<float> {};
		itemsWeights.Add(this.resetParameters["health_potion_frequency"]);
		itemsWeights.Add(this.resetParameters["shield_frequency"]);
		itemsWeights.Add(this.resetParameters["sword_frequency"]);		

		objectsGenerator.SetMaxItems((int) this.resetParameters["max_items"]);
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

	public void EndEpisode() {

		agent.GetComponent<Agent>().Done();
		target.GetComponent<Agent>().Done();
	}

	private void AgentReset() {

		agent.transform.SetPositionAndRotation(this.GetRandomPosInRange(target, 0), Quaternion.identity);

		agent.GetComponent<EnemyMovement>().updateRate = (int) this.resetParameters["agent_update_rate"];
        agent.GetComponent<EnemyMovement>().epsilon = this.resetParameters["attack_range_epsilon"];

		int hp = Utility.GetRandomInt((int) this.resetParameters["min_agent_HP"], (int) this.resetParameters["max_agent_HP"]);

        agent.GetComponent<Enemy>().ResetStatsAndItems(false, hp, 6f);

        agent.GetComponent<EnemyMovement>().AddMovement(0, 0);
    }

	private void TargetReset() {

		target.transform.SetPositionAndRotation(this.GetRandomPosInRange(agent, 0), Quaternion.identity);

		target.GetComponent<EnemyMovement>().updateRate = (int) this.resetParameters["target_update_rate"];
        target.GetComponent<EnemyMovement>().epsilon = this.resetParameters["attack_range_epsilon"];

		int hp = Utility.GetRandomInt((int) this.resetParameters["min_target_HP"], (int) this.resetParameters["max_target_HP"]);

        target.GetComponent<Enemy>().ResetStatsAndItems(false, hp, this.resetParameters["target_speed"]);

        target.GetComponent<EnemyMovement>().AddMovement(0, 0);
	}

}
