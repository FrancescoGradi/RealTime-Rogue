using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RealTimeAcademy : Academy {

	public GameObject target;
	public GameObject agent;
	public ObjectsGenerator objectsGenerator;

	public float range = 10;
	private float epsilon = 1;

	private void Start() {
				
		AcademyReset();
	}

  	public override void AcademyReset() {
		
		objectsGenerator.ResetPositions();
		target.transform.position = GetRandomPosInRange(agent, 0.1f);

		range = this.resetParameters["range"];
		Debug.Log(range);
		// target.transform.position = new Vector3(0, 0.1f, 0);
	}

	public Vector3 GetRandomPosInRange(GameObject otherObject, float posY) {

		Vector3 pos = new Vector3(Random.Range(-range, range), posY, Random.Range(-range, range));

		while (Vector3.Distance(pos, otherObject.gameObject.transform.position) < epsilon) {
			pos = new Vector3(Random.Range(-range, range), posY, Random.Range(-range, range));
		}

		return pos;
	}
}
