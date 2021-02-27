using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public string type = "T";

    private Vector3 currentPosition;
    private Vector3 endPosition;
    private int speed = 50;
    private int high = -40;
    private bool started = false;

    private void Start() {

        endPosition = this.gameObject.transform.position;
        this.gameObject.transform.position = this.gameObject.transform.position + new Vector3(0, high, 0);

        StartCoroutine(PortalAnimationWaiter(4.5f));
    }

    private void Update() {

        if (this.gameObject.transform.position != endPosition && started) {
            
            currentPosition = this.gameObject.transform.position;
            this.gameObject.transform.position = Vector3.MoveTowards(currentPosition, endPosition, speed * Time.deltaTime);
        }
    }

    public string GetPortalType() {
        return type;
    }

    private IEnumerator PortalAnimationWaiter(float seconds) {
        
        yield return new WaitForSecondsRealtime(seconds);
        started = true;
    }

}
