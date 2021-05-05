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
		
		range = this.resetParameters["range"];
		target.GetComponent<Enemy>().speed = this.resetParameters["speed"];
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

}
