using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Room : MonoBehaviour
{

    public LayerMask playerLayer;

    public GameObject portalT;
    public GameObject portalB;

    public GameObject portalL;
    public GameObject portalR;

    public GameObject patchedL;
    public GameObject patchedT;


    private bool roomClear = false;
    private List<GameObject> activePortals = new List<GameObject> {};
    private List<GameObject> patchedInstantieted = new List<GameObject> {};


    private void Start() {

        System.Random random = new System.Random();

        int n_portals = random.Next(1, 5);

        List<int> permutation = new List<int> {0, 0, 0, 0};

        for (int i = 0; i < n_portals; i++) {
            permutation[i] = 1;
        }

        permutation = Utility.Shuffle(permutation);
        
        PortalInstantiation(4.5f, permutation);
    }

    private void Update() {
        
        if (roomClear && (Input.GetButton("BaseAction"))) {
            foreach (GameObject portal in activePortals) {

                Collider[] nearbyPlayers = Physics.OverlapSphere(portal.transform.position, 3f, playerLayer);

                foreach (Collider player in nearbyPlayers) {
                    FindObjectOfType<GameManager>().CreateNewRoom(portal.GetComponent<Portal>().GetPortalType());
                }
            }
        }
    }

    public void RoomClear() {

        roomClear = true;
        FindObjectOfType<RoomClearedScreen>().Clear();

        foreach (GameObject portal in activePortals) {
            portal.GetComponent<Portal>().SetOpenPortal();
        }

    }

    private void OnDestroy() {

        foreach (GameObject portal in activePortals) {
            Destroy(portal);
        }

        foreach (GameObject patch in patchedInstantieted) {
            Destroy(patch);
        }
    }

    private void PortalInstantiation(float seconds, List<int> permutation) {
        
        if (permutation[0] == 1) {
            GameObject tmp = Instantiate(portalT, new Vector3(0, 0, 14.5f), Quaternion.identity);
            activePortals.Add(tmp);
        } else {
            GameObject tmp = Instantiate(patchedT, patchedT.gameObject.transform.position, patchedT.gameObject.transform.rotation);
            patchedInstantieted.Add(tmp);
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
        } else {
            GameObject tmp = Instantiate(patchedL, patchedL.gameObject.transform.position, patchedL.gameObject.transform.rotation);
            patchedInstantieted.Add(tmp);
        }
    }
}
