using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    public List<GameObject> enemies;
    public List<GameObject> spawnPoints;

    public GameObject room;
    public LocalCellView localCellView;
    public CustomBrain warriorBrain;
    public CustomBrain wizardBrain;

    public int n_enemies = 2;

    private List<GameObject> actualEnemies = new List<GameObject> {};
    private int roomNumber;

    private void Start() {
        
        enemies = Utility.Shuffle(enemies);
        spawnPoints = Utility.Shuffle(spawnPoints);

        roomNumber = GameManager.instance.GetRoomNumber();

        if (roomNumber < 3)
            n_enemies = 1;
        
        if (roomNumber < 6 && roomNumber >= 3)
            n_enemies = Utility.GetRandomInt(1, 2);
        
        if (roomNumber < 10 && roomNumber >= 6)
            n_enemies = Utility.GetRandomInt(2, 3);
        
        if (roomNumber >= 10)
            n_enemies = Utility.GetRandomInt(2, 4);

        for (int i = 0; i < n_enemies; i++) {
            GameObject enemy = Utility.GetRandomObject(enemies);
            EnemyAgent agent = enemy.GetComponent<EnemyAgent>();

            agent.actualRoom = room;
            agent.localCellView = localCellView;
            agent.target = Player.instance.gameObject;

            if (agent.enemyClass == 0) {
                agent.customBrain = warriorBrain;
            } else {
                agent.customBrain = wizardBrain;
            }

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