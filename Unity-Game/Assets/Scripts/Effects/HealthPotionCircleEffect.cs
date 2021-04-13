using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionCircleEffect : MonoBehaviour {
    void Start() {
        Object.Destroy(this.gameObject, 1.5f);
    }
}
