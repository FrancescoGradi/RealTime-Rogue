﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    public GameObject mainFirstButton;
    public GameObject optionsMenu;

    private void Start() {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
    }

    public void PlayGame() {
        StartCoroutine(PlayWaiter(0.2f));
    }

    public void ExitGame() {

        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenOptions() {

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsMenu);
    }

    public void CloseOptions() {
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainFirstButton);
    }

    private IEnumerator PlayWaiter(float seconds) {
        yield return new WaitForSecondsRealtime(seconds);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }

}
