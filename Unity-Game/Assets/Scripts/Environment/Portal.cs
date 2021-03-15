using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Portal : MonoBehaviour {

    public string type = "T";

    public TMP_Text healthText;
    public GameObject text;

    public string GetPortalType() {
        return type;
    }

    public void SetOpenPortal() {
        text.gameObject.SetActive(true);
        healthText.text = "Press RB to Enter"; 
    }

}
