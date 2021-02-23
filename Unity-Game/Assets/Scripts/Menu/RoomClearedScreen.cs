using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomClearedScreen : MonoBehaviour
{
    public GameObject roomClearedUI;

    public void Clear() {

        roomClearedUI.SetActive(true);
        StartCoroutine(Waiter());
    }

    private IEnumerator Waiter() {
        
        yield return new WaitForSecondsRealtime(4);
        roomClearedUI.SetActive(false);
    }
}
