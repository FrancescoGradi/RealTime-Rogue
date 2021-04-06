using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour {

    #region Singleton

    public static GameManager instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one Game Manager instance found!");
            return;
        }
        instance = this;
    }

    #endregion

    public GameObject initialRoom;
    public GameObject mainRoom;

    public Player player;

    public TMP_Text roomNumberText;

    private GameObject actualRoom;
    private int initialPortalNum;
    private int roomNumber;


    private void Start() {
        
        roomNumber = 1;
        roomNumberText.text = "Room " + roomNumber.ToString();

        actualRoom = Instantiate(initialRoom, new Vector3(0, initialRoom.gameObject.transform.position.y, 0), Quaternion.identity);
    }

    public void CreateNewRoom(string portalType) {
        StartCoroutine(InstantiationWaiter(portalType));
    }

    private IEnumerator InstantiationWaiter(string portalType) {

        roomNumber += 1;
        roomNumberText.text = "Room " + roomNumber.ToString();
        FindObjectOfType<RoomTransition>().Transition(roomNumberText.text);

        yield return new WaitForSecondsRealtime(1f);

        SetInitialPortalNum(portalType);
        Destroy(actualRoom);
        actualRoom = Instantiate(mainRoom, new Vector3(0, 0, 0), Quaternion.identity);

        if (portalType == "T") {
            player.transform.SetPositionAndRotation(new Vector3(0, 0, -12), Quaternion.identity);
        } else if (portalType == "B") {
            player.transform.SetPositionAndRotation(new Vector3(0, 0, 12), Quaternion.Euler(0, -180, 0));
        } else if (portalType == "R") {
            player.transform.SetPositionAndRotation(new Vector3(-12, 0, 0), Quaternion.Euler(0, 90, 0));
        } else if (portalType == "L") {
            player.transform.SetPositionAndRotation(new Vector3(12, 0, 0), Quaternion.Euler(0, -90, 0));
        } else {
            player.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    public int GetInitialPortalNum() {
        return initialPortalNum;
    }

    private void SetInitialPortalNum(string portalType) {

        if (portalType == "T") {
            initialPortalNum = 1;
        } else if (portalType == "B") {
            initialPortalNum = 0;
        } else if (portalType == "R") {
            initialPortalNum = 2;
        } else if (portalType == "L") {
            initialPortalNum = 3;
        } else {
            initialPortalNum = 0;
        }
    }

    private void EndGame() {
        Debug.Log("WIN");
        FindObjectOfType<WinScreen>().YouWin();
    }

    public void GameOver() {
        Debug.Log("GAME OVER");
        FindObjectOfType<GameOverScreen>().GameOver();
    }

}
