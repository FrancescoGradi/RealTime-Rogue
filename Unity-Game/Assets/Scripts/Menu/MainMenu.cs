using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    public GameObject mainFirstButton;
    public GameObject optionsMenuFirstButton;
    public GameObject commandListFirstButton;

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject commandListMenu;

    private void Start() {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
    }

    private void Update() {

        if (Input.GetButtonDown("Fire3")) {

            if (optionsMenu.activeInHierarchy) {
                optionsMenu.SetActive(false);
                mainMenu.SetActive(true);
                CloseOptions();
                
            } else if (commandListMenu.activeInHierarchy) {
                commandListMenu.SetActive(false);
                optionsMenu.SetActive(true);
                CloseCommandList();
            }
        }
    }

    public void PlayGame() {
        StartCoroutine(PlayWaiter(0.2f));
    }

    public void ExitGame() {

        Application.Quit();
    }

    public void OpenOptions() {

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsMenuFirstButton);
    }

    public void CloseOptions() {
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
    }

    public void OpenCommandList() {

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(commandListFirstButton);
    }

    public void CloseCommandList() {

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsMenuFirstButton);
    }

    private IEnumerator PlayWaiter(float seconds) {
        yield return new WaitForSecondsRealtime(seconds);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }

}
