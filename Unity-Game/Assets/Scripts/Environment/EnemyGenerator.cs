using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    public List<GameObject> enemies;
    public List<GameObject> spawnPoints;

    public GameObject room;
    public LocalCellView localCellView;
    public CustomBrain agentBrain;

    public int n_enemies = 2;

    private List<GameObject> actualEnemies = new List<GameObject> {};

    private void Start() {
        
        enemies = Utility.Shuffle(enemies);
        spawnPoints = Utility.Shuffle(spawnPoints);

        for (int i = 0; i < n_enemies; i++) {
            GameObject enemy = Utility.GetRandomObject(enemies);
            enemy.GetComponent<EnemyAgent>().actualRoom = room;
            enemy.GetComponent<EnemyAgent>().localCellView = localCellView;
            enemy.GetComponent<EnemyAgent>().customBrain = agentBrain;
            enemy.GetComponent<EnemyAgent>().target = Player.instance.gameObject;

            enemy.GetComponent<EnemyMovement>().target = Player.instance.gameObject;

            actualEnemies.Add(Instantiate(enemy, spawnPoints[i].gameObject.transform.position, enemy.gameObject.transform.rotation));
        }

    }
    public void EnemyDown() {

        n_enemies -= 1;
        GameManager.instance.AddEnemyKill();
        if (n_enemies <= 0) {
            FindObjectOfType<Room>().RoomClear();
        }
    }

    private void OnDestroy() {
        foreach (GameObject actualEnemy in actualEnemies) {
            Destroy(actualEnemy);
        }
    }
    
}