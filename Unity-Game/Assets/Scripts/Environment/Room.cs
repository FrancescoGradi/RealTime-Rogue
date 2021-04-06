using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public LayerMask playerLayer;

    public GameObject portalT;
    public GameObject portalB;
    public GameObject portalL;
    public GameObject portalR;

    public GameObject patchedT;
    public GameObject patchedB;
    public GameObject patchedL;
    public GameObject patchedR;



    private bool roomClear = false;
    private bool roomChanged = false;
    private int initialPortalNum;
    private List<GameObject> activePortals = new List<GameObject> {};
    private List<GameObject> patchedInstantieted = new List<GameObject> {};


    private void Start() {

        System.Random random = new System.Random();

        int n_portals = random.Next(2, 5);

        List<int> permutation = new List<int> {0, 0, 0, 0};

        for (int i = 0; i < n_portals; i++) {
            permutation[i] = 1;
        }

        permutation = Utility.Shuffle(permutation);

        try {
            initialPortalNum = FindObjectOfType<GameManager>().GetInitialPortalNum();
        } catch (Exception e) {
            Debug.Log(e);
            initialPortalNum = 0;
        }
        permutation[initialPortalNum] = 1;
        
        PortalInstantiation(permutation);
    }

    private void Update() {
        
        if (roomClear && (Input.GetButton("BaseAction")) && !roomChanged) {
            foreach (GameObject portal in activePortals) {

                if (initialPortalNum == 0 && portal.GetComponent<Portal>().GetPortalType() == "T") {
                continue;
                } else if (initialPortalNum == 1 && portal.GetComponent<Portal>().GetPortalType() == "B") {
                    continue;
                } else if (initialPortalNum == 2 && portal.GetComponent<Portal>().GetPortalType() == "L") {
                    continue;
                } else if (initialPortalNum == 3 && portal.GetComponent<Portal>().GetPortalType() == "R" ) {
                    continue;
                } else {
                    Collider[] nearbyPlayers = Physics.OverlapSphere(portal.transform.position, 3f, playerLayer);

                    foreach (Collider player in nearbyPlayers) {
                        roomChanged = true;
                        FindObjectOfType<GameManager>().CreateNewRoom(portal.GetComponent<Portal>().GetPortalType());
                        break;
                    }
                }
            }
        }
    }

    public void RoomClear() {

        roomClear = true;
        FindObjectOfType<RoomClearedScreen>().Clear();

        foreach (GameObject portal in activePortals) {

            if (initialPortalNum == 0 && portal.GetComponent<Portal>().GetPortalType() == "T") {
                continue;
            } else if (initialPortalNum == 1 && portal.GetComponent<Portal>().GetPortalType() == "B") {
                continue;
            } else if (initialPortalNum == 2 && portal.GetComponent<Portal>().GetPortalType() == "L") {
                continue;
            } else if (initialPortalNum == 3 && portal.GetComponent<Portal>().GetPortalType() == "R" ) {
                continue;
            } else {
                portal.GetComponent<Portal>().SetOpenPortal();
            }
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

    private void PortalInstantiation(List<int> permutation) {
        
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
        } else {
            GameObject tmp = Instantiate(patchedB, patchedB.gameObject.transform.position, patchedB.gameObject.transform.rotation);
            patchedInstantieted.Add(tmp);
        }

        if (permutation[2] == 1) {
            GameObject tmp = Instantiate(portalR, new Vector3(14.5f, 0, 0), Quaternion.Euler(0, 90, 0));
            activePortals.Add(tmp);
        } else {
            GameObject tmp = Instantiate(patchedR, patchedR.gameObject.transform.position, patchedR.gameObject.transform.rotation);
            patchedInstantieted.Add(tmp);
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
