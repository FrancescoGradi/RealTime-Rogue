using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RealTimeAcademy : Academy {

	public Target target;
	public GameObject agent;
	public ObjectsGenerator objectsGenerator;

	public float range = 10;
	private float epsilon = 4f;

	private void Start() {
				
		AcademyReset();
	}

  	public override void AcademyReset() {
		
		objectsGenerator.ResetPositions();
		
		range = this.resetParameters["range"];
		target.updateMovement = (int) this.resetParameters["update_movement"];
		target.speed = this.resetParameters["speed"];

		if ((int) this.resetParameters["target_fixed"] == 1) {
			target.ResetPosition(new Vector3(0, 0.1f, 0));
		} else {
			target.ResetPosition(GetRandomPosInRange(agent, 0.1f));
		}
	}

	public Vector3 GetRandomPosInRange(GameObject otherObject, float posY) {

		Vector3 pos = new Vector3(Random.Range(-range, range), posY, Random.Range(-range, range));

		while (Vector3.Distance(pos, otherObject.gameObject.transform.position) < epsilon) {
			pos = new Vector3(Random.Range(-range, range), posY, Random.Range(-range, range));
		}

		return pos;
	}
}
