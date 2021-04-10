using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTable : MonoBehaviour {

    public GameObject magicTableText;
    public LayerMask playerLayer;
    public GameObject statModMenu;
    public int statPoints;

    private float magicTableRange = 2.5f;
    private bool inMenu = false;

    private void Start() {
        magicTableText.SetActive(false);
    }

    void Update() {

        if (!inMenu && Vector3.Distance(Player.instance.transform.position, this.gameObject.transform.position) < magicTableRange) {
            magicTableText.SetActive(true);
            if (Input.GetButton("BaseAction")) {

                magicTableText.SetActive(false);
                Instantiate(statModMenu, FindObjectOfType<Canvas>().transform);
                inMenu = true;
            }
        } else {
            magicTableText.SetActive(false);
        }
        
    }
}
