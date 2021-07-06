using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WelcomeMenu : MonoBehaviour {

    public static bool gamePaused = false;
    public GameObject firstButton;

    private void Start() {
        this.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);

        Time.timeScale = 0f;
        gamePaused = true;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) {
            if (gamePaused) {
                Resume();
            }
        }
    }

    public void Resume() {
        StartCoroutine(ResumeWaiter(0.2f));
    }

    private IEnumerator ResumeWaiter(float seconds) {
        yield return new WaitForSecondsRealtime(seconds);

        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

}
