using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RealTimeAcademy : Academy {

	public GameObject target;
    public List<GameObject> spawnPoints;

	private void Start() {
		
		target.transform.position = new Vector3(spawnPoints[0].transform.position.x, target.transform.position.y, spawnPoints[0].transform.position.z);
		
	}

  	public override void AcademyReset() {

		spawnPoints = Utility.Shuffle(spawnPoints);
		target.transform.position = new Vector3(spawnPoints[0].transform.position.x, target.transform.position.y, spawnPoints[0].transform.position.z);														
	}
	  
}
