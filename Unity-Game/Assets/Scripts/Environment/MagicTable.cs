using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTable : MonoBehaviour {

    public GameObject magicTableText;
    public LayerMask playerLayer;
    public int statPoints;

    private float magicTableRange = 2.5f;

    private void Start() {
        magicTableText.SetActive(false);
    }

    void Update() {

        if (Vector3.Distance(Player.instance.transform.position, this.gameObject.transform.position) < magicTableRange) {
            magicTableText.SetActive(true);
        } else {
            magicTableText.SetActive(false);
        }
        
    }
}
