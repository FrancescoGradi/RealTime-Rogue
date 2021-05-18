using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    public List<GameObject> enemies;
    public List<GameObject> spawnPoints;

    public int n_enemies = 2;

    private List<GameObject> actualEnemies = new List<GameObject> {};

    private void Start() {
        
        enemies = Utility.Shuffle(enemies);
        spawnPoints = Utility.Shuffle(spawnPoints);

        for (int i = 0; i < n_enemies; i++) {
            GameObject enemy = Utility.GetRandomObject(enemies);
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