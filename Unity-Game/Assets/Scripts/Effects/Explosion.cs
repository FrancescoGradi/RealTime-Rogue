using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    void Start() {
        Object.Destroy(this.gameObject, 1f);
    }
}
