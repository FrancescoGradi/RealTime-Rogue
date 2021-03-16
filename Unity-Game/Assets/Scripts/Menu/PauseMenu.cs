using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject pauseFirstButton;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) {
            if (gamePaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        StartCoroutine(ResumeWaiter(0.2f));
    }

    private void Pause() {
        pauseMenuUI.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private IEnumerator ResumeWaiter(float seconds) {
        yield return new WaitForSecondsRealtime(seconds);

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }
 
}
