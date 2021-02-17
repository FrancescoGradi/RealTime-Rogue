using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Room : MonoBehaviour
{

    public GameObject enemy;
    public LayerMask playerLayer;

    public Item bastardSword;
    public Item goldenShield;
    public Item healthPotion;
    public Item bonusPotion;

    public GameObject portalT;
    public GameObject portalB;

    public GameObject portalL;
    public GameObject portalR;

    public GameObject[] rocks;


    private int n_enemies;
    private List<Item> items;
    private bool roomClear = false;
    private List<GameObject> activePortals = new List<GameObject> {};


    private void Start() {

        items = new List<Item> { bastardSword, goldenShield, healthPotion, bonusPotion };

        n_enemies = 2;

        Instantiate(enemy, new Vector3(5, 0, 8), Quaternion.Euler(0, 180, 0));
        Instantiate(enemy, new Vector3(-5, 0, 8), Quaternion.Euler(0, 180, 0));

        InstantiateItem(GetRandomItem(), -10, 1, -4);
        InstantiateItem(GetRandomItem(), 10, 1, -4);
    }

    private void Update() {
        
        if (roomClear && Input.GetKey(KeyCode.E)) {
            foreach (GameObject portal in activePortals) {
                Debug.Log("Portal pos" + portal.transform.position);

                Collider[] nearbyPlayers = Physics.OverlapSphere(portal.transform.position, 3f, playerLayer);

                foreach (Collider player in nearbyPlayers) {
                    Debug.Log("Player is  near");
                    FindObjectOfType<GameManager>().CreateNewRoom(portal.GetComponent<Portal>().GetPortalType());
                }
            }
        }
    }

    public void RoomCleared() {
        FindObjectOfType<RoomClearedScreen>().Clear();

        System.Random random = new System.Random();

        int n_portals = random.Next(1, 5);

        List<int> permutation = new List<int> {0, 0, 0, 0};

        for (int i = 0; i < n_portals; i++) {
            permutation[i] = 1;
        }

        permutation = Shuffle(permutation);
        
        if (permutation[0] == 1) {
            GameObject tmp = Instantiate(portalT, new Vector3(0, 0, 14.5f), Quaternion.identity);
            activePortals.Add(tmp);
        }

        if (permutation[1] == 1) {
            GameObject tmp = Instantiate(portalB, new Vector3(0, 0, -14.5f), Quaternion.Euler(0, -180, 0));
            activePortals.Add(tmp);
        }

        if (permutation[2] == 1) {
            GameObject tmp = Instantiate(portalR, new Vector3(14.5f, 0, 0), Quaternion.Euler(0, 90, 0));
            activePortals.Add(tmp);
        }

        if (permutation[3] == 1) {
            GameObject tmp = Instantiate(portalL, new Vector3(-14.5f, 0, 0), Quaternion.Euler(0, -90, 0));
            activePortals.Add(tmp);
        }        
    }


    public void EnemyDown() {

        n_enemies -= 1;
        if (n_enemies <= 0) {
            roomClear = true;
            RoomCleared();
        }
    }

    private GameObject InstantiateItem(Item item, float posX, float posY, float posZ) {

        return Instantiate(item.gameObject, new Vector3(posX, posY, posZ), item.gameObject.transform.rotation);
    }

    private Item GetRandomItem() {

        return items[(int)(UnityEngine.Random.Range(0, items.Capacity))];
    }


    private void OnDestroy() {

        foreach (GameObject portal in activePortals) {
            Destroy(portal);
        }
    }

    public List<T> Shuffle<T>(List<T> list)  {  

        System.Random rng = new System.Random();

        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }
        return list;
    }
}
