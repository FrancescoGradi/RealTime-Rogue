using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {
    void Start() {
        Object.Destroy(this.gameObject, 5f);
    }
}
