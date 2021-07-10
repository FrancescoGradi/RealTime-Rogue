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

        int roomNumber = GameManager.instance.GetRoomNumber();

        BalanceN_Enemies(roomNumber);

        for (int i = 0; i < n_enemies; i++) {
            GameObject enemy = Utility.GetRandomObject(enemies);
            EnemyAgent agent = enemy.GetComponent<EnemyAgent>();

            agent.actualRoom = room;
            agent.localCellView = localCellView;
            agent.target = Player.instance.gameObject;

            BalanceEnemy(enemy, roomNumber);

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

    private void BalanceN_Enemies(int roomNumber) {

        if (roomNumber < 3)
            n_enemies = 1;
        
        else if (roomNumber < 6 && roomNumber >= 3)
            n_enemies = Utility.GetRandomInt(1, 2);
        
        else if (roomNumber < 10 && roomNumber >= 6)
            n_enemies = Utility.GetRandomInt(2, 3);
        
        else if (roomNumber >= 10)
            n_enemies = Utility.GetRandomInt(2, 4);
    }

    private void BalanceEnemy(GameObject enemy, int roomNumber) {

        if (roomNumber < 3)
            EditStatsEnemy(enemy.GetComponent<Enemy>(), 20, 4, 3, 3);

        if (roomNumber < 6 && roomNumber >= 3)
            EditStatsEnemy(enemy.GetComponent<Enemy>(), 20, Utility.GetRandomInt(4, 6), Utility.GetRandomInt(3, 5), Utility.GetRandomInt(4, 6));

        if (roomNumber < 10 && roomNumber >= 6)
            EditStatsEnemy(enemy.GetComponent<Enemy>(), 20, Utility.GetRandomInt(6, 8), Utility.GetRandomInt(5, 7), Utility.GetRandomInt(6, 8));
        
        if (roomNumber >= 10)
            EditStatsEnemy(enemy.GetComponent<Enemy>(), 20, Utility.GetRandomInt(8, 10), Utility.GetRandomInt(7, 9), Utility.GetRandomInt(8, 10));

    }

    private void EditStatsEnemy(Enemy enemy, int HP, int ATK, int DEF, int MANA) {
        enemy.HP = HP;
        enemy.currentHealth = HP;
        enemy.ATK = ATK;
        enemy.DEF = DEF;
        enemy.MANA = MANA;
    } 

    private void OnDestroy() {
        foreach (GameObject actualEnemy in actualEnemies) {
            Destroy(actualEnemy);
        }
    }
    
}