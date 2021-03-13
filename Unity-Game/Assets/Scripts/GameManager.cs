using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject initialRoom;
    public GameObject mainRoom;

    public Player player;

    private GameObject actualRoom;


    private void Start() {
        
        actualRoom = Instantiate(initialRoom, new Vector3(0, initialRoom.gameObject.transform.position.y, 0), Quaternion.identity);
    }

    public void CreateNewRoom(string portalType) {
        Destroy(actualRoom);
        actualRoom = Instantiate(mainRoom, new Vector3(0, 0, 0), Quaternion.identity);

        if (portalType == "T") {
            player.transform.SetPositionAndRotation(new Vector3(0, 0, -12), Quaternion.identity);
        } else if (portalType == "B") {
            player.transform.SetPositionAndRotation(new Vector3(0, 0, 12), Quaternion.Euler(0, -180, 0));
        } else if (portalType == "L") {
            player.transform.SetPositionAndRotation(new Vector3(12, 0, 0), Quaternion.Euler(0, -90, 0));
        } else if (portalType == "R") {
            player.transform.SetPositionAndRotation(new Vector3(-12, 0, 0), Quaternion.Euler(0, 90, 0));
        } else {
            player.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
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
