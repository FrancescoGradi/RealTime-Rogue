using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGenerator : MonoBehaviour {

    public GameObject[] rocks;

    void Start() {

        int rand = Random.Range(0, 2);
        Instantiate(rocks[rand], transform.position, Quaternion.identity);
    }

    private void OnDestroy() {
        
    }

}
