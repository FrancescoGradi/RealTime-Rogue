using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {
    public static bool gamePaused = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject commandListMenuUI;

    public GameObject welcomeMenu;

    public GameObject pauseFirstButton;
    public GameObject optionsMenuFirstButton;
    public GameObject commandListMenuFirstButton;

    private bool isInOptionsMenu = false;
    private bool isInCommandListMenu = false;

    void Update() {
        if (!welcomeMenu.gameObject.activeSelf) {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) {
                if (gamePaused && !isInCommandListMenu && !isInOptionsMenu) {
                    Resume();
                } else if (!isInCommandListMenu && !isInOptionsMenu) {
                    Pause();
                }
            } else if (gamePaused && Input.GetButtonDown("Fire3")) {
                if (!isInCommandListMenu && !isInOptionsMenu) {
                    Resume();
                } else if (isInCommandListMenu) {
                    CloseCommandList();
                } else if (isInOptionsMenu) {
                    CloseOptions();
                }
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

    public void OpenOptions() {
        
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);

        isInOptionsMenu = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsMenuFirstButton);
    }

    public void CloseOptions() {

        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);

        isInOptionsMenu = false;
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void OpenCommandList() {

        optionsMenuUI.SetActive(false);
        commandListMenuUI.SetActive(true);

        isInCommandListMenu = true;
        isInOptionsMenu = false;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(commandListMenuFirstButton);
    }

    public void CloseCommandList() {

        commandListMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);

        isInCommandListMenu = false;
        isInOptionsMenu = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsMenuFirstButton);
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
