﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsGenerator : MonoBehaviour
{
    public List<GameObject> items;
    public List<GameObject> envObjects;
    public List<GameObject> spawnPoints;

    public int max_items = 4;
    public int max_objects = 8;

    private List<GameObject> activeItems = new List<GameObject> {};
    private List<GameObject> activeEnvObjects = new List<GameObject> {};

    private void Start() {
        
        int actual_n_items = UnityEngine.Random.Range(0, max_items);
        // int actual_n_envObjects = UnityEngine.Random.Range(0, max_objects);
        int actual_n_envObjects = max_objects;

        items = Utility.Shuffle(items);
        envObjects = Utility.Shuffle(envObjects);
        spawnPoints = Utility.Shuffle(spawnPoints);

        
        if (actual_n_items + actual_n_envObjects > spawnPoints.Count) {
            throw new System.Exception("Too many instantiated items and env objects!");
        } else {
            for (int i = 0; i < actual_n_items; i++) {
                Vector3 pos = spawnPoints[i].gameObject.transform.position;
                pos.y += 1;
                GameObject selectedItem = Utility.GetRandomObject(items);
                activeItems.Add(Instantiate(selectedItem, pos, selectedItem.gameObject.transform.rotation));
            }

            spawnPoints.Reverse();
            
            for (int j = 0; j < actual_n_envObjects; j++) {
                GameObject selectedEnvObject = Utility.GetRandomObject(envObjects);
                activeEnvObjects.Add(Instantiate(selectedEnvObject, spawnPoints[j].gameObject.transform.position, selectedEnvObject.gameObject.transform.rotation));
            }
        }
    }

    public List<GameObject> GetActiveEnvObjects() {

        return activeEnvObjects;
    }

    public void ResetPositions() {

        spawnPoints = Utility.Shuffle(spawnPoints);
        for (int i = 0; i < activeEnvObjects.Count; i++) {
            Vector3 pos = spawnPoints[i].gameObject.transform.position;
            pos.y += 1;
            activeEnvObjects[i].transform.position = pos;
        }
    }

    private void OnDestroy() {

        foreach (GameObject activeItem in activeItems) {
            Destroy(activeItem);
        }
        foreach (GameObject activeEnvObject in activeEnvObjects) {
            Destroy(activeEnvObject);
        }
    }
}
