using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGenerator : MonoBehaviour {

    public List<GameObject> rocks;
    public List<GameObject> spawnPoints;

    public int max_rocks = 6;

    private List<GameObject> activeRocks = new List<GameObject> {};
 
    void Start() {

        int actual_n_rocks = Random.Range(0, max_rocks);

        rocks = Utility.Shuffle(rocks);
        spawnPoints = Utility.Shuffle(spawnPoints);

        for (int i = 0; i < actual_n_rocks; i++) {
            activeRocks.Add(Instantiate(Utility.GetRandomObject(rocks), spawnPoints[i].gameObject.transform.position, Quaternion.identity));
        }
    }

    private void OnDestroy() {
        foreach (GameObject activeRock in activeRocks) {
            Destroy(activeRock);
        }
    }
}