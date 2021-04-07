using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour {

    public string type = "T";
    public GameObject text;

    public string GetPortalType() {
        return type;
    }

    public void SetOpenPortal() {
        text.gameObject.SetActive(true);
    }

}
