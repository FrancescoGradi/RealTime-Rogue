using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RealTimeAcademy : Academy {

	public GameObject target;
    public List<GameObject> spawnPoints;
	public GameObject agent;

	public float range = 10;
	private float epsilon = 1;

	private void Start() {
		
		// target.transform.position = new Vector3(spawnPoints[0].transform.position.x, target.transform.position.y, spawnPoints[0].transform.position.z);
		
		target.transform.position = GetRandomPosInRange(agent);

	}

  	public override void AcademyReset() {

		// spawnPoints = Utility.Shuffle(spawnPoints);
		// target.transform.position = new Vector3(spawnPoints[0].transform.position.x, target.transform.position.y, spawnPoints[0].transform.position.z);														
		target.transform.position = GetRandomPosInRange(agent);

	}

	public Vector3 GetRandomPosInRange(GameObject otherObject) {

		Vector3 pos = new Vector3(Random.Range(-range, range), otherObject.transform.position.y, Random.Range(-range, range));

		while (Vector3.Distance(pos, otherObject.gameObject.transform.position) < epsilon) {
			pos = new Vector3(Random.Range(-range, range), 0.1f, Random.Range(-range, range));
		}

		return pos;
	}
	  
}
