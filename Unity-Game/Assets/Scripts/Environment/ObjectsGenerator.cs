using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsGenerator : MonoBehaviour {
    public List<GameObject> initialItems;
    public List<float> itemsWeights;
    public List<GameObject> envObjects;
    public List<GameObject> spawnPoints;

    public int max_items = 4;
    public int max_objects = 4;

    private List<GameObject> items;
    private List<GameObject> activeItems = new List<GameObject> {};
    private List<GameObject> activeEnvObjects = new List<GameObject> {};

    private void Start() {
        
        // int actual_n_items = UnityEngine.Random.Range(0, max_items);
        // int actual_n_envObjects = UnityEngine.Random.Range(0, max_objects);
        int actual_n_items = max_items;
        int actual_n_envObjects = max_objects;

        items = ItemsVectorGeneratorFromWeights(initialItems, itemsWeights);
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
                if ((int) UnityEngine.Random.Range(0, 3) == 0){
                    selectedEnvObject.gameObject.transform.Rotate(new Vector3(0, -90f, 0));
                }
                activeEnvObjects.Add(Instantiate(selectedEnvObject, spawnPoints[j].gameObject.transform.position, selectedEnvObject.gameObject.transform.rotation));
            }
        }
    }

    // La soluzione per avere una collezione di elementi random pesati è avere un vettore più grande che rispetti questa proporzione
    // e poi scegliere un elemento casuale da questo (il vettore non cambia, per questo è efficiente per il training): soluzione ruota della fortuna

    private List<GameObject> ItemsVectorGeneratorFromWeights(List<GameObject> itms, List<float> weights) {

        if (itms.Count != weights.Count)
            throw new System.Exception("Initial items and weights have different dimension!");

        List<GameObject> full_items = new List<GameObject> {};

        for (int i = 0; i < weights.Count; i++) {
            float weight = weights[i];
            while (weight >= 0.045f) {
                full_items.Add(itms[i]);
                weight -= 0.05f;
            }
        }

        return full_items;
    }

    public List<GameObject> GetActiveItems() {
        
        List<GameObject> actualItems = new List<GameObject> {};

        foreach (GameObject item in activeItems) {
            if (item != null && item.activeSelf) {
                actualItems.Add(item);
            } 
        }

        return actualItems;
    }

    public List<GameObject> GetActiveEnvObjects() {

        return activeEnvObjects;
    }

    public void SetMaxItems(int max_items) {
        if (max_items + max_objects > spawnPoints.Count) {
            throw new System.Exception("Too many instantiated items and env objects!");
        } else {
            this.max_items = max_items;
        }
    }

    public void ResetPositions() {

        // Devo distruggerli ogni volta perche' l'agente potrebbe aver preso (e quindi distrutto) qualche oggetto

        foreach (GameObject activeItem in activeItems) {
            Destroy(activeItem);
        }

        activeItems = new List<GameObject> {};

        spawnPoints = Utility.Shuffle(spawnPoints);

        items = ItemsVectorGeneratorFromWeights(initialItems, itemsWeights);

        Vector3 pos = new Vector3(0, 0, 0);

        // Implementazione con spawn points

        for (int i = 0; i < max_items; i++) {
            pos = spawnPoints[i].gameObject.transform.position;
            pos.y += 1;
            GameObject selectedItem = Utility.GetRandomObject(items);
            activeItems.Add(Instantiate(selectedItem, pos, selectedItem.gameObject.transform.rotation));
        }

        /*

        // Implementazione con spawn totalmente casuali senza spawn points

        float range = 14.5f;

        for (int i = 0; i < max_items; i++) {
            pos = new Vector3(Random.Range(-range, range), 1f, Random.Range(-range, range));
            GameObject selectedItem = Utility.GetRandomObject(items);
            activeItems.Add(Instantiate(selectedItem, pos, selectedItem.gameObject.transform.rotation));
        }

        */

        spawnPoints.Reverse();

        for (int i = 0; i < activeEnvObjects.Count; i++) {
            pos = spawnPoints[i].gameObject.transform.position;
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
