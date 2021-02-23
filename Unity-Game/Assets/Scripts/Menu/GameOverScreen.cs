using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverScreenUI;

    public void GameOver() {

        gameOverScreenUI.SetActive(true);
        StartCoroutine(Waiter());
    }

    private IEnumerator Waiter() {
        
        yield return new WaitForSecondsRealtime(4);
        SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene(0);
    }

}
