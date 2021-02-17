using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public GameObject winScreenUI;

    public void YouWin() {

        winScreenUI.SetActive(true);
        StartCoroutine(Waiter());
    }

    private IEnumerator Waiter() {
        
        yield return new WaitForSecondsRealtime(4);
        SceneManager.LoadScene(0);
    }

}
