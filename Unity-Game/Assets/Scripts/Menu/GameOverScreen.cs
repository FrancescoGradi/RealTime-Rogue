using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour {
    public GameObject gameOverScreenUI;
    public TMP_Text enemiesKilled;
    public TMP_Text roomOverpassed;
    public GameObject itemsHub;
    public GameObject playerHealthBar;
    public GameObject fps;

    public void GameOver() {

        gameOverScreenUI.SetActive(true);
        itemsHub.SetActive(false);
        playerHealthBar.SetActive(false);
        fps.SetActive(false);

        roomOverpassed.text = "Overpassed rooms: " + (GameManager.instance.GetComponent<GameManager>().GetRoomNumber() - 1).ToString();
        enemiesKilled.text = "Killed enemies: " + GameManager.instance.GetComponent<GameManager>().GetKilledEnemies().ToString();
        StartCoroutine(Waiter());
    }

    private IEnumerator Waiter() {
        
        yield return new WaitForSecondsRealtime(5);
        SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene(0);
    }

}
