using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour {

    void Start() {
        Object.Destroy(this.gameObject, 0.7f);
    }
}
